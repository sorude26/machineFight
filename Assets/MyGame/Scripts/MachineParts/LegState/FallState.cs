using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public partial class LegStateContext
{
    private class FallState : ILegState
    {
        public void ExecuteEnter(LegStateContext context)
        {
            context.ChangeAnimation(context.AnimeName.Fall);
            context.IsFall = true;
            context.IsFloat = false;
        }
        private float _turnSpeed = 2f;
        private float _identiySpeed = 2f;
        public void ExecuteFixedUpdate(LegStateContext context)
        {
            var rootR = context.BodyTrans.localRotation;
            context.LegBaseTrans.localRotation = Quaternion.Lerp(context.LegBaseTrans.localRotation, rootR, _turnSpeed * Time.fixedDeltaTime);
            context.LegTrans.localRotation = Quaternion.Lerp(context.LegTrans.localRotation, Quaternion.identity, _identiySpeed * Time.fixedDeltaTime);
            if (context._groundCheck == true)
            {
                context.IsFall = false;
                context.ChangeState(context._landingState);
            }
        }
    }
}