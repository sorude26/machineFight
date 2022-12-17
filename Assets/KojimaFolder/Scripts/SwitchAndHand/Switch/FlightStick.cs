using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

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

    Vector3 _currentStickValue;
    //�t���C�g�X�e�B�b�N����
    Vector3 CurrentStickValue { get => _currentStickValue; set => _currentStickValue = value / STICK_ANGLE_MAX; }

    public override HoldTypes HoldType => HoldTypes.Grab;

    /// <summary>
    /// �t���C�g�X�e�B�b�N�A�{�̂̓��͂����
    /// </summary>
    /// <returns></returns>
    public Vector2 GetStickBodyInput()
    {
        if (_lockinHand == null) return Vector2.zero;
        return new Vector2(-CurrentStickValue.z, CurrentStickValue.x);
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
    public bool GetTriggerInput(bool down)
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

    /// <summary>
    /// �t���C�g�X�e�B�b�N�A�R���g���[���[�㕔�̃{�^���̓��͂����
    /// </summary>
    /// <param name="down"></param>
    /// <returns></returns>
    public bool GetUpperButtonInput(bool down)
    {
        if (_lockinHand == null) return false;
        if (down)
        {
            return OVRInput.GetDown(OVRInput.Button.Two, _lockinHand.ControllerType);
        }
        else
        {
            return OVRInput.Get(OVRInput.Button.Two, _lockinHand.ControllerType);
        }
    }
    /// <summary>
    /// �t���C�g�X�e�B�b�N�A�R���g���[���[�����̃{�^���̓��͂����
    /// </summary>
    /// <param name="down"></param>
    /// <returns></returns>
    public bool GetLowerButtonInput(bool down)
    {
        if (_lockinHand == null) return false;
        if (down)
        {
            return OVRInput.GetDown(OVRInput.Button.One, _lockinHand.ControllerType);
        }
        else
        {
            return OVRInput.Get(OVRInput.Button.One, _lockinHand.ControllerType);
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
        //�X�e�B�b�N�̓������̂ɃX���[�W���O�����������̂ŁA���ʂȏ������s��

        //
        //�ǋL���A��x�X�e�B�b�N�̉�]����̐��K�̈ʒu��񂩂瓾��B
        //
        Vector3 stickRotateRaw = (Quaternion.Inverse(this.transform.rotation) * _lockinHand.transform.parent.rotation).eulerAngles.NomalizeRotate();
        stickRotateRaw.y = 0;
        //��]���X�e�B�b�N�̌��E�l�𒴂��Ȃ��悤��
        stickRotateRaw.x = Mathf.Min(stickRotateRaw.x, STICK_ANGLE_MAX);
        stickRotateRaw.x = Mathf.Max(stickRotateRaw.x, -STICK_ANGLE_MAX);
        stickRotateRaw.z = Mathf.Min(stickRotateRaw.z, STICK_ANGLE_MAX);
        stickRotateRaw.z = Mathf.Max(stickRotateRaw.z, -STICK_ANGLE_MAX);
        //��UStick�̉�]��K�p
        _stickRotateBase.localRotation = Quaternion.Euler(stickRotateRaw);
        //
        //�ǋL���I��
        //

        //rotate
        Quaternion myRotate = GetMyHoldRotation();
        //�X�C�b�`�ʒu����̑��ΓI�ȉ�]��؂�o���A�e����Weight��������
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

        //��]�K�p
        Quaternion newRotate = myRotate * Quaternion.Euler(currentHandRotateEuler);
        newRotate = Quaternion.Lerp(_currentHandRotate, newRotate, ROTATE_LERP_SPEED);
        _lockinHand.transform.rotation = newRotate;
        _currentHandRotate = newRotate;

        //
        //�ǋL���A������x�X�e�B�b�N�̉�]��K�p����
        //
        Vector3 stickRotate = (Quaternion.Inverse(this.transform.rotation) * newRotate).eulerAngles.NomalizeRotate();
        stickRotateRaw.y = 0;
        //��]���X�e�B�b�N�̌��E�l�𒴂��Ȃ��悤��
        stickRotate.x = Mathf.Min(stickRotate.x, STICK_ANGLE_MAX);
        stickRotate.x = Mathf.Max(stickRotate.x, -STICK_ANGLE_MAX);
        stickRotate.z = Mathf.Min(stickRotate.z, STICK_ANGLE_MAX);
        stickRotate.z = Mathf.Max(stickRotate.z, -STICK_ANGLE_MAX);
        //Stick�̉�]��K�p
        _stickRotateBase.localRotation = Quaternion.Euler(stickRotate);
        //�t���C�g�X�e�B�b�N���͂��O���������悤�ɕۑ�
        CurrentStickValue = stickRotate;
        //
        //�ǋL���I��
        //


        //move
        _handMove = Vector3.Lerp(_handMove, _holdPosition.position - this.transform.position, MOVE_LERP_SPEED);
        _lockinHand.transform.position = _handMove + _lockinHand.transform.position - _lockinHand.HoldPosition(HoldType) + this.transform.position;
    }
}
