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
        /// <summary> 減速値 </summary>
        private const float DEFAULT_DECELERATE = 0.99f;
        /// <summary> 停止減速値 </summary>
        private const float BRAKE_DECELERATE = 0.3f;
        /// <summary> 重力値 </summary>
        private const float GRAVITYE = -0.2f;
        /// <summary> 慣性減衰 </summary>
        private const float INERTIA_RANGE = 0.1f;
        /// <summary> 慣性影響値 </summary>
        private const float INERTIA_POWER = 9f;
        /// <summary> 慣性値 </summary>
        private const float INERTIA_HAFE = 0.5f;
        private Rigidbody _rb = default;
        public MoveController(Rigidbody rigidbody)
        {
            _rb = rigidbody;
        }
        /// <summary>
        /// 慣性を強く乗せた滑らかな平行移動
        /// </summary>
        /// <param name="dir"></param>
        public void VelocityMoveInertia(Vector3 dir)
        {
            var current = _rb.velocity;
            dir += current * INERTIA_POWER;
            dir *= INERTIA_RANGE;
            _rb.velocity = dir;
        }
        /// <summary>
        /// 慣性を強く乗せた滑らかな移動
        /// </summary>
        /// <param name="dir"></param>
        public void GVelocityMoveInertia(Vector3 dir)
        {
            var current = _rb.velocity;
            dir += current * INERTIA_POWER;
            float y = _rb.velocity.y;
            dir *= INERTIA_RANGE;
            dir.y = y;
            _rb.velocity = dir;
        }
        /// <summary>
        /// 慣性を乗せた滑らかな移動
        /// </summary>
        /// <param name="dir"></param>
        public void VelocityMove(Vector3 dir)
        {
            var current = _rb.velocity;
            dir += current;
            dir *= INERTIA_HAFE;
            _rb.velocity = dir;
        }
        /// <summary>
        /// 重力を強く掛けた移動
        /// </summary>
        /// <param name="dir"></param>
        public void GVelocityMove(Vector3 dir)
        {
            dir.y = _rb.velocity.y + GRAVITYE;
            _rb.velocity = dir;
        }
        /// <summary>
        /// ゆっくりと力を掛ける移動
        /// </summary>
        /// <param name="dir"></param>
        public void AddMove(Vector3 dir)
        {
            _rb.AddForce(dir, ForceMode.Force);
        }
        /// <summary>
        /// 強く力を掛ける移動
        /// </summary>
        /// <param name="dir"></param>
        public void AddImpulse(Vector3 dir)
        {
            _rb.AddForce(dir, ForceMode.Impulse);
        }
        /// <summary>
        /// 下降方向の力を強く減衰する減速
        /// </summary>
        /// <param name="decelerate"></param>
        /// <param name="breakPower"></param>
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
        /// <summary>
        /// 強めの減速
        /// </summary>
        /// <param name="breakPower"></param>
        public void MoveBreak(float breakPower = BRAKE_DECELERATE)
        {
            var velocity = _rb.velocity;
            float y = velocity.y;
            velocity *= breakPower;
            velocity.y = y;
            _rb.velocity = velocity;
        }
        /// <summary>
        /// 弱めの減速
        /// </summary>
        /// <param name="decelerate"></param>
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