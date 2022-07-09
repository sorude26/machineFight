using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.MachineFrame
{
    public partial class StateController
    {
        protected class StateFall : IMachineState
        {
            public void OnEnter(StateController control)
            {
                control.ChangeAnimation(StateType.Fall);
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