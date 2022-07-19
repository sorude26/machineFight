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
                control.SetState(StateType.Walk);
            }

            public void OnFixedUpdate(StateController control)
            {
                control.ChackFallOnGround();
                if (control._moveDir.y == 0)
                {
                    control.ChangeState(StateType.Idle);
                    control._moveController.MoveBreak();
                }
            }

            public void OnUpdate(StateController control)
            {
            }
        }
    }
}
