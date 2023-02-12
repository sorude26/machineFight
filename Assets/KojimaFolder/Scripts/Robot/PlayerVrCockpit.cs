using MyGame;
using Oculus.Interaction.Input;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

/// <summary>
/// �R�b�N�s�b�g��������̓��͂���������N���X
/// </summary>
public class PlayerVrCockpit : MonoBehaviour
{
    const int ZONE_HOVER = 1;
    const int SE_HOVER_ID = 57;
    const float SE_HOVER_VOLUME = 0.5f;
    const int SE_SUBMIT_ID = 25;
    const float SE_SUBMIT_VOLUME = 0.25f;
    const int SE_CANCEL_ID = 24;
    const float SE_CANCEL_VOLUME = 0.25f;
    const int SE_BUTTON_ID = 61;
    const float SE_BUTTON_VOLUME = 0.25f;

    const float DANGER_HP_RATIO = 0.25f;
    private static PlayerVrCockpit _instance;
    public static PlayerVrCockpit Instance => _instance;

    [SerializeField]
    PlayerMachineController _machine;
    [SerializeField]
    protected FlightStick _flightStick;
    [SerializeField]
    ThrottleLever _throttleLever;
    [SerializeField]
    Switch _systemStartSwitch;
    [SerializeField]
    Switch _weaponRightToggleSwitch;
    [SerializeField]
    Switch _weaponLeftToggleSwitch;
    [SerializeField]
    Switch _weaponBackToggleSwitch;
    [SerializeField, Range(0.01f, 0.99f)]
    float _groundToHoverThrottle;
    [SerializeField]
    LayerMask _dontChangeLayer;
    [SerializeField]
    Light _cockpitLight;
    [SerializeField]
    RenderTexture _mainTexture;

    SoundPlayer _hoverSoundPlayer;

    [SerializeField]
    bool _isDebagVR = false;
    public bool IsDebagVR => _isDebagVR;

    public Switch SystemStartSwitch => _systemStartSwitch;

    public Switch WeaponSwitch(int i)
    {
        switch (i)
        {
            case 0:
                return _weaponRightToggleSwitch;
            case 1:
                return _weaponLeftToggleSwitch;
            case 2:
                return _weaponBackToggleSwitch;
            default:
                return null;
        }
    }

