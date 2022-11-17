using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static System.Collections.Specialized.BitVector32;

namespace MyGame
{
    public class PlayerInput : MonoBehaviour
    {
        private static PlayerInput instance;
        private static bool isInstanced = false;

        private Vector2 _moveDir = default;
        private Vector2 _cameraDir = default;
        private InputMode _mode = default;
        private InputControls _controls = default;
        /// <summary> ゲーム内入力直後 </summary>
        private Dictionary<InputType, Action> _onEnterInputDicInGame = new Dictionary<InputType, Action>();
        /// <summary> ゲーム内入力解除 </summary>
        private Dictionary<InputType, Action> _onExitInputDicInGame = new Dictionary<InputType, Action>();
        /// <summary> 入力直後 </summary>
        private Dictionary<InputType, Action> _onEnterInputDic = new Dictionary<InputType, Action>();
        /// <summary> 入力解除 </summary>
        private Dictionary<InputType, Action> _onExitInputDic = new Dictionary<InputType, Action>();
        /// <summary> 入力中フラグ </summary>
        private Dictionary<InputType, bool> _isStayInputDic = new Dictionary<InputType, bool>();
        /// <summary>
        /// 移動入力方向
        /// </summary>
        public static Vector2 MoveDir
        {
            get
            {
                if (isInstanced == false)
                {
                    Initialize();
                }
                return instance._moveDir;
            }
        }
        /// <summary>
        /// カメラ入力方向
        /// </summary>
        public static Vector2 CameraDir
        {
            get
            {
                if (isInstanced == false)
                {
                    Initialize();
                }
                return instance._cameraDir;
            }
        }
        /// <summary>
        /// 現在の入力モード
        /// </summary>
        public static InputMode CurrentInputMode
        {
            get
            {
                if (isInstanced == false)
                {
                    Initialize();
                }
                return instance._mode;
            }
        }
        public static PlayerInput Instance
        {
            get
            {
                if (isInstanced == false)
                {
                    Initialize();
                }
                return instance;
            }
        }
        /// <summary>
        /// 初期化を行う
        /// </summary>
        private static void Initialize()
        {
            var obj = new GameObject("PlayerInput");
            instance = obj.AddComponent<PlayerInput>();
            instance._controls = new InputControls();
            instance._controls.Enable();
            instance.InitializeInput();
            instance._controls.InputMap.Move.performed += context => { instance._moveDir = context.ReadValue<Vector2>(); };
            instance._controls.InputMap.Move.canceled += context => { instance._moveDir = Vector2.zero; };
            instance._controls.InputMap.Camera.performed += context => { instance._cameraDir = context.ReadValue<Vector2>(); };
            instance._controls.InputMap.Camera.canceled += context => { instance._cameraDir = Vector2.zero; };
            instance._controls.InputMap.Jump.started += context => { ExecuteInput(InputType.Jump, ExecuteType.Enter); };
            instance._controls.InputMap.Jump.canceled += context => { ExecuteInput(InputType.Jump, ExecuteType.Exit); };
            instance._controls.InputMap.ChangeMode.started += context => { ExecuteInput(InputType.ChangeMode, ExecuteType.Enter); };
            instance._controls.InputMap.Attack1.started += context => { ExecuteInput(InputType.Fire1, ExecuteType.Enter); };
            instance._controls.InputMap.Attack1.canceled += context => { ExecuteInput(InputType.Fire1, ExecuteType.Exit); };
            instance._controls.InputMap.Attack2.started += context => { ExecuteInput(InputType.Fire2, ExecuteType.Enter); };
            instance._controls.InputMap.Attack2.canceled += context => { ExecuteInput(InputType.Fire2, ExecuteType.Exit); };
            instance._controls.InputMap.Attack3.started += context => { ExecuteInput(InputType.Fire3, ExecuteType.Enter); };
            instance._controls.InputMap.Attack3.canceled += context => { ExecuteInput(InputType.Fire3, ExecuteType.Exit); };
            instance._controls.InputMap.JetBoost.started += context => { ExecuteInput(InputType.Booster, ExecuteType.Enter); };
            instance._controls.InputMap.JetBoost.canceled += context => { ExecuteInput(InputType.Booster, ExecuteType.Exit); };
            instance._controls.InputMap.ChangeTarget.started += context => { ExecuteInput(InputType.ChangeTarget, ExecuteType.Enter); };
            instance._controls.InputMap.Submit.started += context => { ExecuteInput(InputType.Submit, ExecuteType.Enter); };
            instance._controls.InputMap.Submit.canceled += context => { ExecuteInput(InputType.Submit, ExecuteType.Exit); };
            isInstanced = true;
        }
        /// <summary>
        /// 入力処理の初期化を行う
        /// </summary>
        public void InitializeInput()
        {
            if (isInstanced == true)
            {
                for (int i = 0; i < Enum.GetValues(typeof(InputType)).Length; i++)
                {
                    _onEnterInputDicInGame[(InputType)i] = null;
                    _onExitInputDicInGame[(InputType)i] = null;
                    _onEnterInputDic[(InputType)i] = null;
                    _onExitInputDic[(InputType)i] = null;
                    _isStayInputDic[(InputType)i] = false;
                }
                return;
            }
            for (int i = 0; i < Enum.GetValues(typeof(InputType)).Length; i++)
            {
                _onEnterInputDicInGame.Add((InputType)i, null);
                _onExitInputDicInGame.Add((InputType)i, null);
                _onEnterInputDic.Add((InputType)i, null);
                _onExitInputDic.Add((InputType)i, null);
                _isStayInputDic.Add((InputType)i, false);
            }
        }
        /// <summary>
        /// 指定入力の指定実行タイプ処理を行う
        /// </summary>
        /// <param name="input"></param>
        /// <param name="type"></param>
        private static void ExecuteInput(InputType input, ExecuteType type)
        {
            if (isInstanced == false)
            {
                Initialize();
            }            
            switch (type)
            {
                case ExecuteType.Enter:
                    ExecuteEnterInput(input);
                    break;
                case ExecuteType.Exit:
                    ExecuteExitInput(input);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// モードに合わせて入力開始処理を実行する
        /// </summary>
        /// <param name="input"></param>
        private static void ExecuteEnterInput(InputType input)
        {
            if (instance._isStayInputDic[input] == true)
            {
                return;
            }
            switch (instance._mode)
            {
                case InputMode.InGame:
                    instance._onEnterInputDicInGame[input]?.Invoke();
                    break;
                case InputMode.Menu:
                    Instance._onEnterInputDic[input]?.Invoke();
                    break;
                default:
                    break;
            }
            instance._isStayInputDic[input] = true;
        }
        /// <summary>
        /// モードに合わせて入力解除処理を実行する
        /// </summary>
        /// <param name="input"></param>
        private static void ExecuteExitInput(InputType input)
        {
            if (instance._isStayInputDic[input] == false)
            {
                return;
            }
            switch (instance._mode)
            {
                case InputMode.InGame:
                    instance._onExitInputDicInGame[input]?.Invoke();
                    break;
                case InputMode.Menu:
                    instance._onExitInputDic[input]?.Invoke();
                    break;
                default:
                    break;
            }
            instance._isStayInputDic[input] = false;
        }
        /// <summary>
        /// 指定操作モードへ切替
        /// </summary>
        /// <param name="mode"></param>
        public static void ChangeInputMode(InputMode mode)
        {
            instance._mode = mode;
        }
        /// <summary>
        /// 指定入力の入力中フラグを返す
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool GetStayInput(InputType type)
        {
            if (isInstanced == false)
            {
                Initialize();
            }
            return instance._isStayInputDic[type];
        }
        /// <summary>
        /// 指定モードでの特定入力で行うActionを登録する
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="type"></param>
        /// <param name="action"></param>
        public static void SetEnterInput(InputMode mode, InputType type, Action action)
        {
            if (isInstanced == false)
            {
                Initialize();
            }
            switch (mode)
            {
                case InputMode.InGame:
                    instance._onEnterInputDicInGame[type] += action;
                    break;
                case InputMode.Menu:
                    instance._onEnterInputDic[type] += action;
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 指定モードでの特定入力で行うActionを登録解除する
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="type"></param>
        /// <param name="action"></param>
        public static void LiftEnterInput(InputMode mode, InputType type, Action action)
        {
            if (isInstanced == false)
            {
                Initialize();
            }
            switch (mode)
            {
                case InputMode.InGame:
                    instance._onEnterInputDicInGame[type] -= action;
                    break;
                case InputMode.Menu:
                    instance._onEnterInputDic[type] -= action;
                    break;
                default:
                    break;
            }
        }
    }
    /// <summary>
    /// ゲーム中のモード
    /// </summary>
    public enum InputMode
    {
        /// <summary> ゲーム中操作モード </summary>
        InGame,
        /// <summary> メニュー等ＵＩ操作モード </summary>
        Menu,
    }
    /// <summary>
    /// 実行タイプ
    /// </summary>
    public enum ExecuteType
    {
        /// <summary> 入力時 </summary>
        Enter,
        /// <summary> 入力終了時 </summary>
        Exit,
    }
    /// <summary>
    /// 入力種類
    /// </summary>
    public enum InputType
    {
        /// <summary> 決定入力 </summary>
        Submit,
        /// <summary> キャンセル入力 </summary>
        Cancel,
        /// <summary> ジャンプ入力 </summary>
        Jump,
        /// <summary> モードチェンジ入力 </summary>
        ChangeMode,
        /// <summary> ターゲットチェンジ入力 </summary>
        ChangeTarget,
        /// <summary> 攻撃入力１ </summary>
        Fire1,
        /// <summary> 攻撃入力２ </summary>
        Fire2,
        /// <summary> 攻撃入力３ </summary>
        Fire3,
        /// <summary> ブースター入力 </summary>
        Booster,
    }
}