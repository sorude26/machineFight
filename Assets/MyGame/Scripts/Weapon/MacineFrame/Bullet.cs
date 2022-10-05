using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MyGame.MachineFrame
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody _rb = default;
        [SerializeField]
        private float _lifeTime = 1f;
        private float _timer = 0f;
        private void OnEnable()
        {
            _timer = 0;
        }
        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer > _lifeTime)
            {
                _timer = 0;
                _rb.velocity = Vector3.zero;
                gameObject.SetActive(false);
            }
        }
        public void Shot(Vector3 dir)
        {
            transform.forward = dir;
            _rb.AddForce(dir, ForceMode.Impulse);
        }
    }
}