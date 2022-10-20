using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace MyGame
{
    public class MachinePartsController : MonoBehaviour
    {
        [SerializeField]
        private BodyController _bodyController = default;
        [SerializeField]
        private LegController _legController = default;
        private bool _isDown = false;
        public bool IsInitalized { get; private set; }
        public void Initialize()
        {
            _bodyController.Initialize();
            _legController.Initialize();
            IsInitalized = true;
        }
        public void ExecuteFixedUpdate(Vector3 dir)
        {
            if (_legController != null)
            {
                _legController.ExecuteFixedUpdate(dir);
            }
            if (_legController.IsFall == true)
            {
                _bodyController.BoostMove(dir);
            }
            _bodyController.ExecuteFixedUpdate(_legController.IsFall);
        }
        public void SetLockOn(Transform target)
        {
            _bodyController.TestTarget = target;
        }
        public void ExecuteJump()
        {
            if (_legController != null && _legController.IsFall == false)
            {
                _legController.Jump();
            }
            else
            {
                _bodyController.UpBoost();
            }
        }
        public void ExecuteJet(Vector3 dir)
        {
            _bodyController.AngleBoost(dir, _legController.IsFall);
        }
        public void ShotLeft()
        {
            _bodyController.ShotLeft();
        }
        public void ShotRight()
        {
            _bodyController.ShotRight();
        }
        public void PowerDownMachine()
        {
            if (_isDown == true)
            {
                return;
            }
            _isDown = true;
            _bodyController.IsDown = true;
            _legController.PowerDown();
        }
    }
}