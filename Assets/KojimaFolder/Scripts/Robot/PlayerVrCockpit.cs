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
    private static PlayerVrCockpit _instance;
    public static PlayerVrCockpit Instance => _instance;

    [SerializeField]
    PlayerMachineController _machine;
    [SerializeField]
    FlightStick _flightStick;
    [SerializeField]
    ThrottleLever _throttleLever;
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
    bool _isDebagVR = false;
    public bool IsDebagVR => _isDebagVR;

    public bool WeaponSwitch(int i)
    {
        switch (i)
        {
            case 1:
                return _weaponRightToggleSwitch.IsOn;
            case 2:
                return _weaponLeftToggleSwitch.IsOn;
            case 3:
                return _weaponBackToggleSwitch.IsOn;
            default:
                return false;
        }
    }

    /// <summary>
    /// �E��
    /// </summary>
    /// <returns></returns>
    public static bool Attack1()
    {
        if (!Instance._weaponRightToggleSwitch.IsOn) return false;
        return Instance._flightStick.GetTriggerInput(false);
    }
    /// <summary>
    /// ����
    /// </summary>
    /// <returns></returns>
    public static bool Attack2()
    {
        if (!Instance._weaponLeftToggleSwitch.IsOn) return false;
        return Instance._flightStick.GetTriggerInput(false);
    }
    /// <summary>
    /// �o�b�N�E�F�|��or�z�o�[
    /// </summary>
    /// <returns></returns>
    public static bool Attack3()
    {
        if (Instance._machine.MachineController.BodyController.BackPack.BackPackWeapon == null)
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

    public static Vector2 Move()
    {
        return Instance._flightStick.GetStickBodyInput();
    }

    public static Vector2 Camera()
    {
        return Instance._flightStick.GetThumbstickInput();
    }

    private void Awake()
    {
        _instance = this;
        var layer = this.gameObject.layer;
        SetLayerToChildlen(layer, this.transform);
        CameraSetup();
        ThrottleLeverSetUp();
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

    private void CameraSetup()
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
            //��l�̐؂�ւ�
            FollowCamera.ChangeToVrCamera();
        }
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
            _machine.MachineController.TryFloat();
            
        }
    }

    private void ExitZoneThrottle(int zone)
    {
        if (zone == ZONE_HOVER)
        {
            //�X���b�g�������������ꍇ�͒n�ヂ�[�h�Ɉڍs
            _machine.MachineController.TryGround();
        }
    }

    private void ThrottleValueChanged(int zone)
    {
        if (zone == ZONE_HOVER)
        {
            //�z�o�[���̑��x��o�^
            _machine.MachineController.BodyController.SetFloatSpeed(ZONE_HOVER);
        }
    }
}
