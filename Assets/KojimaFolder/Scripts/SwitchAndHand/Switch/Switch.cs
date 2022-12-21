using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class Switch : MonoBehaviour
{
    const float VIBE_SPEED = 0.05f;
    const float VIBE_POWER = 0.5f;
    const float VIBE_TIME = 0.05f;
    protected const float SWITCH_VALUE_MAX = 1;
    protected const float SWITCH_VALUE_MIN = 0;
    protected const float SWITCH_VALUE_MID = 0.5f;
    protected const float ROTATE_LERP_SPEED = 0.2f;
    protected const float MOVE_LERP_SPEED = 0.5f;
    public enum HoldTypes
    {
        Pinch,
        Grab,

        //�e�w�e�����͗p�̓��ʂȂ���
        Thumb,
    }
    static List<Switch> _switches = new List<Switch>();

    [SerializeField]
    protected Transform _holdPosition;
    [SerializeField]
    protected Transform _holdRotation;
    protected SwitchCtrlHand _lockinHand;
    [SerializeField]
    HandPoseWeights _poseOnHold;
    [SerializeField]
    bool _isReverseHoldOnRight = true;
    [Header("�y�b�g�{�g���J����Ƃ��̂Ђ˂�")]
    [SerializeField, Range(0.0f, 1.0f)]
    protected float _rotateLockX = 0f;
    [Header("�G�Ѝi��̂Ђ˂�")]
    [SerializeField, Range(0.0f, 1.0f)]
    protected float _rotateLockY = 0f;
    [Header("�����̂Ђ˂�")]
    [SerializeField, Range(0.0f, 1.0f)]
    protected float _rotateLockZ = 0f;
    [SerializeField]
    protected AudioClip _audioClip;

    protected AudioSource _audioSource;
    protected Vector3 _handMove = Vector3.zero;
    protected Quaternion _currentHandRotate = Quaternion.identity;
    

    //�X�C�b�`�̏��(��{�I��0�`1�ŊǗ�)
    protected float _value = 0;

    public event Action OnTurnOn;
    public event Action OnTurnOff;
    public event Action OnInit;
    public bool IsOn => _value > SWITCH_VALUE_MID;
    /// <summary>
    /// �X�C�b�`�̏�ԁA��{�I�ɂ�0�`1
    /// </summary>
    public virtual float Value => _value;
    public abstract HoldTypes HoldType { get; }
    public Transform HoldPosition => _holdPosition;

    public static IEnumerable<Switch> SwitchList => _switches;
    public virtual HandPoseWeights GetPose() => _poseOnHold;

    #region HAND_INPUT
    //�t���C�g�X�e�B�b�N��A�X���b�g�����o�[�ɂ����Ĉ�������ɓ��͂����邩�`�F�b�N���鎞�Ɏg�p
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
    public bool GetThumbstickButtonInput(bool down)
    {
        if (_lockinHand == null) return false;
        if (down)
        {
            return OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick, _lockinHand.ControllerType);
        }
        else
        {
            return OVRInput.Get(OVRInput.Button.PrimaryThumbstick, _lockinHand.ControllerType);
        }
    }
    public Vector2 GetThumbstickInput()
    {
        if (_lockinHand == null) return Vector2.zero;
        return OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, _lockinHand.ControllerType);
    }
    #endregion

    public void Touch(SwitchCtrlHand from)
    {
        TouchImple(from);
    }
    public void LockInUpdate()
    {
        LockInUpdateImple();
    }
    public void Init()
    {
        TurnOff(true);
    }
    public virtual void TurnOn(bool isInit = false)
    {
        if (IsOn && !isInit) return;//���ł�on�̏ꍇ�͉������Ȃ�;
        _value = SWITCH_VALUE_MAX;
        if (!isInit) OnTurnOn?.Invoke();
        else OnInit?.Invoke();
    }
    public virtual void TurnOff(bool isInit = false)
    {
        if (!IsOn && !isInit) return;//���ł�off�̏ꍇ�͉������Ȃ�;
        _value = SWITCH_VALUE_MIN;
        if(!isInit) OnTurnOff?.Invoke();
        else OnInit?.Invoke();
    }


    protected virtual void Awake()
    {
        _switches.Add(this);
        _audioSource = GetComponent<AudioSource>();
        OnTurnOn += () => _audioSource?.PlayOneShot(_audioClip);
        OnTurnOff += () => _audioSource?.PlayOneShot(_audioClip);
        OnTurnOn += Vibrate;
        OnTurnOff += Vibrate;

    }
    private void OnDestroy()
    {
        _switches.Remove(this);
        if (_lockinHand != null)
        {
            Free();
        }
    }
    private void OnEnable()
    {
        if (!_switches.Contains(this))
        {
            _switches.Add(this);
        }
    }
    private void OnDisable()
    {
        if (_switches.Contains(this))
        {
            _switches.Remove(this);
        }
        if (_lockinHand != null)
        {
            Free();
        }
    }
    protected virtual void LockIn(SwitchCtrlHand from)
    {
        from.LockIn(this);
        _lockinHand = from;
        //rotate
        _currentHandRotate = from.transform.rotation;
        //move
        _handMove = from.HoldPosition(HoldType) - this.transform.position;
    }
    protected virtual void Free()
    {
        _lockinHand?.Free();
        _lockinHand = null;
    }
    protected virtual void TouchImple(SwitchCtrlHand from)
    {
        //���ł�lockIn����Ă���ꍇ�͉����������Ȃ�
        if (_lockinHand != null) return;
        //���ݓ��͂������LockIn
        if (GetHoldInInput(from, HoldType))
        {
            LockIn(from);
        }
    }
    protected virtual void LockInUpdateImple()
    {
        //����锻������Free
        if (GetFreeInput(_lockinHand, HoldType) || !this.gameObject.activeInHierarchy)
        {
            Free();
            return;
        }
        SwitchUpdateOnLockIn();
        if (_lockinHand == null)
        {
            Free();
            return;
        }
        MoveRotateUpdateOnLockIn();
    }

    protected virtual void SwitchUpdateOnLockIn()
    {

    }

    protected virtual void MoveRotateUpdateOnLockIn()
    {
        //rotate
        Quaternion myRotate = GetMyHoldRotation();
        //�X�C�b�`�ʒu����̑��ΓI�ȉ�]��؂�o���A�e����Weight��������
        Vector3 currentHandRotateEuler = (Quaternion.Inverse(myRotate) * _lockinHand.transform.parent.rotation).eulerAngles.NomalizeRotate180();
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

        //move
        //move
        _handMove = Vector3.Lerp(_handMove, _holdPosition.position - this.transform.position, MOVE_LERP_SPEED);
        _lockinHand.transform.position = _handMove + _lockinHand.transform.position - _lockinHand.HoldPosition(HoldType) + this.transform.position;
    }

    protected Quaternion GetMyHoldRotation()
    {
        //rotate
        Quaternion myRotate = _holdRotation.rotation;
        if (_lockinHand.IsRightHand && _isReverseHoldOnRight)
        {
            //�E��͔��]
            myRotate *= Quaternion.Euler(0, 0, 180);
        }
        return myRotate;
    }

    protected void Vibrate()
    {
        ControllerVibrator.Vibrate(VIBE_SPEED, VIBE_POWER, VIBE_TIME, _lockinHand?.ControllerType ?? OVRInput.Controller.None);
    }

    protected virtual bool GetHoldInInput(SwitchCtrlHand from, HoldTypes hold)
    {
        switch (hold)
        {
            case HoldTypes.Pinch:
                return OculusGameInput.GetPinchIn(from.ControllerType);
            case HoldTypes.Grab:
                return OculusGameInput.GetGrabIn(from.ControllerType);
            case HoldTypes.Thumb:
                return OculusGameInput.GetThumbIn(from.ControllerType);
            default:
                Debug.LogError("���͂����܂���");
                return false;
        }
    }

    protected virtual bool GetFreeInput(SwitchCtrlHand from, HoldTypes hold)
    {
        switch (hold)
        {
            case HoldTypes.Pinch:
                return OculusGameInput.GetPinchOut(from.ControllerType);
            case HoldTypes.Grab:
                return OculusGameInput.GetGrabOut(from.ControllerType);
            case HoldTypes.Thumb:
                return OculusGameInput.GetThumbOut(from.ControllerType);
            default:
                Debug.LogError("���͂����܂���");
                return false;
        }
    }
}


