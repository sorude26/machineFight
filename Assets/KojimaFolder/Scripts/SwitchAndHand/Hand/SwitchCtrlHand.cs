using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Switch;

public class SwitchCtrlHand : MonoBehaviour
{
    public enum HandState
    {
        Free,
        LockIn,
    }
    const float ROTATE_LERP_SPEED = 0.15f;
    const float MOVE_LERP_SPEED = 0.15f;

    [SerializeField]
    OVRInput.Controller _controller;
    [SerializeField]
    HandTouchCollision _handTouchCollision;
    [SerializeField]
    Transform _pinchPosition;
    [SerializeField]
    Transform _grabPosition;
    [SerializeField]
    Transform _thumbPosition;
    Switch _lockingSwitch;
    HandState _state;
    //�͂݌�̎�̈ړ������邽�߂̊�_
    Transform _referencePosition;

    public HandState State => _state;
    public Switch LockingSwitch => _lockingSwitch;
    public OVRInput.Controller ControllerType => _controller;
    public bool IsRightHand => _controller == OVRInput.Controller.RTouch;
    public Vector3 ReferencePositionWorld => _referencePosition.position;
    public Vector3 HoldPosition(Switch.HoldTypes type)
    {
        switch (type)
        {
            case HoldTypes.Pinch:
                return _pinchPosition.position;
            case HoldTypes.Grab:
                return _grabPosition.position;
            case HoldTypes.Thumb:
                return _thumbPosition.position;
            default:
                Debug.LogError("��O���������Ă��܂�");
                return Vector3.zero;
        }
    }
    public void LockIn(Switch target)
    {
        LockInImple(target);
    }
    public void Free()
    {
        FreeImple();
    }


    private void Awake()
    {
        _referencePosition = new GameObject().transform;
        _referencePosition.SetParent(this.transform.parent);
    }
    private void Update()
    {
        if (_state == HandState.Free)
        {
            FreeUpdate();
        }
    }
    private void LateUpdate()
    {
        if (_state == HandState.LockIn)
        {
            LockInUpdate();
        }
    }
    private void LockInImple(Switch target)
    {
        _state = HandState.LockIn;
        _lockingSwitch = target;
        SetReferencePosition(target);
    }
    private void FreeImple()
    {
        _state = HandState.Free;
        _lockingSwitch = null;
    }
    private void FreeUpdate()
    {
        //��̈ʒu�A��]�����[�v�œ�����
        this.transform.position = Vector3.Lerp(this.transform.position, this.transform.parent.position, MOVE_LERP_SPEED);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, this.transform.parent.rotation, ROTATE_LERP_SPEED);
        //�X�C�b�`��T��
        Switch activeSwitch = _handTouchCollision.GetActiveSwitch();
        //�G���
        activeSwitch?.Touch(this);
    }
    private void LockInUpdate()
    {
        //�ΏۃX�C�b�`���Ȃ��Ȃ��Ă����ꍇ��Free()
        if (_lockingSwitch == null)
        {
            FreeImple();
            return;
        }
        _lockingSwitch.LockInUpdate();
    }

    //Switch�̃I���I�t����Ɏg����_�̐ݒ�
    void SetReferencePosition(Switch target)
    {
        switch (target.HoldType)
        {
            case HoldTypes.Pinch:
                _referencePosition.position = _pinchPosition.position;
                break;
            case HoldTypes.Grab:
                _referencePosition.position = _grabPosition.position;
                break;
            case HoldTypes.Thumb:
                _referencePosition.position = _thumbPosition.position;
                break;
            default:
                Debug.LogError("��O���������Ă��܂�");
                break;

        }
    }
}
