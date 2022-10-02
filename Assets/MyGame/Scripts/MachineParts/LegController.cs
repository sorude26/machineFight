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
        public Transform BaseTrans = default;
        private MoveController _moveController = default;
        private LegStateContext _stateContext = default;
        private bool _isJump = false;
        private void Start()
        {
            _moveController = new MoveController(_moveRb);
            _stateContext = new LegStateContext(_legAnimator, _moveController);
            _stateContext.ActionParam = _actionParam;
            _stateContext.AnimeName = _animeName;
            _stateContext.LegTrans = LegBase;
            _stateContext.BodyTrans = LockTrans;
            _stateContext.LegBaseTrans = BaseTrans;
            _stateContext.InitializeState();
        }
        public void ExecuteFixedUpdate(Vector3 dir)
        {
            _stateContext.ExecuteFixedUpdate(dir, _isJump, _groundChecker.IsWalled());
            _isJump = false;
        }
        public void Jump()
        {
            _isJump = true;
        }
    }
}