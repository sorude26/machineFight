using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public partial class LegStateContext
{
    private class JumpState : ILegState
    {
        private readonly float jumpTime = 0.6f;
        private readonly float stateChangeTime = 0.5f;
        private float _timer = 0f;
        private bool _isJump = false;
        public void ExecuteEnter(LegStateContext context)
        {
            context.ChangeAnimation(context.AnimeName.Jump);
            _timer = 0f;
            _isJump = false;
        }

        public void ExecuteFixedUpdate(LegStateContext context)
        {
            if (_timer < jumpTime && _isJump == false)
            {
                _timer += Time.fixedDeltaTime;
                if (_timer >= jumpTime)
                {
                    _isJump = true;
                    _timer = 0f;
                    Vector3 dir = context.BodyTrans.forward * context._moveDir.z + context.BodyTrans.right * context._moveDir.x;
                    context._moveController.AddImpulse((dir + Vector3.up).normalized * context.ActionParam.JumpPower);
                }
            }
            else if (_timer < stateChangeTime && _isJump == true)
            {
                context.LegTrans.localRotation = Quaternion.Lerp(context.LegTrans.localRotation, Quaternion.identity, Time.fixedDeltaTime);
                _timer += Time.fixedDeltaTime;
                if (_timer >= stateChangeTime)
                {
                    context.ChangeState(context._fallState);
                }
            }
        }
    }
}
