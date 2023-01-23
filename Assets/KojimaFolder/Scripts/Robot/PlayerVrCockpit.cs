using Oculus.Interaction.Input;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

/// <summary>
/// コックピット内部からの入力を処理するクラス
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
    /// 右手
    /// </summary>
    /// <returns></returns>
    public static bool Attack1()
    {
        if (!Instance._weaponRightToggleSwitch.IsOn) return false;
        return Instance._flightStick.GetTriggerInput(false);
    }
    /// <summary>
    /// 左手
    /// </summary>
    /// <returns></returns>
    public static bool Attack2()
    {
        if (!Instance._weaponLeftToggleSwitch.IsOn) return false;
        return Instance._flightStick.GetTriggerInput(false);
    }
    /// <summary>
    /// バックウェポンorホバー
    /// </summary>
    /// <returns></returns>
    public static bool Attack3()
    {
        if (Instance._machine.MachineController.BodyController.BackPack.BackPackWeapon == null)
        {
            //バックウェポンを装備していない場合は入力を渡さない
            return false;
        }
        if (!Instance._weaponBackToggleSwitch.IsOn) return false;
        return Instance._flightStick.GetTriggerInput(false);
    }
    /// <summary>
    /// キック
    /// </summary>
    /// <returns></returns>
    public static bool Attack4()
    {
        //VRモードではキックしない
        return false;
    }
    /// <summary>
    /// ジャンプ
    /// </summary>
    /// <returns></returns>
    public static bool Jump()
    {
        return Instance._throttleLever.GetTriggerInput(false);
    }
    /// <summary>
    /// ステップ
    /// </summary>
    /// <returns></returns>
    public static bool JetBoost()
    {
        return Instance._throttleLever.GetUpperButtonInput(false);
    }
    /// <summary>
    /// ターゲット切り替え
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
            //再帰処理ですべての子オブジェクトに適用
            SetLayerToChildlen(layer, item);
        }
    }

    private void CameraSetup()
    {
        //VR機器が接続されていない場合はデスクトップ用のカメラに切り替え、コックピットを非表示にする。
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
            //一人称切り替え
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
            //スロットルが上がった場合はホバーモードに移行
            _machine.MachineController.TryFloat();
            
        }
    }

    private void ExitZoneThrottle(int zone)
    {
        if (zone == ZONE_HOVER)
        {
            //スロットルが下がった場合は地上モードに移行
            _machine.MachineController.TryGround();
        }
    }

    private void ThrottleValueChanged(int zone)
    {
        if (zone == ZONE_HOVER)
        {
            //ホバー時の速度を登録
            _machine.MachineController.BodyController.SetFloatSpeed(ZONE_HOVER);
        }
    }
}
