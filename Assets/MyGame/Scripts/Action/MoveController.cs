using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    /// <summary>
    /// ˆÚ“®‚ğ‘€ì‚·‚é
    /// </summary>
    public class MoveController
    {
        /// <summary> Œ¸‘¬’l </summary>
        private const float DEFAULT_DECELERATE = 0.99f;
        /// <summary> ’â~Œ¸‘¬’l </summary>
        private const float BRAKE_DECELERATE = 0.3f;
        /// <summary> d—Í’l </summary>
        private const float GRAVITYE = -0.2f;
        /// <summary> Šµ«Œ¸Š </summary>
        private const float INERTIA_RANGE = 0.1f;
        /// <summary> Šµ«‰e‹¿’l </summary>
        private const float INERTIA_POWER = 9f;
        /// <summary> Šµ«’l </summary>
        private const float INERTIA_HAFE = 0.5f;
        private Rigidbody _rb = default;
        public MoveController(Rigidbody rigidbody)
        {
            _rb = rigidbody;
        }
        /// <summary>
        /// Šµ«‚ğ‹­‚­æ‚¹‚½ŠŠ‚ç‚©‚ÈˆÚ“®
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
        /// Šµ«‚ğæ‚¹‚½ŠŠ‚ç‚©‚ÈˆÚ“®
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
        /// d—Í‚ğ‹­‚­Š|‚¯‚½ˆÚ“®
        /// </summary>
        /// <param name="dir"></param>
        public void GVelocityMove(Vector3 dir)
        {
            dir.y = _rb.velocity.y + GRAVITYE;
            _rb.velocity = dir;
        }
        /// <summary>
        /// ‚ä‚Á‚­‚è‚Æ—Í‚ğŠ|‚¯‚éˆÚ“®
        /// </summary>
        /// <param name="dir"></param>
        public void AddMove(Vector3 dir)
        {
            _rb.AddForce(dir, ForceMode.Force);
        }
        /// <summary>
        /// ‹­‚­—Í‚ğŠ|‚¯‚éˆÚ“®
        /// </summary>
        /// <param name="dir"></param>
        public void AddImpulse(Vector3 dir)
        {
            _rb.AddForce(dir, ForceMode.Impulse);
        }
        /// <summary>
        /// ‰º~•ûŒü‚Ì—Í‚ğ‹­‚­Œ¸Š‚·‚éŒ¸‘¬
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
        /// ‹­‚ß‚ÌŒ¸‘¬
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
        /// ã‚ß‚ÌŒ¸‘¬
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