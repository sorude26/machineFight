using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public partial class LegStateContext
{
    private class LandingState : ILegState
    {
        private readonly float landingEndTime = 0.5f;
        private float _timer = 0f;
        private int _landingSEID = 31;
        private float _seVolume = 0.1f;
        public void ExecuteEnter(LegStateContext context)
        {
            context.ChangeAnimation(context.AnimeName.Landing);
            context._moveController.MoveBreak();
            _timer = 0f;
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySE(_landingSEID, context.LegBaseTrans.position, _seVolume);
            }
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
