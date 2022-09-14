using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MyGame
{

    public class TestPlayerController : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody _rb = default;
        [SerializeField]
        private float _speed = 5f;
        [SerializeField]
        private float _rotationSpeed = 1f;
        [SerializeField]
        private float _dotSub = 0.5f;
        public Transform LegBase = default;
        public Transform LockTrans = default;
        private MoveController _moveController = default;
        private void Start()
        {
            PlayerInput.SetEnterInput(InputType.Jump,Jump);
            _moveController = new MoveController(_rb);
        }
        private void Jump()
        {

        }
        private void FixedUpdate()
        {
            Vector3 dir = new Vector3(PlayerInput.MoveDir.x, 0, PlayerInput.MoveDir.y);
            Vector3 lockDir = transform.forward;
            if (dir != Vector3.zero)
            {
                lockDir = dir.normalized;
                if (dir.z < 0)
                {
                    lockDir *= -1;
                }
            }
            LegBase.forward = Vector3.Lerp(LegBase.forward, lockDir, _rotationSpeed * Time.fixedDeltaTime);
            if (Vector3.Dot(lockDir,LegBase.forward) < _dotSub)
            {
                if (dir != Vector3.back)
                {
                    dir = Vector3.zero;
                }
            }
            _moveController.VelocityMove(dir.normalized * _speed);
        }
    }
}