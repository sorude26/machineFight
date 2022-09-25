using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public partial class LegStateContext
{
    public class FallState : ILegState
    {
        public void ExecuteEnter(LegStateContext context)
        {
            context.ChangeAnimetion(context._animeName.Fall);
        }

        public void ExecuteUpdate(LegStateContext context)
        {
            if(context._groundCheck == true)
            {
                context.ChangeState(context._landingState);
            }
        }
    }
}