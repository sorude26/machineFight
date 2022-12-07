using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���̃N���X�͊��N���X��ONOFF�̒l���g��Ȃ��A�e���암���̓��͂͌ʂ̊֐��Ŏ���B
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
    /// �t���C�g�X�e�B�b�N�A�{�̂̓��͂����
    /// </summary>
    /// <returns></returns>
    public Vector2 GetStickBodyInput()
    {
        return new Vector2(_currentStickRotate.x / STICK_ANGLE_MAX, _currentStickRotate.z / STICK_ANGLE_MAX);
    }

    /// <summary>
    /// �t���C�g�X�e�B�b�N�A�T���X�e�B�b�N�̓��͂����
    /// </summary>
    /// <returns></returns>
    public Vector2 GetThumbsStickInput()
    {
        if (_lockinHand == null) return Vector2.zero;
        return OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, _lockinHand.ControllerType);
    }

    /// <summary>
    /// �t���C�g�X�e�B�b�N�A�l�����w�g���K�[�̓��͂����
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
        //��]���X�e�B�b�N�̌��E�l�𒴂��Ȃ��悤��
        local.x = Mathf.Min(local.x, STICK_ANGLE_MAX);
        local.x = Mathf.Max(local.x, -STICK_ANGLE_MAX);
        local.z = Mathf.Min(local.z, STICK_ANGLE_MAX);
        local.z = Mathf.Max(local.z, -STICK_ANGLE_MAX);
        //��]�K�p
        _stickRotateBase.localRotation = Quaternion.Euler(local);
        _currentStickRotate = local;
    }
}
