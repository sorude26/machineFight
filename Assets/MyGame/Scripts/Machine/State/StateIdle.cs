using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.MachineFrame
{
    public partial class StateController
    {       
        protected class StateIdle : IMachineState
        {
            private readonly float CHANGE_WALK = 0.5f;
            public void OnEnter(StateController control)
            {
                control.SetState(StateType.Idle);
            }

            public void OnFixedUpdate(StateController control)
            {
                control._moveController.MoveBreak();
                control.ChackFallOnGround();
                if (control._moveDir.magnitude > CHANGE_WALK)
                {
                    control.ChangeState(StateType.Walk);
                }
            }

            public void OnUpdate(StateController control)
            {
            }
        }
    }
}