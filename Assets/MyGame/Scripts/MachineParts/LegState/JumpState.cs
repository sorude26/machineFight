using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public partial class LegStateContext
{
    protected class JumpState : ILegState
    {
        private readonly float jumpTime = 0.6f;
        private readonly float stateChangeTime = 0.5f;
        private float _timer = 0f;
        private bool _isJump = false;
        public void ExecuteEnter(LegStateContext context)
        {
            context.ChangeAnimetion(context._animeName.Jump);
            _timer = 0f;
            _isJump = false;
        }

        public void ExecuteUpdate(LegStateContext context)
        {
            if (_timer < jumpTime && _isJump == false)
            {
                _timer += Time.fixedDeltaTime;
                if (_timer >= jumpTime)
                {
                    _isJump = true;
                    _timer = 0f;
                    context._moveController.AddImpulse(((context._moveDir + Vector3.up).normalized + Vector3.up) * context._actionParam.JumpPower);
                }
            }
            else if (_timer < stateChangeTime && _isJump == true)
            {
                _timer += Time.fixedDeltaTime;
                if (_timer >= stateChangeTime)
                {
                    context.ChangeState(context._fallState);
                }
            }
        }
    }
}
