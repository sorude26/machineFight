using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MyGame
{
    public class MachinePartsController : MonoBehaviour
    {
        private const float BOOSTER_POWER = 2f;
        private const float UP_POWER = 0.2f;
        [SerializeField]
        private Rigidbody _moveRb = default;
        [SerializeField]
        private MachineBuilder _builder = default;
        [SerializeField]
        public UnityEvent _onDeadEvent = default;
        [SerializeField]
        private string _defaultLayerName = "MoveObj";
        [SerializeField]
        private string _deadLayerName = "AnHitMoveObj";
        private BodyController _bodyController = default;
        private LegController _legController = default;
        private MoveController _moveController = default;
        private bool _isDown = false;
        public bool IsFloat { get => _legController.IsFloat; }
        public bool IsFall { get => _legController.IsFall; }
        public bool IsInitalized { get; private set; }
        public DamageChecker DamageChecker { get => _bodyController.DamageChecker; }
        public BodyController BodyController { get => _bodyController; }
        public LegController LegController { get => _legController; }
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
        public void PlayDeadEvent()
        {
            if (IsInitalized == false)
            {
                return;
            }
            PowerDownMachine();
            _onDeadEvent?.Invoke();
            this.gameObject.layer = LayerMask.NameToLayer(_deadLayerName);
        }
        public void ExecuteFixedUpdate(Vector3 dir)
        {
            if (IsInitalized == false)
            {
                return;
            }
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
            if (IsInitalized == false)
            {
                return;
            }
            _bodyController.AttackTarget = target;
        }
        public void ExecuteJump()
        {
            if (IsInitalized == false)
            {
                return;
            }
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
            if (IsInitalized == false)
            {
                return;
            }
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
            if (IsInitalized == false)
            {
                return;
            }
            if (_bodyController.BackPack.BackPackWeapon != null)
            {
                _bodyController.BackPackBurst();
            }
            else
            {
                if (_legController.IsFall)
                {
                    _bodyController.MeleeAttackMove(BOOSTER_POWER);
                }
                else
                {
                    _bodyController.MeleeAttackMove(0, Vector3.up * UP_POWER);
                }
                //_legController.ChangeFloat();
            }
        }

        public void TryFloat()
        {
            if (IsInitalized == false)
            {
                return;
            }
            if (_legController.IsFloat) return;
            _legController.ChangeFloat();
            _bodyController.StartFloatBoosters();
        }

        public void TryGround()
        {
            if (IsInitalized == false)
            {
                return;
            }
            if (!_legController.IsFloat) return;
            _legController.ChangeFloat();
            _bodyController.StopFloatBoosters();
        }

        public void ShotLeft()
        {
            if (IsInitalized == false)
            {
                return;
            }
            _bodyController.ShotLeft();
        }
        public void ShotRight()
        {
            if (IsInitalized == false)
            {
                return;
            }
            _bodyController.ShotRight();
        }
        public void AttackLeg()
        {
            if (IsInitalized == false)
            {
                return;
            }
            _legController.Attack();
            _bodyController.MeleeAttackMove(UP_POWER);
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
        public void StartUpMachine()
        {
            _isDown = false;
            _bodyController.IsDown = false;
            _legController.StartUpLeg();
            _bodyController.StartUpBody();
            this.gameObject.layer = LayerMask.NameToLayer(_defaultLayerName);
        }
    }
}