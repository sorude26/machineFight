using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MyGame
{
    public class PlayerInput : MonoBehaviour
    {
        private static PlayerInput instance;

        private Vector2 _moveDir = default;
        private Vector2 _cameraDir = default;
        private InputControls _controls = default;
        /// <summary> 入力直後 </summary>
        private Dictionary<InputType, Action> _onEnterInputDic = new Dictionary<InputType, Action>();
        /// <summary> 入力中 </summary>
        private Dictionary<InputType, Action> _onStayInputDic = new Dictionary<InputType, Action>();
        /// <summary> 入力解除 </summary>
        private Dictionary<InputType, Action> _onExitInputDic = new Dictionary<InputType, Action>();
        public static Vector2 MoveDir 
        {
            get 
            {
                if (instance is null)
                {
                    Initialize();
                }
                return instance._moveDir;
            }
        }
        public static Vector2 CameraDir
        {
            get
            {
                if (instance is null)
                {
                    Initialize();
                }
                return instance._cameraDir;
            }
        }
        public static PlayerInput Instance
        {
            get 
            {
                if (instance is null)
                {
                    Initialize();
                }
                return instance; 
            }
        }
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
            instance._controls.InputMap.Jump.started += context => { instance._onEnterInputDic[InputType.Jump]?.Invoke(); };
            instance._controls.InputMap.Jump.canceled += context => { instance._onExitInputDic[InputType.Jump]?.Invoke(); };
            instance._controls.InputMap.ChangeMode.started += context => { instance._onEnterInputDic[InputType.ChangeMode]?.Invoke(); };
            instance._controls.InputMap.Attack1.started += context => { instance._onEnterInputDic[InputType.Fire1]?.Invoke(); };
            instance._controls.InputMap.Attack2.started += context => { instance._onEnterInputDic[InputType.Fire2]?.Invoke(); };
            instance._controls.InputMap.JetBoost.started += context => { instance._onEnterInputDic[InputType.Booster]?.Invoke(); };
            instance._controls.InputMap.ChangeTarget.started +=context => { instance._onEnterInputDic[InputType.ChangeTarget]?.Invoke(); };
        }
        private void InitializeInput()
        {
            for (int i = 0; i < Enum.GetValues(typeof(InputType)).Length; i++)
            {
                _onEnterInputDic.Add((InputType)i, null);
                _onStayInputDic.Add((InputType)i, null);
                _onExitInputDic.Add((InputType)i, null);
            }
        }
        public static void SetEnterInput(InputType type, Action action)
        {
            Instance._onEnterInputDic[type] += action;
        }
        public static void LiftEnterInput(InputType type, Action action)
        {
            instance._onEnterInputDic[type] -= action;
        }
    }
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