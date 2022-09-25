using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MyGame
{
    public class LegController : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody _moveRb = default;
        [SerializeField]
        private Animator _legAnimator = default;
        [SerializeField]
        private WallChecker _groundChecker = default;
        [SerializeField]
        private LegAnimation _animeName = default;
        [SerializeField]
        private LegActionParam _actionParam = default;
        public Transform LegBase = default;
        public Transform LockTrans = default;
        private MoveController _moveController = default;
        private LegStateContext _stateContext = default;
        private bool _isJump = false;
        private void Start()
        {
            PlayerInput.SetEnterInput(InputType.Jump, Jump);
            _moveController = new MoveController(_moveRb);
            _stateContext = new LegStateContext(_legAnimator, _moveController);
            _stateContext.ActionParam = _actionParam;
            _stateContext.AnimeName = _animeName;
            _stateContext.LegTrans = LegBase;
            _stateContext.BodyTrans = LockTrans;
            _stateContext.InitializeState();
        }
        private void FixedUpdate()
        {
            Vector3 dir = new Vector3(PlayerInput.MoveDir.x, 0, PlayerInput.MoveDir.y);
            _stateContext.ExecuteFixedUpdate(dir, _isJump, _groundChecker.IsWalled());
            _isJump = false;
        }
        private void Jump()
        {
            _isJump = true;
        }
    }
}