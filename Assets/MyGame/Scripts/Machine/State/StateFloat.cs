using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.MachineFrame
{
    public partial class StateController
    {
        protected class StateFloat : IMachineState
        {
            public void OnEnter(StateController control)
            {
                control.SetState(StateType.Float);
            }

            public void OnFixedUpdate(StateController control)
            {
            }

            public void OnUpdate(StateController control)
            {
            }
        }
    }
}