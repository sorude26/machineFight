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
        private const float DEFAULT_CHSNGE_SPEED = 0.2f;
        private Animator _animator = default;
        public event Action OnMove = default;
        public event Action OnJump = default;
        public event Action OnStop = default;
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
        public void ChangeAnimation(string taragetAnimation, float changeSpeed = DEFAULT_CHSNGE_SPEED)
        {
            _animator.CrossFadeInFixedTime(taragetAnimation, changeSpeed);
        }
    }
}