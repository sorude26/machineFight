using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public partial class LegStateContext
{
    private class LandingState : ILegState
    {
        private readonly float landingEndTime = 1f;
        private float _timer = 0f;
        public void ExecuteEnter(LegStateContext context)
        {
            context.ChangeAnimation(context.AnimeName.Landing);
            context._moveController.MoveBreak();
            _timer = 0f;
        }
        public void ExecuteFixedUpdate(LegStateContext context)
        {
            if (_timer < landingEndTime)
            {
                _timer += Time.fixedDeltaTime;
                if (_timer >= landingEndTime)
                {
                    context.ChangeState(context._groundState);
                }
            }
        }
    }
}
