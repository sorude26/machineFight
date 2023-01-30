using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    /// <summary>
    /// �ړ��𑀍삷��
    /// </summary>
    public class MoveController
    {
        /// <summary> �����l </summary>
        private const float DEFAULT_DECELERATE = 0.99f;
        /// <summary> ��~�����l </summary>
        private const float BRAKE_DECELERATE = 0.3f;
        /// <summary> �d�͒l </summary>
        private const float GRAVITYE = -0.2f;
        /// <summary> �������� </summary>
        private const float INERTIA_RANGE = 0.1f;
        /// <summary> �����e���l </summary>
        private const float INERTIA_POWER = 9f;
        /// <summary> �����l </summary>
        private const float INERTIA_HAFE = 0.5f;
        private Rigidbody _rb = default;
        public MoveController(Rigidbody rigidbody)
        {
            _rb = rigidbody;
        }
        /// <summary>
        /// �����������悹�����炩�Ȉړ�
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
        /// �������悹�����炩�Ȉړ�
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
        /// �d�͂������|�����ړ�
        /// </summary>
        /// <param name="dir"></param>
        public void GVelocityMove(Vector3 dir)
        {
            dir.y = _rb.velocity.y + GRAVITYE;
            _rb.velocity = dir;
        }
        /// <summary>
        /// �������Ɨ͂��|����ړ�
        /// </summary>
        /// <param name="dir"></param>
        public void AddMove(Vector3 dir)
        {
            _rb.AddForce(dir, ForceMode.Force);
        }
        /// <summary>
        /// �����͂��|����ړ�
        /// </summary>
        /// <param name="dir"></param>
        public void AddImpulse(Vector3 dir)
        {
            _rb.AddForce(dir, ForceMode.Impulse);
        }
        /// <summary>
        /// ���~�����̗͂������������錸��
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
        /// ���߂̌���
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
        /// ��߂̌���
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