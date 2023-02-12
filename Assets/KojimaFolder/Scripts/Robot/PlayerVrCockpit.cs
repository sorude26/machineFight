using MyGame;
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
    /// 右手
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
    /// 左手
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
    /// バックウェポンorホバー
    /// </summary>
    /// <returns></returns>
    public static bool Attack3()
    {
        if (Instance._machine?.MachineController.BodyController.BackPack.BackPackWeapon == null)
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
            //再帰処理ですべての子オブジェクトに適用
            SetLayerToChildlen(layer, item);
        }
    }

    IEnumerator CameraSetup()
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
            yield return null;
            //一人称切り替え
            FollowCamera.ChangeToVrCamera();
            //メインカメラをモニターに映るよう設定
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
            //スロットルが上がった場合はホバーモードに移行
            _machine?.MachineController.TryFloat();
            //音声再生
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
            //スロットルが下がった場合は地上モードに移行
            _machine?.MachineController.TryGround();
            _hoverSoundPlayer.AudioSource.volume = 0;
        }
    }

    private void ThrottleValueChanged(int zone)
    {
        if (zone == ZONE_HOVER)
        {
            //ホバー時の速度を登録
            _machine?.MachineController.BodyController.SetFloatSpeed(ZONE_HOVER);
        }
    }

    private void OnSubmit()
    {
        //サウンド再生
        SoundManager.Instance.PlaySE(SE_SUBMIT_ID, SE_SUBMIT_VOLUME);
    }

    private void OnCancel()
    {
        //サウンド再生
        SoundManager.Instance.PlaySE(SE_CANCEL_ID, SE_CANCEL_VOLUME);
    }

    private void OnButton()
    {
        if (!OVRManager.isHmdPresent && !_isDebagVR)
        {
            return;
        }
        //サウンド再生
        SoundManager.Instance.PlaySE(SE_BUTTON_ID, SE_BUTTON_VOLUME);
    }
}
