using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.MachineFrame
{
    public partial class StateController
    {
        protected class StateWalk : IMachineState
        {
            public void OnEnter(StateController control)
            {
            }

            public void OnFixedUpdate(StateController control)
            {
                control._moveController.MoveBreak();
            }

            public void OnUpdate(StateController control)
            {
            }
        }
    }
}