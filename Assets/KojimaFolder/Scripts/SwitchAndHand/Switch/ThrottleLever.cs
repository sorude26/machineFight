using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// レバーの位置によってvalueが0～1に変化
/// 複数個のクリックポイント(レバーを上げ下げするときに抵抗を感じるポイント)を設定できる
/// </summary>
public class ThrottleLever : Switch
{
    const float LEVER_LEAP_SPEED = 0.2f;
    [SerializeField]
    Transform _minPosition;
    [SerializeField]
    Transform _maxPosition;
    [SerializeField]
    Transform _leverBody;

    protected override void Awake()
    {
        base.Awake();
        //レバー本体を移動させる
        _leverBody.position = Vector3.Lerp(_minPosition.position, _maxPosition.position, _value);
    }

    public float GetValueByZone(int zone)
    {
        return _value;
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
        _value = value;

        //レバー本体を移動させる
        _leverBody.position = minPos + minToMax * value;
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
