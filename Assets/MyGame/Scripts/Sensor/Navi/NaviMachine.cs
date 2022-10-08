using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class NaviMachine : MonoBehaviour
    {
        [SerializeField]
        private MachinePartsController _machineController = default;
        [SerializeField]
        private Transform _lockTrans = default;
        [SerializeField]
        private Transform _body = default;
        [SerializeField]
        private float _transSpeed = 5f;
        [SerializeField]
        private float _naviInterval = 1f;
        private float _timer = 0f;
        private Vector3 _currentDir = Vector3.zero;
        private void FixedUpdate()
        {
            _timer += Time.fixedDeltaTime;
            if (_timer > _naviInterval)
            {
                _timer = 0;
                _currentDir = NavigationManager.Instance.GetMoveDir(_body);
            }
            Vector3 dir = Vector3.zero;
            if (_currentDir != Vector3.zero)
            {
                _lockTrans.forward = Vector3.Lerp(_lockTrans.forward, _currentDir, _transSpeed * Time.fixedDeltaTime);
                dir = Vector3.forward;
            }
            _machineController.ExecuteFixedUpdate(dir);
        }
    }
}