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
    public static string ATTACK1 = "attack1";
    public static string ATTACK2 = "attack2";
    public static string ATTACK3 = "attack3";
    public static string ATTACK4 = "attack4";
    public static string JUMP = "jump";
    public static string JETBOOST = "jetBoost";
    public static string CHANGETARGET = "changeTarget";
    public static string MOVEAXIS = "moveAxis";
    public static string CAMERAAXIS = "cameraAxis";

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
    //メモリ上での識別を高速化するためのものらしい、任意の四文字を指定可能
    public FourCC format => new FourCC('V', 'R', 'C', 'K');


    [InputControl(name = "attack1"      , layout = "Button", bit = 0)]
    [InputControl(name = "attack2"      , layout = "Button", bit = 1)]
    [InputControl(name = "attack3"      , layout = "Button", bit = 2)]
    [InputControl(name = "attack4"      , layout = "Button", bit = 3)]
    [InputControl(name = "jump"         , layout = "Button", bit = 4)]
    [InputControl(name = "jetBoost"     , layout = "Button", bit = 5)]
    [InputControl(name = "changeTarget" , layout = "Button", bit = 6)]
    public ushort buttons;//16bit

    //軸
    [InputControl(name = "moveAxis", layout = "Vector2")]
    [InputControl(name = "moveAxis/x", defaultState = 0.0f, format = "FLT", parameters = "normalize,normalizeMin=-1,normalizeMax=1,normalizeZero=0.0,clamp=2,clampMin=-1,clampMax=1")]
    [InputControl(name = "moveAxis/y", defaultState = 0.0f, format = "FLT", parameters = "normalize,normalizeMin=-1,normalizeMax=1,normalizeZero=0.0,clamp=2,clampMin=-1,clampMax=1")]
    public Vector2 MoveAxis;

    [InputControl(name = "cameraAxis", layout = "Vector2")]
    [InputControl(name = "cameraAxis/x", defaultState = 0.0f, format = "FLT", parameters = "normalize,normalizeMin=-1,normalizeMax=1,normalizeZero=0.0,clamp=2,clampMin=-1,clampMax=1")]
    [InputControl(name = "cameraAxis/y", defaultState = 0.0f, format = "FLT", parameters = "normalize,normalizeMin=-1,normalizeMax=1,normalizeZero=0.0,clamp=2,clampMin=-1,clampMax=1")]
    public Vector2 CameraAxis;
}