    /// <summary>
    /// �E��
    /// </summary>
    /// <returns></returns>
    public static bool Attack1()
    {
        return Instance?.Attack1Virtual() ?? false;
    }
    protected virtual bool Attack1Virtual()
    {
        if (!_weaponRightToggleSwitch.IsOn) return false;
        return _flightStick.GetTriggerInput(false);
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <returns></returns>
    public static bool Attack2()
    {
        return Instance?.Attack2Virtual() ?? false;
    }
    protected virtual bool Attack2Virtual()
    {
        if (!_weaponLeftToggleSwitch.IsOn) return false;
        return _flightStick.GetTriggerInput(false);
    }

    /// <summary>
    /// �o�b�N�E�F�|��or�z�o�[
    /// </summary>
    /// <returns></returns>
    public static bool Attack3()
    {
        if (Instance._machine?.MachineController.BodyController.BackPack.BackPackWeapon == null)
        {
            //�o�b�N�E�F�|���𑕔����Ă��Ȃ��ꍇ�͓��͂�n���Ȃ�
            return false;
        }
        if (!Instance._weaponBackToggleSwitch.IsOn) return false;
        return Instance._flightStick.GetTriggerInput(false);
    }
    /// <summary>
    /// �L�b�N
    /// </summary>
    /// <returns></returns>
    public static bool Attack4()
    {
        //VR���[�h�ł̓L�b�N���Ȃ�
        return false;
    }
    /// <summary>
    /// �W�����v
    /// </summary>
    /// <returns></returns>
    public static bool Jump()
    {
        return Instance._throttleLever.GetTriggerInput(false);
    }
    /// <summary>
    /// �X�e�b�v
    /// </summary>
    /// <returns></returns>
    public static bool JetBoost()
    {
        return Instance._throttleLever.GetUpperButtonInput(false);
    }
    /// <summary>
    /// �^�[�Q�b�g�؂�ւ�
    /// </summary>
    /// <returns></returns>
    public static bool ChangeTarget()
    {
        return Instance._flightStick.GetThumbstickButtonInput(false);
    }

    public static bool Submit()
    {
        return Instance._flightStick.GetTriggerInput(false);
    }

    public static bool Cancel()
    {
        return Instance._flightStick.GetUpperButtonInput(false) || Instance._throttleLever.GetLowerButtonInput(false);
    }

    public static Vector2 Move()
    {
        return Instance.MoveVirtual();
    }
    protected virtual Vector2 MoveVirtual()
    {
        return Instance._flightStick.GetStickBodyInput();
    }

    public static Vector2 Camera()
    {
        return Instance.CameraVirtual();
    }

    protected virtual Vector2 CameraVirtual()
    {
        return Instance._flightStick.GetThumbstickInput();
    }

    public void HpUpdate(float max, float current)
    {
        if ((current / max) < DANGER_HP_RATIO)
        {
            _cockpitLight.color = Color.red;
        }
    }

    private void Awake()
    {
        _instance = this;
        var layer = this.gameObject.layer;
        SetLayerToChildlen(layer, this.transform);
        StartCoroutine(CameraSetup());
        ThrottleLeverSetUp();
        PlayerInput.SetEnterInput(InputMode.InGame, InputType.Submit, OnButton);
        PlayerInput.SetEnterInput(InputMode.InGame, InputType.Jump, OnButton);
        PlayerInput.SetEnterInput(InputMode.InGame, InputType.Booster, OnButton);

        PlayerInput.SetEnterInput(InputMode.Menu, InputType.Submit, OnSubmit);
        PlayerInput.SetEnterInput(InputMode.Menu, InputType.Cancel, OnCancel);
    }

    private void OnDestroy()
    {
        PlayerInput.LiftEnterInput(InputMode.InGame, InputType.Submit, OnButton);
        PlayerInput.LiftEnterInput(InputMode.InGame, InputType.Jump, OnButton);
        PlayerInput.LiftEnterInput(InputMode.InGame, InputType.Booster, OnButton);

        PlayerInput.LiftEnterInput(InputMode.Menu, InputType.Submit, OnSubmit);
        PlayerInput.LiftEnterInput(InputMode.Menu, InputType.Cancel, OnCancel);
    }

    private void SetLayerToChildlen(int layer, Transform t)
    {
        foreach (Transform item in t)
        {
            if (((1 << item.gameObject.layer) & _dontChangeLayer.value) == 0)
            {
                item.gameObject.layer = layer;
            }
            //�ċA�����ł��ׂĂ̎q�I�u�W�F�N�g�ɓK�p
            SetLayerToChildlen(layer, item);
        }
    }

    IEnumerator CameraSetup()
    {
        //VR�@�킪�ڑ�����Ă��Ȃ��ꍇ�̓f�X�N�g�b�v�p�̃J�����ɐ؂�ւ��A�R�b�N�s�b�g���\���ɂ���B
        if (!OVRManager.isHmdPresent && !_isDebagVR)
        {
            var cameras = FindObjectsOfType<Camera>(true);
            foreach (var item in cameras)
            {
                item.targetTexture = null;
            }
            this.gameObject.SetActive(false);
        }
        else
        {
            yield return null;
            //��l�̐؂�ւ�
            FollowCamera.ChangeToVrCamera();
            //���C���J���������j�^�[�ɉf��悤�ݒ�
            MainCameraLocator.MainCamera.targetTexture = _mainTexture;
        }
        yield return null;
    }

    private void ThrottleLeverSetUp()
    {
        _throttleLever.SetClickPointsAsNew(new float[] { _groundToHoverThrottle });
        _throttleLever.OnEnterZone += (a) => EnterZoneThrottle(a);
        _throttleLever.OnExitZone += (a) => ExitZoneThrottle(a);
        _throttleLever.OnValueChanged += (a) => ThrottleValueChanged(a);
    }

    private void EnterZoneThrottle(int zone)
    {
        if (zone == ZONE_HOVER)
        {
            //�X���b�g�����オ�����ꍇ�̓z�o�[���[�h�Ɉڍs
            _machine?.MachineController.TryFloat();
            //�����Đ�
            if (MannedOperationSystem.Instance.IsOnline)
            {
                _hoverSoundPlayer ??= SoundManager.Instance.PlaySELoop(SE_HOVER_ID, this.gameObject, SE_HOVER_VOLUME);
                _hoverSoundPlayer.AudioSource.volume = SE_HOVER_VOLUME;
            }
        }
    }

    private void ExitZoneThrottle(int zone)
    {
        if (zone == ZONE_HOVER)
        {
            //�X���b�g�������������ꍇ�͒n�ヂ�[�h�Ɉڍs
            _machine?.MachineController.TryGround();
            _hoverSoundPlayer.AudioSource.volume = 0;
        }
    }

    private void ThrottleValueChanged(int zone)
    {
        if (zone == ZONE_HOVER)
        {
            //�z�o�[���̑��x��o�^
            _machine?.MachineController.BodyController.SetFloatSpeed(ZONE_HOVER);
        }
    }

    private void OnSubmit()
    {
        //�T�E���h�Đ�
        SoundManager.Instance.PlaySE(SE_SUBMIT_ID, SE_SUBMIT_VOLUME);
    }

    private void OnCancel()
    {
        //�T�E���h�Đ�
        SoundManager.Instance.PlaySE(SE_CANCEL_ID, SE_CANCEL_VOLUME);
    }

    private void OnButton()
    {
        if (!OVRManager.isHmdPresent && !_isDebagVR)
        {
            return;
        }
        //�T�E���h�Đ�
        SoundManager.Instance.PlaySE(SE_BUTTON_ID, SE_BUTTON_VOLUME);
    }
}
