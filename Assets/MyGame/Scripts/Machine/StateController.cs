using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.MachineFrame
{
    public partial class StateController
    {
        private MoveController _moveController = default;
        private AnimatorController _animatorController = default;
        public StateController(MoveController moveController,AnimatorController animatorController)
        {
            _moveController = moveController;
            _animatorController = animatorController;
        }
        public void Update()
        {

        }
        public void FixedUpdate()
        {

        }
    }
}