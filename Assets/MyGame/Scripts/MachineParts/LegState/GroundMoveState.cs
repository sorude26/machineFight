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
        private Quaternion _legR = Quaternion.identity;        
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
            Quaternion lockQ = Quaternion.Euler(0, (90f - Mathf.Abs(dir.z) * 45f) * dir.x, 0);
            Vector3 moveDir = context.LegTrans.forward;
            if (dir.z < 0)
            {
                lockQ = Quaternion.Euler(0, (90f - Mathf.Abs(dir.z) * 45f) * -dir.x, 0);
                if (_currentAngle != LegAngle.Back)
                {
                    _currentAngle = LegAngle.Back;
                    context.ChangeAnimetion(context._animeName.Back);
                }
                moveDir = -moveDir;
            }
            if (lockQ == Quaternion.Euler(0, -_legR.x, 0))
            {
                lockQ = Quaternion.identity;
            }
            float local = Mathf.Abs(context.LegTrans.localRotation.y);
            float target = Mathf.Abs(lockQ.y);
            context.LegTrans.localRotation = Quaternion.Lerp(context.LegTrans.localRotation, lockQ, context._actionParam.WalkTurnSpeed * Time.fixedDeltaTime);
            if (Mathf.Max(local, target) - Mathf.Min(local, target) > context._actionParam.DotSub)
            {
                if (_currentAngle != LegAngle.Back)
                {
                    float angleY = context.LegTrans.localRotation.y - lockQ.y;
                    if (angleY > 0)
                    {
                        if (_currentAngle != LegAngle.Left)
                        {
                            _currentAngle = LegAngle.Left;
                            context.ChangeAnimetion(context._animeName.TurnLeft);
                        }
                    }
                    else if (angleY < 0)
                    {
                        if (_currentAngle != LegAngle.Right)
                        {
                            _currentAngle = LegAngle.Right;
                            context.ChangeAnimetion(context._animeName.TurnRight);
                        }
                    }
                    context._moveController.VelocityMove(Vector3.zero);
                    return;
                }
            }
            else if (_currentAngle != LegAngle.Front && dir.z >= 0)
            {
                _currentAngle = LegAngle.Front;
                context.ChangeAnimetion(context._animeName.Walk);
            }
            context._moveController.VelocityMove(moveDir * context._actionParam.WalkSpeed);
        }
        public void ExecuteEnter(LegStateContext context)
        {
            _currentAngle = LegAngle.Idle;
        }

        public void ExecuteUpdate(LegStateContext context)
        {            
            if (context._groundCheck == false)
            {
                context.ChangeState(context._fallState);
                return;
            }
            if (context._jumpInput == true)
            {
                context.ChangeState(context._jumpState);
                return;
            }
            LegMove(context);
        }
    }
}