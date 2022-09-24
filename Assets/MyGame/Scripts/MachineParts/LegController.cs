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
        private LegAnimation _animeName = default;
        [SerializeField]
        private LegActionParam _actionParam = default;
        public Transform LegBase = default;
        public Transform LockTrans = default;
        private MoveController _moveController = default;
        private LegStateContext _stateContext = default;
        private void Start()
        {
            PlayerInput.SetEnterInput(InputType.Jump, Jump);
            _moveController = new MoveController(_moveRb);
            _stateContext = new LegStateContext(_legAnimator, _moveController);
            _stateContext._actionParam = _actionParam;
            _stateContext._animeName = _animeName;
            _stateContext.LegTrans = LegBase;
            _stateContext.BodyTrans = LockTrans;
        }
        private void FixedUpdate()
        {
            Vector3 dir = new Vector3(PlayerInput.MoveDir.x, 0, PlayerInput.MoveDir.y);
            _stateContext.ExecuteUpdate(dir, false, false);
        }
        private void Jump()
        {

        }
    }
}