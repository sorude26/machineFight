using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MyGame
{
    public class MachinePartsController : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody _moveRb = default;
        [SerializeField]
        private MachineBuilder _builder = default;
        [SerializeField]
        public UnityEvent _onDeadEvent = default;
        private BodyController _bodyController = default;
        private LegController _legController = default;
        private MoveController _moveController = default;
        private bool _isDown = false;
        public bool IsInitalized { get; private set; }
        public DamageChecker DamageChecker { get => _bodyController.DamageChecker; }
        public BodyController BodyController { get => _bodyController; }
        public MachineBuilder Builder { get => _builder; }
        public void Initialize(PartsBuildParam buildParam)
        {
            _builder.Build(buildParam);
            _bodyController = _builder.Body;
            _legController = _builder.Leg;
            _moveController = new MoveController(_moveRb);
            _bodyController.Initialize(_moveController);
            _legController.Initialize(_moveController);
            _bodyController.OnBodyDestroy += PlayDeadEvent;
            IsInitalized = true;
        }
        private void PlayDeadEvent()
        {
            PowerDownMachine();
            _onDeadEvent.Invoke();
        }
        public void ExecuteFixedUpdate(Vector3 dir)
        {
            if (_legController != null)
            {
                _legController.ExecuteFixedUpdate(dir);
            }
            if (_legController.IsFall == true)
            {
                _bodyController.BoostMove(dir,_legController.IsFloat);
            }
            _bodyController.ExecuteFixedUpdate(_legController.IsFall);
        }
        public void SetLockOn(Transform target)
        {
            _bodyController.AttackTarget = target;
        }
        public void ExecuteJump()
        {
            if (_legController != null && _legController.IsFall == false && _legController.IsFloat == false)
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
            if (_legController != null && _legController.IsFall == false)
            {
                _legController.Step();
            }
            else
            {
                _bodyController.AngleBoost(dir, _legController.IsFall, _legController.IsFloat);
            }
        }
        public void ExecuteBurst()
        {
            if (_bodyController.BackPack.BackPackWeapon != null)
            {
                _bodyController.BackPackBurst();
            }
            else
            {
                _legController.ChangeFloat();
            }
        }
        public void ShotLeft()
        {
            _bodyController.ShotLeft();
        }
        public void ShotRight()
        {
            _bodyController.ShotRight();
        }
        public void AttackLeg()
        {
            _legController.Attack();
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