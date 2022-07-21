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
        float _turnSpeed = 5f;
        [SerializeField]
        float _turnPower = 5f;
        [SerializeField]
        float _jumpPower = 10f;
        [SerializeField]
        private WallChecker _groundChecker = default;
        [SerializeField]
        private Rigidbody _rigidbody = default;
        [SerializeField]
        private AnimatorController _animatorController = default;
        private MoveController _moveController = default;
        private StateController _stateController = default;
        private Quaternion _baseRotation = Quaternion.identity;
        public Transform BaseTransform = null;
        public Vector2 MoveDir { get; set; }
        private void Start()
        {
            _moveController = new MoveController(_rigidbody);
            _animatorController.OnMove += WalkMove;
            _animatorController.OnTurn += MachineTurn;
            _animatorController.OnJump += JumpMove;
            _animatorController.OnStop += MoveBreak;
            _stateController = new StateController(_moveController, _animatorController, _groundChecker);
        }
        private void FixedUpdate()
        {
            _stateController.FixedUpdate(MoveDir);
            BaseTransform.localRotation = Quaternion.Lerp(BaseTransform.localRotation, _baseRotation, _turnSpeed * Time.fixedDeltaTime);
        }
        private void WalkMove()
        {
            //_moveController.VelocityMove(BaseTransform.forward * _moveSpeed * MoveDir.y + BaseTransform.right * MoveDir.x);
            _moveController.AddImpulse(BaseTransform.forward * MoveDir.y * _moveSpeed + BaseTransform.right * MoveDir.x);
        }
        private void JumpMove()
        {
            _moveController.AddImpulse((BaseTransform.forward * MoveDir.y + BaseTransform.right * MoveDir.x + Vector3.up).normalized * _jumpPower);
        }
        private void MoveBreak()
        {
            _moveController.MoveBreak();
        }       
        private void MachineTurn()
        {
            _baseRotation = Quaternion.Euler(Vector3.up * MoveDir.x * _turnPower) * _baseRotation;
        }
        public void InputJump()
        {
            _stateController.InputJump();
        }
    }
}
