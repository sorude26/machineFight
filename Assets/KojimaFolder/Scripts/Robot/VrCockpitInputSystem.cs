//参考 https://gentome.com/gentomeblog/3972/customnewinputsystem/
//https://github.com/Unity-Technologies/InputSystem/blob/develop/Packages/com.unity.inputsystem/Documentation~/Devices.md#step-3-the-update-method

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UIElements;

/// <summary>
/// VRコックピットのエミュレートされた入力をUnityのInputSystemに登録するクラス
/// </summary>
#if UNITY_EDITOR
[InitializeOnLoad]
#endif
[InputControlLayout(displayName = "VrCockpit", stateType = typeof(CockpitInputState))]
public class VrCockpitInputSystem : InputDevice, IInputUpdateCallbackReceiver
{
    public const string ATTACK1         = "attack1";
    public const int    ATTACK1_BIT     = 0;

    public const string ATTACK2         = "attack2";
    public const int    ATTACK2_BIT     = 1;

    public const string ATTACK3         = "attack3";
    public const int    ATTACK3_BIT     = 2;

    public const string ATTACK4         = "attack4";
    public const int    ATTACK4_BIT     = 3;

    public const string JUMP            = "jump";
    public const int    JUMP_BIT        = 4;

    public const string JETBOOST        = "jetBoost";
    public const int    JETBOOST_BIT    = 5;

    public const string CHANGETARGET    = "changeTarget";
    public const int    CHANGETARGET_BIT= 6;

    public const string MOVEAXIS = "moveAxis";
    public const string CAMERAAXIS = "cameraAxis";

    public ButtonControl attack1 { get; private set; }
    public ButtonControl attack2 { get; private set; }
    public ButtonControl attack3 { get; private set; }
    public ButtonControl attack4 { get; private set; }
    public ButtonControl jump { get; private set; }
    public ButtonControl jetBoost { get; private set; }
    public ButtonControl changeTarget { get; private set; }

    public Vector2Control moveAxis { get; private set; }
    public Vector2Control cameraAxis { get; private set; }

    protected override void FinishSetup()
    {
        base.FinishSetup();
        attack1 = GetChildControl<ButtonControl>(ATTACK1);
        attack2 = GetChildControl<ButtonControl>(ATTACK2);
        attack3 = GetChildControl<ButtonControl>(ATTACK3);
        attack4 = GetChildControl<ButtonControl>(ATTACK4);
        jump = GetChildControl<ButtonControl>(JUMP);
        jetBoost = GetChildControl<ButtonControl>(JETBOOST);
        changeTarget = GetChildControl<ButtonControl>(CHANGETARGET);
        moveAxis = GetChildControl<Vector2Control>(MOVEAXIS);
        cameraAxis = GetChildControl<Vector2Control>(CAMERAAXIS);
    }


    static VrCockpitInputSystem()
    {
        InputSystem.RegisterLayout<VrCockpitInputSystem>("VrCockpit");
        InputSystem.AddDevice<VrCockpitInputSystem>();
    }

    

    public void OnUpdate()
    {
        
        var state = new CockpitInputState();
        if (PlayerVrCockpit.Instance == null)
        {
            //コックピットがない場合は入力値なしでreturn
            InputSystem.QueueStateEvent(this, state);
            return;
        }
        //ビット演算関数
        void SetButtonBit(bool isButton, int bit)
        {
            if (isButton)
            {
                state.buttons |= (ushort)(1u << bit);
            }
        }
        SetButtonBit(PlayerVrCockpit.Attack1(), ATTACK1_BIT);
        SetButtonBit(PlayerVrCockpit.Attack2(), ATTACK2_BIT);
        SetButtonBit(PlayerVrCockpit.Attack3(), ATTACK3_BIT);
        SetButtonBit(PlayerVrCockpit.Attack4(), ATTACK4_BIT);
        SetButtonBit(PlayerVrCockpit.Jump(), JUMP_BIT);
        SetButtonBit(PlayerVrCockpit.JetBoost(), JETBOOST_BIT);
        SetButtonBit(PlayerVrCockpit.ChangeTarget(), CHANGETARGET_BIT);
        state.MoveAxis = PlayerVrCockpit.Move();
        state.CameraAxis = PlayerVrCockpit.Camera();
        InputSystem.QueueStateEvent(this, state);
    }

   

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InitializeInPlayer()
    {
        //staticコンストラクタを呼び出すための空メソッド
    }
}

public struct CockpitInputState : IInputStateTypeInfo
{
    public FourCC format => new FourCC('V', 'R', 'C', 'K');


    [InputControl(name = VrCockpitInputSystem.ATTACK1       , layout = "Button", bit = VrCockpitInputSystem.ATTACK1_BIT)]
    [InputControl(name = VrCockpitInputSystem.ATTACK2       , layout = "Button", bit = VrCockpitInputSystem.ATTACK2_BIT)]
    [InputControl(name = VrCockpitInputSystem.ATTACK3       , layout = "Button", bit = VrCockpitInputSystem.ATTACK3_BIT)]
    [InputControl(name = VrCockpitInputSystem.ATTACK4       , layout = "Button", bit = VrCockpitInputSystem.ATTACK4_BIT)]
    [InputControl(name = VrCockpitInputSystem.JUMP          , layout = "Button", bit = VrCockpitInputSystem.JUMP_BIT)]
    [InputControl(name = VrCockpitInputSystem.JETBOOST      , layout = "Button", bit = VrCockpitInputSystem.JETBOOST_BIT)]
    [InputControl(name = VrCockpitInputSystem.CHANGETARGET  , layout = "Button", bit = VrCockpitInputSystem.CHANGETARGET_BIT)]
    public ushort buttons;//16bit

    [InputControl(name = VrCockpitInputSystem.MOVEAXIS, layout = "Vector2")]
    public Vector2 MoveAxis;

    [InputControl(name = VrCockpitInputSystem.CAMERAAXIS, layout = "Vector2")]
    public Vector2 CameraAxis;
}
