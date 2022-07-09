using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.MachineFrame
{
    public partial class StateController
    {
        protected class StateIdle : IMachineState
        {
            public void OnEnter(StateController control)
            {
                control.SetState(StateType.Idle);
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