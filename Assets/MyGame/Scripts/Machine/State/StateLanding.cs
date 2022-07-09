using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.MachineFrame
{
    public partial class StateController
    {
        protected class StateLanding : IMachineState
        {
            public void OnEnter(StateController control)
            {
                control._currentStateType = StateType.Landing;
                control.ChangeAnimation(StateType.Landing);
            }

            public void OnFixedUpdate(StateController control)
            {
                control.ChackFallOnGround();
            }

            public void OnUpdate(StateController control)
            {
            }
        }
    }
}