using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    /// <summary>
    /// 移動を操作する
    /// </summary>
    public class MoveController
    {
        private const float DEFAULT_DECELERATE = 0.99f;
        private const float BRAKE_DECELERATE = 0.3f;
        private Rigidbody _rb = default;
        public MoveController(Rigidbody rigidbody)
        {
            _rb = rigidbody;
        }
        public void VelocityMove(Vector3 dir)
        {
            _rb.velocity = dir;
        }
        public void AddMove(Vector3 dir)
        {
            _rb.AddForce(dir, ForceMode.Force);
        }
        public void AddImpulse(Vector3 dir)
        {
            _rb.AddForce(dir, ForceMode.Impulse);
        }
        public void MoveBreak(float breakPower = BRAKE_DECELERATE)
        {
            var velocity = _rb.velocity;
            float y = velocity.y;
            velocity *= breakPower;
            velocity.y = y;
            _rb.velocity = velocity;
        }
        public void MoveDecelerate(float decelerate = DEFAULT_DECELERATE)
        {
            var velocity = _rb.velocity;
            float y = velocity.y;
            velocity *= decelerate;
            velocity.y = y;
            _rb.velocity = velocity;
        }
    }
}