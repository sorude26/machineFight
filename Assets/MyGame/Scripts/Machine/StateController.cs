using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.MachineFrame
{
    public partial class StateController
    {
        protected WallChecker _groundChecker = default;
        protected MoveController _moveController = default;
        protected AnimatorController _animatorController = default;
        protected IMachineState _currentState = default;
        protected StateType _currentStateType = default;
        protected StateIdle _stateIdle = new StateIdle();
        protected StateWalk _stateWalk = new StateWalk();
        protected StateFall _stateFall = new StateFall();
        protected StateJump _stateJump = new StateJump();
        protected StateFloat _stateFloat = new StateFloat();
        protected StateLanding _stateLanding = new StateLanding();
        protected Dictionary<StateType, string> _stateAnimationNames = new Dictionary<StateType, string>();

        public event Func<bool> OnChackGround = default;
        public StateController(MoveController moveController, AnimatorController animatorController,WallChecker checker)
        {
            _moveController = moveController;
            _animatorController = animatorController;
            _groundChecker = checker;
            _currentState = _stateIdle;
        }
        protected void ChangeAnimation(StateType type)
        {
            if (!_stateAnimationNames.ContainsKey(type)) { return; }
            _animatorController.ChangeAnimation(_stateAnimationNames[type]);
        }
        protected void ChangeState(StateType type)
        {
            if (_currentStateType == type) { return; }
            switch (type)
            {
                case StateType.Idle:
                    _currentState = _stateIdle;
                    break;
                case StateType.Walk:
                    _currentState = _stateWalk;
                    break;
                case StateType.Jump:
                    _currentState = _stateJump;
                    break;
                case StateType.Fall:
                    _currentState = _stateFall;
                    break;
                case StateType.Landing:
                    _currentState = _stateLanding;
                    break;
                case StateType.Float:
                    _currentState = _stateFloat;
                    break;
                default:
                    break;
            }
            _currentState.OnEnter(this);
        }
        protected void SetState(StateType type)
        {
            _currentStateType = type;
            ChangeAnimation(type);
        }
        protected void ChackFallOnGround()
        {
            _moveController.MoveDecelerate();
            if (!_groundChecker.IsWalled())
            {
                ChangeState(StateType.Fall);
            }
        }
        protected void ChackGround()
        {
            if (_groundChecker.IsWalled())
            {
                ChangeState(StateType.Landing);
            }
        }
        public void Update()
        {
            _currentState.OnUpdate(this);
        }
        public void FixedUpdate()
        {
            _currentState.OnFixedUpdate(this);
        }
    }
}