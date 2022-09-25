using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public partial class LegStateContext
{
    private class GroundMoveState : ILegState
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
        private LegAngle _currentAngle = default;
        private float _turnSpeed = default;
        private float _walkSpeed = default;
        private void LegMove(LegStateContext context)
        {
            if (context._moveDir == Vector3.zero)
            {
                if (_currentAngle != LegAngle.Stop)
                {
                    _currentAngle = LegAngle.Stop;
                    context.ChangeAnimation(context.AnimeName.Idle);
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
                    context.ChangeAnimation(context.AnimeName.Back);
                }
                moveDir = -moveDir;
            }
            float local = Mathf.Abs(context.LegTrans.localRotation.y);
            float target = Mathf.Abs(lockQ.y);
            context.LegTrans.localRotation = Quaternion.Lerp(context.LegTrans.localRotation, lockQ, _turnSpeed * Time.fixedDeltaTime);
            if (_currentAngle != LegAngle.Back && Mathf.Max(local, target) - Mathf.Min(local, target) > context.ActionParam.DotSub)
            {
                float angleY = context.LegTrans.localRotation.y - lockQ.y;
                if (angleY > 0)
                {
                    if (_currentAngle != LegAngle.Left)
                    {
                        _currentAngle = LegAngle.Left;
                        context.ChangeAnimation(context.AnimeName.TurnLeft);
                    }
                }
                else if (angleY < 0)
                {
                    if (_currentAngle != LegAngle.Right)
                    {
                        _currentAngle = LegAngle.Right;
                        context.ChangeAnimation(context.AnimeName.TurnRight);
                    }
                }
                context._moveController.VelocityMove(Vector3.zero);
                return;
            }
            else if (_currentAngle != LegAngle.Front && dir.z >= 0)
            {
                _currentAngle = LegAngle.Front;
                context.ChangeAnimation(context.AnimeName.Walk);
            }
            context._moveController.VelocityMove(moveDir * _walkSpeed);
        }
        public void ExecuteEnter(LegStateContext context)
        {
            _currentAngle = LegAngle.Idle;
            _turnSpeed = context.ActionParam.WalkTurnSpeed;
            _walkSpeed = context.ActionParam.WalkSpeed;
        }

        public void ExecuteFixedUpdate(LegStateContext context)
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