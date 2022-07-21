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
        protected Vector2 _moveDir = default;
        protected StateIdle _stateIdle = new StateIdle();
        protected StateWalk _stateWalk = new StateWalk();
        protected StateFall _stateFall = new StateFall();
        protected StateJump _stateJump = new StateJump();
        protected StateFloat _stateFloat = new StateFloat();
        protected StateLanding _stateLanding = new StateLanding();
        public StateController(MoveController moveController, AnimatorController animatorController, WallChecker checker)
        {
            _moveController = moveController;
            _animatorController = animatorController;
            _groundChecker = checker;
            _currentState = _stateIdle;
            _animatorController.OnJumpEnd += JumpEnd;
            _animatorController.OnLandingEnd += LandingEnd;
        }
        protected void ChangeAnimation(StateType type, float speed = AnimatorController.DEFAULT_CHSNGE_SPEED)
        {
            _animatorController.ChangeAnimation(type, speed);
        }
        protected void ChangeAnimation(string type, float speed = AnimatorController.DEFAULT_CHSNGE_SPEED)
        {
            _animatorController.ChangeAnimation(type, speed);
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
            _currentStateType = type;
            _currentState.OnEnter(this);
        }
        protected void SetState(StateType type)
        {
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
            if (!_groundChecker.IsWalled()) { return; }
            ChangeState(StateType.Landing);
        }
        protected void JumpEnd()
        {
            ChangeState(StateType.Fall);
        }
        protected void LandingEnd()
        {
            ChangeState(StateType.Idle);
        }
        public void Update()
        {
            _currentState.OnUpdate(this);
        }
        public void FixedUpdate(Vector2 dir)
        {
            _moveDir = dir;
            _currentState.OnFixedUpdate(this);
        }
        public void InputJump()
        {
            if (!_groundChecker.IsWalled()){ return; }
            ChangeState(StateType.Jump);
        }
    }
}