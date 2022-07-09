using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.MachineFrame
{
    public class MachineController : MonoBehaviour
    {
        [SerializeField]
        float _moveSpeed = 10f;
        [SerializeField]
        private WallChecker _groundChecker = default;
        [SerializeField]
        private Rigidbody _rigidbody = default;
        [SerializeField]
        private AnimatorController _animatorController = default;
        private MoveController _moveController = default;
        private StateController _stateController = default;
        private void Start()
        {
            _moveController = new MoveController(_rigidbody);
            _animatorController.OnMove += WalkMove;
            _animatorController.OnJump += JumpMove;
            _animatorController.OnStop += MoveBreak;
            _stateController = new StateController(_moveController, _animatorController, _groundChecker);
        }
        private void FixedUpdate()
        {
            _stateController.FixedUpdate();
        }
        private void WalkMove()
        {
            _moveController.AddMove(transform.forward * _moveSpeed);
        }
        private void JumpMove()
        {
            _moveController.AddImpulse(transform.forward + Vector3.up * _moveSpeed);
        }
        private void MoveBreak()
        {
            _moveController.MoveBreak();
        }
        private bool IsGround()
        {
            return _groundChecker.IsWalled();
        }
    }
}
