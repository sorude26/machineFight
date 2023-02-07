using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �I���̎��̓��o�[��y��������A�I�t�̎���y���������ɌX���悤�Ƀv���n�u���Ȃ��ƃo�O����������
/// </summary>
public class ToggleSwitch : Switch
{
    enum AdvancedGrabType
    {
        Nomal,
        Thumb,
        Index
    }
    [SerializeField]
    Transform _onPosition;
    [SerializeField]
    Transform _offPosition;
    [SerializeField]
    Transform _switchMovablePartObject;
    [SerializeField]
    HandPoseWeights _thumbHandPose;
    [SerializeField]
    HandPoseWeights _IndexHandPose;
    AdvancedGrabType _currentAdvancedGrabType;

    public override HoldTypes HoldType 
    { get
        {
            switch (_currentAdvancedGrabType)
            {
                case AdvancedGrabType.Nomal:
                    return HoldTypes.Pinch;
                case AdvancedGrabType.Thumb:
                    return HoldTypes.Thumb;
                case AdvancedGrabType.Index:
                    return HoldTypes.Pinch;
                default:
                    Debug.LogError("ToggleSwitch�ŃG���[");
                    return HoldTypes.Pinch;
            }
        } 
    }

    public override HandPoseWeights GetPose()
    {
        switch (_currentAdvancedGrabType)
        {
            case AdvancedGrabType.Nomal:
                return base.GetPose();
            case AdvancedGrabType.Thumb:
                return _thumbHandPose;
            case AdvancedGrabType.Index:
                return _IndexHandPose;
            default:
                Debug.LogError("ToggleSwitch.GetPose�ŃG���[");
                return base.GetPose();
        }
    }

    [ContextMenu("TurnOn")]
    void TurnOnTest()
    {
        TurnOn();
    }
    [ContextMenu("TurnOff")]
    void TurnOffTest()
    {
        TurnOff();
    }

    public override void TurnOn(bool isInit = false)
    {
        base.TurnOn(isInit);
        _switchMovablePartObject.transform.parent = _onPosition;
        _switchMovablePartObject.transform.localPosition = Vector3.zero;
        _switchMovablePartObject.transform.localRotation = Quaternion.identity;
    }

    
    public override void TurnOff(bool isInit = false)
    {
        base.TurnOff(isInit);
        _switchMovablePartObject.transform.parent = _offPosition;
        _switchMovablePartObject.transform.localPosition = Vector3.zero;
        _switchMovablePartObject.transform.localRotation = Quaternion.identity;
    }



    protected override void Awake()
    {
        base.Awake();
        TurnOff(true);
    }

    protected override void SwitchUpdateOnLockIn()
    {
        base.SwitchUpdateOnLockIn();
        Vector3 local = transform.InverseTransformPoint(_lockinHand.ReferencePositionWorld);
        bool _beforeValue = IsOn;
        if (local.y > 0)
        {
            TurnOn();
        }
        else
        {
            TurnOff();
        }

        if (base.GetHoldInInput(_lockinHand, HoldTypes.Pinch))
        {
            _currentAdvancedGrabType = AdvancedGrabType.Nomal;
        }
        //�e�����͂̏ꍇ�͒l���ς��������Free����
        if ((IsOn != _beforeValue) && (_currentAdvancedGrabType != AdvancedGrabType.Nomal))
        {
            Free();
        }
    }

    protected override bool GetHoldInInput(SwitchCtrlHand from, HoldTypes hold)
    {
        //��UNomal�ɂ��Ă����Ȃ��Ɠ��͂����܂������Ȃ��\��������
        _currentAdvancedGrabType = AdvancedGrabType.Nomal;
        //�ʏ�̂܂ݓ��͂�����΂��̂܂ܕԋp
        if (base.GetHoldInInput(from, hold))
        {
            _currentAdvancedGrabType = AdvancedGrabType.Nomal;
            return true;
        }

        //�ȉ��e�����͔��ʏ���
        bool thumbInput = OVRInput.GetDown(OVRInput.Button.Two, from.ControllerType);
        bool indexInput = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, from.ControllerType);
        Vector3 rotate = (from.transform.parent.rotation * Quaternion.Inverse(this.transform.rotation))
            .eulerAngles;
        float rotateZ = rotate.NomalizeRotate180().z;
        if (IsOn)
        {
            //On��ԂȂ̂Őe�w���l�����w���ォ�牟��������`�ɂȂ��ē��͂����Ă��邩�`�F�b�N
            //�e�w(-90�`90)
            if (rotateZ < 90f && rotateZ > -90 && thumbInput)
            {
                _currentAdvancedGrabType = AdvancedGrabType.Thumb;
                return true;
            }

            //�l�����w
            //�E��ƍ���Ŋp�x���Ⴄ�̂Ŗ�� ����=>-180�`0�̊ԂŔ���A �E��=>0�`180�̊ԂŔ���
            if (from.IsRightHand)
            {
                //�E��(0�`180) �p�x��-180�`180�Ő��K������Ă���̂Ő��̒l�����ʂ��邾���ŗǂ�
                if (rotateZ > 0 && indexInput)
                {
                    _currentAdvancedGrabType = AdvancedGrabType.Index;
                    return true;
                }
            }
            else
            {
                //����(0�`-180) �p�x��-180�`180�Ő��K������Ă���̂ŕ��̒l�����ʂ��邾���ŗǂ�
                if (rotateZ < 0 && indexInput)
                {
                    _currentAdvancedGrabType = AdvancedGrabType.Index;
                    return true;
                }
            }
        }
        else
        {
            //Off��ԂȂ̂Őe�w���l�����w�������牟���グ��`�ɂȂ��Ă��邩�`�F�b�N

            //�e�w(90�`270)
            float zeroTo360 = rotateZ < 0f ? 360f + rotateZ : rotateZ;
            if (zeroTo360 > 90f && zeroTo360 < 270f && thumbInput)
            {
                _currentAdvancedGrabType = AdvancedGrabType.Thumb;
                return true;
            }

            //�l�����w
            if (from.IsRightHand)
            {
                //�E��(-180�`0)
                if (rotateZ < 0 && indexInput)
                {
                    _currentAdvancedGrabType = AdvancedGrabType.Index;
                    return true;
                }
            }
            else
            {
                //����(0�`180)
                if (rotateZ > 0 && indexInput)
                {
                    _currentAdvancedGrabType = AdvancedGrabType.Index;
                    return true;
                }
            }
        }
        return false;
    }

    protected override bool GetFreeInput(SwitchCtrlHand from, HoldTypes hold)
    {
        switch (_currentAdvancedGrabType)
        {
            case AdvancedGrabType.Nomal:
                return base.GetFreeInput(from, hold);
            case AdvancedGrabType.Thumb:
                return OculusGameInput.GetThumbOut(from.ControllerType);
            case AdvancedGrabType.Index:
                return !OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, from.ControllerType);
            default:
                Debug.LogError("ToggleSwitch�ŃG���[");
                return true;
        }
    }
}
