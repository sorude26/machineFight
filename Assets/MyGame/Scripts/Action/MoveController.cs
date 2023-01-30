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
        private const float GRAVITYE = -0.2f;
        private const float INERTIA_RANGE = 0.1f;
        private const float INERTIA_POWER = 9f;
        private const float INERTIA_HAFE = 0.5f;
        private Rigidbody _rb = default;
        public MoveController(Rigidbody rigidbody)
        {
            _rb = rigidbody;
        }
        public void VelocityMoveInertia(Vector3 dir)
        {
            var current = _rb.velocity;
            dir += current * INERTIA_POWER;
            dir *= INERTIA_RANGE;
            _rb.velocity = dir;
        }
        public void VelocityMove(Vector3 dir)
        {
            var current = _rb.velocity;
            dir += current;
            dir *= INERTIA_HAFE;
            _rb.velocity = dir;
        }
        public void GVelocityMove(Vector3 dir)
        {
            dir.y = _rb.velocity.y + GRAVITYE;
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
        public void FlyDecelerate(float decelerate = DEFAULT_DECELERATE)
        {
            _rb.velocity = _rb.velocity * decelerate;
        }
        public void FloatDecelerate(float decelerate = DEFAULT_DECELERATE, float breakPower = BRAKE_DECELERATE)
        {
            var velocity = _rb.velocity;
            float y = velocity.y;
            if (y < 0)
            {
                y *= breakPower;
            }
            velocity *= decelerate;
            velocity.y = y;
            _rb.velocity = velocity;
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