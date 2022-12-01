using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public partial class LegStateContext
{
    public class GroundStepState : ILegState
    {
        private readonly float SIDE_RANGE = 0.5f;
        private float _turnSpeed = 10f;
        private float _identiySpeed = 10f;
        private float _upPower = 3f;
        private float _stepTime = 0.6f;
        private float _timer = 0;
        public void ExecuteEnter(LegStateContext context)
        {
            _upPower = context.ActionParam.StepUpPower;
            var moveDir = context._moveDir;
            if (moveDir.z > 0)
            {
                if (moveDir.x < -SIDE_RANGE)
                {
                    context.ChangeAnimation(context.AnimeName.StepLeft);
                }
                else if (moveDir.x > SIDE_RANGE)
                {
                    context.ChangeAnimation(context.AnimeName.StepRight);
                }
                else
                {
                    context.ChangeAnimation(context.AnimeName.StepFront);
                }
            }
            else if (moveDir.z < 0)
            {
                context.ChangeAnimation(context.AnimeName.StepBack);
            }
            else
            {
                context.ChangeState(context._jumpState);
                return;
            }
            _timer = _stepTime;
            moveDir = context.BodyTrans.forward * moveDir.z + context.BodyTrans.right * moveDir.x;
            moveDir = moveDir.normalized * context.ActionParam.JumpPower + Vector3.up * _upPower;
            context._moveController.AddImpulse(moveDir);
        }
        public void ExecuteFixedUpdate(LegStateContext context)
        {
            var rootR = context.BodyTrans.localRotation;
            context.LegBaseTrans.localRotation = Quaternion.Lerp(context.LegBaseTrans.localRotation, rootR, _turnSpeed * Time.fixedDeltaTime);
            context.LegTrans.localRotation = Quaternion.Lerp(context.LegTrans.localRotation, Quaternion.identity, _identiySpeed * Time.fixedDeltaTime);
            _timer -= Time.fixedDeltaTime;
            if (_timer <= 0)
            {
                context.ChangeState(context._fallState);
            }
        }
    }
}