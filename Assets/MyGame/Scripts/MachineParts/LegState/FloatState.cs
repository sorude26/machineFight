using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public partial class LegStateContext
{
    private class FloatState : ILegState
    {
        public void ExecuteEnter(LegStateContext context)
        {
            context.ChangeAnimation(context.AnimeName.FloatMove);
            context.IsFall = true;
            context.IsFloat = true;
        }

        private float _turnSpeed = 2f;
        private float _identiySpeed = 2f;
        public void ExecuteFixedUpdate(LegStateContext context)
        {
            var rootR = context.BodyTrans.localRotation;
            context.LegBaseTrans.localRotation = Quaternion.Lerp(context.LegBaseTrans.localRotation, rootR, _turnSpeed * Time.fixedDeltaTime);
            context.LegTrans.localRotation = Quaternion.Lerp(context.LegTrans.localRotation, Quaternion.identity, _identiySpeed * Time.fixedDeltaTime);
        }
    }
}