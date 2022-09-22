using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MyGame
{
    public class LegController : MonoBehaviour
    {
        private enum LegAngle
        {
            Stop,
            Front,
            Back,
            Left,
            Right,
        }
        [SerializeField]
        private Rigidbody _moveRb = default;
        [SerializeField]
        private Animator _legAnimator = default;
        [SerializeField]
        private float _speed = 5f;
        [SerializeField]
        private float _rotationSpeed = 1f;
        [SerializeField]
        private float _dotSub = 0.5f;
        [SerializeField]
        private float _animeChangeSpeed = 0.2f;
        [SerializeField]
        private string[] _moveAnimeName = { "Idle", "Walk", "Back", "TurnLeft", "TurnRight" };
        private LegAngle _angle = default;
        private LegAngle _currentAngle = default;
        public Transform LegBase = default;
        public Transform LockTrans = default;
        private MoveController _moveController = default;
        private void Start()
        {
            PlayerInput.SetEnterInput(InputType.Jump, Jump);
            _moveController = new MoveController(_moveRb);
        }
        private void FixedUpdate()
        {
            Vector3 dir = new Vector3(PlayerInput.MoveDir.x, 0, PlayerInput.MoveDir.y);
            Vector3 lockDir = transform.forward;
            if (dir != Vector3.zero)
            {
                _angle = LegAngle.Front;
                lockDir = dir.normalized;
                if (dir.z < 0)
                {
                    lockDir *= -1;
                    _angle = LegAngle.Back;
                }
            }
            else
            {
                _angle = LegAngle.Stop;
            }
            LegBase.forward = Vector3.Lerp(LegBase.forward, lockDir, _rotationSpeed * Time.fixedDeltaTime);
            if (Vector3.Dot(lockDir, LegBase.forward) < _dotSub)
            {
                if (dir != Vector3.back)
                {
                    if (dir.x > 0)
                    {
                        _angle = LegAngle.Right;
                    }
                    else if (dir.x < 0)
                    {
                        _angle = LegAngle.Left;
                    }
                    else
                    {
                        _angle = LegAngle.Stop;
                    }
                    dir = Vector3.zero;
                }
            }
            if (_currentAngle != _angle)
            {
                _currentAngle = _angle;
                ChangeAnimetion(_moveAnimeName[(int)_angle]);
            }
            _moveController.VelocityMove(dir.normalized * _speed);
        }
        private void ChangeAnimetion(string target)
        {
            _legAnimator.CrossFadeInFixedTime(target, _animeChangeSpeed);
        }
        private void Jump()
        {

        }
    }
}