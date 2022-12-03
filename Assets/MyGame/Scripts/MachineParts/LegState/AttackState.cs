using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public partial class LegStateContext
{
    private class AttackState : ILegState
    {
        private readonly float attackEndTime = 0.7f;
        private float _timer = 0f;
        private float _turnSpeed = 2f;
        private float _identiySpeed = 5f;
        private float _forwardPower = 0.5f;
        private float _moveDecelerate = 0.9f;
        public void ExecuteEnter(LegStateContext context)
        {
            _timer = 0f;
            var moveDir = context._moveDir;
            context._legAnimator.Play(context.AnimeName.Attack);
            context._moveController.MoveBreak();
            moveDir = context.BodyTrans.forward * moveDir.z + context.BodyTrans.right * moveDir.x + context.BodyTrans.forward * _forwardPower;
            moveDir = moveDir.normalized * context.ActionParam.JumpPower * _forwardPower;
            context._moveController.AddImpulse(moveDir);
        }

        public void ExecuteFixedUpdate(LegStateContext context)
        {
            var rootR = context.BodyTrans.localRotation;
            context.LegBaseTrans.localRotation = Quaternion.Lerp(context.LegBaseTrans.localRotation, rootR, _turnSpeed * Time.fixedDeltaTime);
            context.LegTrans.localRotation = Quaternion.Lerp(context.LegTrans.localRotation, Quaternion.identity, _identiySpeed * Time.fixedDeltaTime);
            if (context.IsFall == false)
            {
                context._moveController.MoveDecelerate(_moveDecelerate);
            }
            if (_timer < attackEndTime)
            {
                _timer += Time.fixedDeltaTime;
                if (_timer >= attackEndTime)
                {
                    if (context.IsFall == false)
                    {
                        context.ChangeState(context._groundState);
                    }
                    else
                    {
                        context.ChangeState(context._fallState);
                    }
                }
            }
        }
    }
}