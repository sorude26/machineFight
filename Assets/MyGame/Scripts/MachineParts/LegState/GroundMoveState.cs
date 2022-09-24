using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public partial class LegStateContext
{
    protected class GroundMoveState : ILegState
    {
        private enum LegAngle
        {
            Idle,
            Stop,
            Front,
            Back,
            Left,
            Right,
        }
        private LegAngle _currentAngle;
        private void LegMove(LegStateContext context)
        {
            if (context._moveDir == Vector3.zero)
            {
                if (_currentAngle != LegAngle.Stop)
                {
                    _currentAngle = LegAngle.Stop;
                    context.ChangeAnimetion(context._animeName.Idle);
                }
                context._moveController.MoveBreak();
                return;
            }
            Vector3 dir = context._moveDir.normalized;
            Vector3 lockDir = dir;
            if (dir.z < 0)
            {
                lockDir *= -1;
                if (_currentAngle != LegAngle.Back)
                {
                    _currentAngle = LegAngle.Back;
                    context.ChangeAnimetion(context._animeName.Back);
                }
            } else if (dir == -context.LegTransform.forward)
            {
                lockDir = context.LegTransform.forward + context.BodyTransform.forward;
            }
            context.LegTransform.forward = Vector3.Lerp(context.LegTransform.forward, lockDir,context._actionParam.WalkTurnSpeed * Time.fixedDeltaTime);
            if (Vector3.Dot(lockDir, context.LegTransform.forward) < context._actionParam.DotSub)
            {
                if (_currentAngle != LegAngle.Back)
                {
                    var angle = Vector3.Cross(context.LegTransform.forward, context.BodyTransform.forward);
                    if (angle.y < 0)
                    {
                        if (_currentAngle != LegAngle.Left)
                        {
                            _currentAngle = LegAngle.Left;
                            context.ChangeAnimetion(context._animeName.TurnLeft);
                        }
                    }
                    else if (angle.y > 0)
                    {
                        if (_currentAngle != LegAngle.Right)
                        {
                            _currentAngle = LegAngle.Right;
                            context.ChangeAnimetion(context._animeName.TurnRight);
                        }
                    }
                    dir = Vector3.zero;
                }
            }
            else if (_currentAngle != LegAngle.Front && dir.z >= 0)
            {
                _currentAngle = LegAngle.Front;
                context.ChangeAnimetion(context._animeName.Walk);
            }
            context._moveController.VelocityMove(dir * context._actionParam.WalkSpeed);
        }
        public void ExecuteEnter(LegStateContext context) 
        {
            _currentAngle = LegAngle.Idle;
        }

        public void ExecuteExit(LegStateContext context) {}

        public void ExecuteUpdate(LegStateContext context) 
        {
            if (context._jumpInput == true)
            {
                context.ChangeState(context._jumpState);
                return;
            }
            if (context._groundCheck == false)
            {

            }
            LegMove(context);
        }
    }
}