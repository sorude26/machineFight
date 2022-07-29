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
                control.ChangeAnimation(StateType.Landing, 0);
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