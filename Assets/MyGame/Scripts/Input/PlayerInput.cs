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
        /// <summary> “ü—Í’¼Œã </summary>
        private Dictionary<InputType, Action> _onEnterInputDic = new Dictionary<InputType, Action>();
        /// <summary> “ü—Í’† </summary>
        private Dictionary<InputType, Action> _onStayInputDic = new Dictionary<InputType, Action>();
        /// <summary> “ü—Í‰ğœ </summary>
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
        public static Vector2 CameraDir
        {
            get
            {
                if (instance is null)
                {
                    Initialization();
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
            instance._controls.InputMap.Camera.performed += context => { instance._cameraDir = context.ReadValue<Vector2>(); };
            instance._controls.InputMap.Camera.canceled += context => { instance._cameraDir = Vector2.zero; };
            instance._controls.InputMap.Jump.started += context => { instance._onEnterInputDic[InputType.Jump]?.Invoke(); };
            instance._controls.InputMap.Jump.canceled += context => { instance._onExitInputDic[InputType.Jump]?.Invoke(); };
            instance._controls.InputMap.ChangeMode.started += context => { instance._onEnterInputDic[InputType.ChangeMode]?.Invoke(); };
            instance._controls.InputMap.Attack1.started += context => { instance._onEnterInputDic[InputType.Fire1]?.Invoke(); };
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
        /// <summary> Œˆ’è“ü—Í </summary>
        Submit,
        /// <summary> ƒLƒƒƒ“ƒZƒ‹“ü—Í </summary>
        Cancel,
        /// <summary> ƒWƒƒƒ“ƒv“ü—Í </summary>
        Jump,
        /// <summary> ƒ‚[ƒhƒ`ƒFƒ“ƒW“ü—Í </summary>
        ChangeMode,
        /// <summary> UŒ‚“ü—Í‚P </summary>
        Fire1,
        /// <summary> UŒ‚“ü—Í‚Q </summary>
        Fire2,
        /// <summary> UŒ‚“ü—Í‚R </summary>
        Fire3,
    }
}