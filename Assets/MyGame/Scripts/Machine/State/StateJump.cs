using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.MachineFrame
{
    public partial class StateController
    {
        protected class StateJump : IMachineState
        {
            public void OnEnter(StateController control)
            {
                control.SetState(StateType.Jump);
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