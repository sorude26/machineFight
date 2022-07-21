using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.MachineFrame
{
    /// <summary>
    /// AnimationÇÃëÄçÏÇ∆ÉCÉxÉìÉgÇàµÇ§
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class AnimatorController : MonoBehaviour
    {
        public const float DEFAULT_CHSNGE_SPEED = 0.5f;
        private Animator _animator = default;
        public string[] StateAnimations = default;
        public event Action OnMove = default;
        public event Action OnJump = default;
        public event Action OnStop = default;
        public event Action OnTurn = default;
        public event Action OnJumpEnd = default;
        public event Action OnLandingEnd = default;
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        private void AnimationEventMove()
        {
            OnMove?.Invoke();
        }
        private void AnimationEventJump()
        {
            OnJump?.Invoke();
        }
        private void AnimationEventStop()
        {
            OnStop?.Invoke();
        }
        private void AnimationEventTurn()
        {
            OnTurn?.Invoke();
        }
        private void AnimationEventJumpEnd()
        {
            OnJumpEnd?.Invoke();
        }
        private void AnimationEventLandingEnd()
        {
            OnLandingEnd?.Invoke();
        }
        public void ChangeAnimation(StateType state, float changeSpeed = DEFAULT_CHSNGE_SPEED)
        {
            if (StateAnimations.Length <= (int)state)
            {
                return;
            }
            _animator.CrossFadeInFixedTime(StateAnimations[(int)state], changeSpeed);
        }
        public void ChangeAnimation(string taragetAnimation, float changeSpeed = DEFAULT_CHSNGE_SPEED)
        {
            if (changeSpeed == 0)
            {
                _animator.Play(taragetAnimation);
                return;
            }
            _animator.CrossFadeInFixedTime(taragetAnimation, changeSpeed);
        }
    }
}