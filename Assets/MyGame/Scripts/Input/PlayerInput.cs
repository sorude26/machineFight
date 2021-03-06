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
                    Initialization();
                }
                return instance._moveDir;
            }
        }
        public static PlayerInput Instance
        {
            get 
            {
                if (instance is null)
                {
                    Initialization();
                }
                return instance; 
            }
        }
        private static void Initialization()
        {
            var obj = new GameObject("PlayerInput");
            instance = obj.AddComponent<PlayerInput>();
            instance._controls = new InputControls();
            instance._controls.Enable();
            instance.InitializationInput();
            instance._controls.InputMap.Move.performed += context => { instance._moveDir = context.ReadValue<Vector2>(); };
            instance._controls.InputMap.Move.canceled += context => { instance._moveDir = Vector2.zero; }; 
            instance._controls.InputMap.Jump.started += context => { instance._onEnterInputDic[InputType.Jump]?.Invoke(); };
            instance._controls.InputMap.Jump.canceled += context => { instance._onExitInputDic[InputType.Jump]?.Invoke(); };
            instance._controls.InputMap.ChangeMode.started += context => { instance._onEnterInputDic[InputType.ChangeMode]?.Invoke(); };
        }
        private void InitializationInput()
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
        /// <summary> 攻撃入力１ </summary>
        Fire1,
        /// <summary> 攻撃入力２ </summary>
        Fire2,
        /// <summary> 攻撃入力３ </summary>
        Fire3,
    }
}