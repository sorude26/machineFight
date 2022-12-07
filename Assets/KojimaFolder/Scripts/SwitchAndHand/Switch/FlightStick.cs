using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    Vector3 _currentStickRotate;

    /// <summary>
    /// フライトスティック、本体の入力を取る
    /// </summary>
    /// <returns></returns>
    public Vector2 GetStickBodyInput()
    {
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
        Vector3 local = (Quaternion.Inverse(this.transform.rotation) * _lockinHand.transform.parent.rotation).eulerAngles.NomalizeRotate();
        local.y = 0;
        //回転がスティックの限界値を超えないように
        local.x = Mathf.Min(local.x, STICK_ANGLE_MAX);
        local.x = Mathf.Max(local.x, -STICK_ANGLE_MAX);
        local.z = Mathf.Min(local.z, STICK_ANGLE_MAX);
        local.z = Mathf.Max(local.z, -STICK_ANGLE_MAX);
        //回転適用
        _stickRotateBase.localRotation = Quaternion.Euler(local);
        _currentStickRotate = local;
    }
}
