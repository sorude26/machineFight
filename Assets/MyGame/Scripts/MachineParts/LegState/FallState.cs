using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public partial class LegStateContext
{
    private class FallState : ILegState
    {
        public void ExecuteEnter(LegStateContext context)
        {
            context.ChangeAnimation(context.AnimeName.Fall);
        }

        public void ExecuteFixedUpdate(LegStateContext context)
        {
            if(context._groundCheck == true)
            {
                context.ChangeState(context._landingState);
            }
        }
    }
}