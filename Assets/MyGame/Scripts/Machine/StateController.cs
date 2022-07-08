using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.MachineFrame
{
    public partial class StateController
    {
        private MoveController _moveController = default;
        private AnimatorController _animatorController = default;
        private IMachineState _currentState = default;
        private StateIdle _idle = default;
        public StateController(MoveController moveController,AnimatorController animatorController)
        {
            _moveController = moveController;
            _animatorController = animatorController;
            _idle = new StateIdle();
            _currentState = _idle;
        }
        public void Update()
        {
            _currentState.OnUpdate(this);
        }
        public void FixedUpdate()
        {
            _currentState.OnFixedUpdate(this);
        }
    }
}