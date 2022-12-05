using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���̃N���X�͊��N���X��ONOFF�̒l���g��Ȃ��A�e���암���̓��͂͌ʂ̊֐��Ŏ���B
/// </summary>
public class FlightStick : Switch
{
    [SerializeField]
    float _holdMoveOffsetX;

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
}
