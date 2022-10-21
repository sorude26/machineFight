using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class LegStateContext
{
    private class DownState : ILegState
    {
        public void ExecuteEnter(LegStateContext context)
        {
            context.ChangeAnimation(context.AnimeName.Down);
        }

        public void ExecuteFixedUpdate(LegStateContext context)
        {
            context._moveController.MoveDecelerate();
        }
    }
}