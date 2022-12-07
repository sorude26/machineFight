using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

/// <summary>
/// このクラスは基底クラスのONOFFの値を使わない、各操作部分の入力は個別の関数で取れる。
/// </summary>
public class FlightStick : Switch
{
    const float STICK_ANGLE_MAX = 45f;

    [SerializeField]
    float _holdMoveOffsetX;
    [SerializeField]
    Transform _stickRotateBase;

    //フライトスティック入力
    Vector3 _currentStickRotate;

    /// <summary>
    /// フライトスティック、本体の入力を取る
    /// </summary>
    /// <returns></returns>
    public Vector2 GetStickBodyInput()
    {
        if (_lockinHand == null) return Vector2.zero;
        return new Vector2(_currentStickRotate.x / STICK_ANGLE_MAX, _currentStickRotate.z / STICK_ANGLE_MAX);
    }

    /// <summary>
    /// フライトスティック、サムスティックの入力を取る
    /// </summary>
    /// <returns></returns>
    public Vector2 GetThumbsStickInput()
    {
        if (_lockinHand == null) return Vector2.zero;
        return OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, _lockinHand.ControllerType);
    }

    /// <summary>
    /// フライトスティック、人差し指トリガーの入力を取る
    /// </summary>
    /// <param name="down"></param>
    /// <returns></returns>
    public bool GetTriggerInput(bool down = false)
    {
        if (_lockinHand == null) return false;
        if (down)
        {
            return OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, _lockinHand.ControllerType);
        }
        else
        {
            return OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, _lockinHand.ControllerType);
        }
    }

    protected override void LockIn(SwitchCtrlHand from)
    {
        float offset = _holdMoveOffsetX;
        if (!from.IsRightHand)
        {
            offset = -offset;
        }
        var pos = _holdPosition.localPosition;
        pos.x = offset;
        _holdPosition.localPosition = pos;
        base.LockIn(from);
    }

    protected override void SwitchUpdateOnLockIn()
    {
        base.SwitchUpdateOnLockIn();
        
    }

    protected override void MoveRotateUpdateOnLockIn()
    {
        //スティックの動き自体にスムージングをかけたいので、特別な処理を行う

        //
        //追記部、一度スティックの回転を手の正規の位置情報から得る。
        //
        Vector3 stickRotateRaw = (Quaternion.Inverse(this.transform.rotation) * _lockinHand.transform.parent.rotation).eulerAngles.NomalizeRotate();
        stickRotateRaw.y = 0;
        //回転がスティックの限界値を超えないように
        stickRotateRaw.x = Mathf.Min(stickRotateRaw.x, STICK_ANGLE_MAX);
        stickRotateRaw.x = Mathf.Max(stickRotateRaw.x, -STICK_ANGLE_MAX);
        stickRotateRaw.z = Mathf.Min(stickRotateRaw.z, STICK_ANGLE_MAX);
        stickRotateRaw.z = Mathf.Max(stickRotateRaw.z, -STICK_ANGLE_MAX);
        //一旦Stickの回転を適用
        _stickRotateBase.localRotation = Quaternion.Euler(stickRotateRaw);
        //
        //追記部終了
        //

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

        //
        //追記部、もう一度スティックの回転を適用する
        //
        Vector3 stickRotate = (Quaternion.Inverse(this.transform.rotation) * newRotate).eulerAngles.NomalizeRotate();
        stickRotateRaw.y = 0;
        //回転がスティックの限界値を超えないように
        stickRotate.x = Mathf.Min(stickRotate.x, STICK_ANGLE_MAX);
        stickRotate.x = Mathf.Max(stickRotate.x, -STICK_ANGLE_MAX);
        stickRotate.z = Mathf.Min(stickRotate.z, STICK_ANGLE_MAX);
        stickRotate.z = Mathf.Max(stickRotate.z, -STICK_ANGLE_MAX);
        //Stickの回転を適用
        _stickRotateBase.localRotation = Quaternion.Euler(stickRotate);
        //フライトスティック入力を外部から取れるように保存
        _currentStickRotate = stickRotate;
        //
        //追記部終了
        //


        //move
        _handMove = Vector3.Lerp(_handMove, _holdPosition.position, MOVE_LERP_SPEED);
        _lockinHand.transform.position = _handMove + _lockinHand.transform.position - _lockinHand.HoldPosition(_holdType);
    }
}
