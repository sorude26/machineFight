using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.MachineFrame
{
    public partial class StateController
    {
        protected class StateWalk : IMachineState
        {
            private enum WalkAngle
            {
                Front,
                Back,
                Left,
                Right,
            }
            private WalkAngle _currentAngle = WalkAngle.Front;
            private string[] _stateAnimations = { "Walk", "WalkBack","WalkTurnLeft","WalkTurnRight" };
            public void OnEnter(StateController control)
            {
                control.SetState(StateType.Walk);
                if (control._moveDir.y > 0)
                {
                    _currentAngle = WalkAngle.Front;
                }
                else if (control._moveDir.y < 0)
                {
                    _currentAngle = WalkAngle.Back;
                }
                else if (control._moveDir.x < 0)
                {
                    _currentAngle = WalkAngle.Left;
                    control.ChangeAnimation(_stateAnimations[(int)_currentAngle]);
                }
                else if (control._moveDir.x > 0)
                {
                    _currentAngle = WalkAngle.Right;
                    control.ChangeAnimation(_stateAnimations[(int)_currentAngle]);
                }
                control.ChangeAnimation(_stateAnimations[(int)_currentAngle]);
            }

            public void OnFixedUpdate(StateController control)
            {
                control.ChackFallOnGround();               
                if (control._moveDir.y > 0 && _currentAngle != WalkAngle.Front)
                {
                    control._moveController.MoveBreak();
                    _currentAngle = WalkAngle.Front;
                    control.ChangeAnimation(_stateAnimations[(int)_currentAngle]);
                }
                else if (control._moveDir.y < 0 && _currentAngle != WalkAngle.Back)
                {
                    control._moveController.MoveBreak();
                    _currentAngle = WalkAngle.Back;
                    control.ChangeAnimation(_stateAnimations[(int)_currentAngle]);
                }
                else if (control._moveDir.x < 0 && control._moveDir.y == 0 && _currentAngle != WalkAngle.Left)
                {
                    control._moveController.MoveBreak();
                    _currentAngle = WalkAngle.Left;
                    control.ChangeAnimation(_stateAnimations[(int)_currentAngle]);
                }
                else if (control._moveDir.x > 0 && control._moveDir.y == 0 && _currentAngle != WalkAngle.Right)
                {
                    control._moveController.MoveBreak();
                    _currentAngle = WalkAngle.Right;
                    control.ChangeAnimation(_stateAnimations[(int)_currentAngle]);
                }
                else if (control._moveDir == Vector2.zero)
                {
                    control.ChangeState(StateType.Idle);
                    control._moveController.MoveBreak();
                }
            }

            public void OnUpdate(StateController control)
            {
            }
        }
    }
}
