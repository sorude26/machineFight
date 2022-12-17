using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// レバーの位置によってvalueが0～1に変化
/// 複数個のクリックポイント(レバーを上げ下げするときに抵抗を感じるポイント)を外部から設定できる
/// OnExitZoneとOnEnterZoneコールバックを外部から登録可能
/// </summary>
public class ThrottleLever : Switch
{
    const float LEVER_LEAP_SPEED = 0.2f;
    const float CLICK_FORCE = 0.02f;
    [SerializeField]
    Transform _minPosition;
    [SerializeField]
    Transform _maxPosition;
    [SerializeField]
    Transform _leverBody;

    //昇順
    [SerializeField]
    List<float> _clickPoints = new List<float>();
    float _valueByZone = 0;
    int _currentZone = 0;

    protected override void Awake()
    {
        base.Awake();
        //レバー本体を移動させる
        _leverBody.position = Vector3.Lerp(_minPosition.position, _maxPosition.position, _value);
        //ゾーンの値を初期化
        ChangeValue(_value);
    }

    public event Action<int> OnExitZone;
    public event Action<int> OnEnterZone;
    public int ZoneCount => _clickPoints.Count + 1;
    public int CurrentZone => _currentZone;
    public float ValueByZone => _valueByZone;
    /// <summary>
    /// レバーを上げ下げするときに抵抗を感じるポイントを設定する
    /// </summary>
    /// <param name="points">0～1の範囲内で設定されるポイントの位置</param>
    public void SetClickPointsAsNew(float[] points)
    {
        //昇順に並び替えて保管
        _clickPoints = points.Where(p => p < 1f && p > 0f).OrderBy(p => p).ToList();
        //ゾーン計算
        ChangeValue(_value);
    }


    private void SetValueByZone(float value)
    {
        float r = value;
        float min = 0;
        if (_currentZone != 0)
        {
            min = _clickPoints[_currentZone - 1];
        }
        float max = 1;
        if (_currentZone != _clickPoints.Count)
        {
            max = _clickPoints[_currentZone];
        }

        r -= min;
        r /= (max - min);
        _valueByZone = r;
    }



    private int GetZone(float value)
    {
        int zone =  0;
        foreach (var point in _clickPoints)
        {
            if (point > value)
            {
                return zone;
            }
            ++zone;
        }
        return zone;
    }

    private void ChangeValue(float value)
    {
        if (_clickPoints.Count == 0)
        {
            //区分けされているゾーンが存在しないならばそのまま値を適用して終了
            _value = value;
            _valueByZone = value;
            return;
        }


        int current = _currentZone;
        int newZone = GetZone(value);
        if (current == newZone)
        {
            //ゾーンを変える必要がないためそのまま値を適用して終了する
            _value = value;
            //ValueByZone適用
            SetValueByZone(value);
            return;
        }

        if (current < newZone)
        {
            //レバーが上に行っているとき
            float max = _clickPoints[current];
            //敷居より上へ行った場合はゾーン変更
            if (max < value - CLICK_FORCE)
            {
                //ゾーン変更適用
                ChangeZone(value, newZone);
                //ValueByZone適用
                SetValueByZone(value);
                return;
            }
            _value = max;
            _valueByZone = 1;
            return;
        }
        else
        {
            //レバーが下に行っているとき
            float min = _clickPoints[current - 1];
            //敷居より下へ行った場合はゾーン変更
            if (min > value + CLICK_FORCE)
            {
                //ゾーン変更適用
                ChangeZone(value, newZone);
                //ValueByZone適用
                SetValueByZone(value);
                return;
            }
            _value = min;
            _valueByZone = 0;
            return;
        }
    }
    private void ChangeZone(float value, int newZone)
    {
        _value = value;
        //ゾーンが変わっていない場合はここから下の処理は不要
        if (_currentZone == newZone) return;

        //ゾーン変更時のコールバック実行
        OnExitZone?.Invoke(_currentZone);
        OnEnterZone?.Invoke(newZone);
        //ゾーン変更
        _currentZone = newZone;
        //エフェクト処理
        Vibrate();
        _audioSource?.PlayOneShot(_audioClip);
    }

    protected override void SwitchUpdateOnLockIn()
    {
        base.SwitchUpdateOnLockIn();
        //各座標を取る
        Vector3 handPos = _lockinHand.transform.parent.position;
        Vector3 minPos = _minPosition.position;
        Vector3 maxPos = _maxPosition.position;
        Vector3 minToMax = maxPos - minPos;
        //内積で値を取り出す
        float handSqrLength = Vector3.Dot(handPos - minPos, minToMax.normalized);
        if (handSqrLength < 0)
        {
            handSqrLength *= handSqrLength;
            handSqrLength = -handSqrLength;
        }
        else
        {
            handSqrLength *= handSqrLength;
        }
        float sqrLength = minToMax.sqrMagnitude;
        //値を算出
        float value = handSqrLength / sqrLength;
        //Lerpさせて値の変化をなめらかに
        value = Mathf.Lerp(_value, value, LEVER_LEAP_SPEED);
        value = Mathf.Max(0, value);
        value = Mathf.Min(1, value);
        //値を適用
        ChangeValue(value);

        //レバー本体を移動させる
        _leverBody.position = minPos + minToMax * _value;
    }

    protected override void LockIn(SwitchCtrlHand from)
    {
        from.LockIn(this);
        _lockinHand = from;
        //rotate
        _currentHandRotate = from.transform.rotation;
        //move(改変部)
        _handMove = from.HoldPosition(_holdType) - _holdPosition.transform.position;
    }

    protected override void MoveRotateUpdateOnLockIn()
    {
        //rotate
        Quaternion myRotate = GetMyHoldRotation();
        //スイッチ位置からの相対的な回転を切り出し、各軸にWeightをかける
        Vector3 currentHandRotateEuler = (Quaternion.Inverse(myRotate) * _lockinHand.transform.parent.rotation).eulerAngles.NomalizeRotate();
        currentHandRotateEuler.x = SetWeight(currentHandRotateEuler.x, _rotateLockX);
        currentHandRotateEuler.y = SetWeight(currentHandRotateEuler.y, _rotateLockY);
        currentHandRotateEuler.z = SetWeight(currentHandRotateEuler.z, _rotateLockZ);
        static float SetWeight(float current, float weight)
        {
            if (weight == 0f) return current;
            //Weight
            current *= (1 - weight);
            return current;
        }

        //回転適用
        Quaternion newRotate = myRotate * Quaternion.Euler(currentHandRotateEuler);
        newRotate = Quaternion.Lerp(_currentHandRotate, newRotate, ROTATE_LERP_SPEED);
        _lockinHand.transform.rotation = newRotate;
        _currentHandRotate = newRotate;

        //move(改変部)
        _handMove = Vector3.Lerp(_handMove, Vector3.zero, MOVE_LERP_SPEED);
        _lockinHand.transform.position = _handMove + _lockinHand.transform.position - _lockinHand.HoldPosition(_holdType) + _holdPosition.position;
    }
}
