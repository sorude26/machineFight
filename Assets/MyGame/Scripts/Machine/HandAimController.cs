using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace MyGame.MachineFrame
{
    public class HandAimController : MonoBehaviour
    {
        private const float ATTACK_ANGLE = 0.999f;
        private readonly Quaternion HAND_ANGLE = Quaternion.Euler(-20, 0, 0);
        [SerializeField]
        private Transform _shoulder = default;
        [SerializeField]
        private Transform _hand = default;
        [SerializeField]
        private Transform _controlTarget = default;
        [SerializeField]
        private OverrideTransform _shoulderRig = default;
        [SerializeField]
        private OverrideTransform _handRig = default;
        [SerializeField]
        private WeaponBase _weapon = default;
        [SerializeField]
        private float _attackWaitTime = 1f;
        private float _rigWeight = 1;
        private float _resetSpeed = 4f;
        private Quaternion _topRotaion = Quaternion.identity;
        private Quaternion _handRotaion = Quaternion.identity;
        private Transform _targetTrans = default;
        private Vector3 _targetCurrent = default;
        private Vector3 _targetBefore = default;
        private Vector3 _targetTwoBefore = default;
        private bool _isAttack = false;
        private bool _isLockOn = false;
        private WaitForFixedUpdate _waitFixed = new WaitForFixedUpdate();
        private WaitForSeconds _waitAttack = default;
        public event Action OnAttackEnd = default;
        public float PartsRotaionSpeed { get; set; } = 3.0f;
        public float WeaponShotSpeed { get; set; } = 120f;
        private void ResetAngle()
        {
            _topRotaion = Quaternion.identity;
            _handRotaion = Quaternion.identity;
        }
        private void LockOn(Vector3 targetPos)
        {
            _targetCurrent = ShotPrediction.Circle(_hand.position, targetPos, _targetBefore, _targetTwoBefore, WeaponShotSpeed);
            _controlTarget.forward = _targetCurrent - _hand.position;
            _handRotaion = _controlTarget.rotation;
            _targetTwoBefore = _targetBefore;
            _targetBefore = targetPos;
        }
        private bool ChackAngle()
        {
            var angle = Quaternion.Dot(_handRotaion, _hand.localRotation);
            return angle > ATTACK_ANGLE || angle < -ATTACK_ANGLE;
        }
        public void PartsMotion()
        {
            _shoulder.localRotation = Quaternion.Lerp(_shoulder.localRotation, _topRotaion, PartsRotaionSpeed * Time.fixedDeltaTime);
            _hand.localRotation = Quaternion.Lerp(_hand.localRotation, _handRotaion, PartsRotaionSpeed * Time.fixedDeltaTime);
        }
        public void SetLockOn(Vector3 targetPos)
        {
            _targetBefore = targetPos;
            _targetTwoBefore = targetPos;
            LockOn(targetPos);
        }
        public void Attack(Transform target)
        {
            if (_waitAttack is null)
            {
                _waitAttack = new WaitForSeconds(_attackWaitTime);
            }
            _targetTrans = target;
            if (_isAttack || _isLockOn)
            {
                return;
            }
            _isAttack = true;
            _topRotaion = HAND_ANGLE;
            SetLockOn(_targetTrans.position);
            StartCoroutine(AttackImpl());
        }
        private void Update()
        {
            if (_isLockOn)
            {
                _handRig.weight += _resetSpeed * Time.deltaTime; 
                _shoulderRig.weight += _resetSpeed * Time.deltaTime;
            }
            else if (_handRig.weight > 0)
            {
                _handRig.weight -= Time.deltaTime;
                _shoulderRig.weight -= Time.deltaTime;
            }
        }
        private IEnumerator AttackImpl()
        {
            _isLockOn = true;
            while (_isAttack)
            {               
                LockOn(_targetTrans.position);
                if (ChackAngle() && _handRig.weight >= _rigWeight)
                {
                    _weapon.Fire();
                    _isAttack = false;
                    yield return _waitAttack;
                }
                yield return _waitFixed;
            }
            ResetAngle();
            _isAttack = false;
            _isLockOn = false;
            OnAttackEnd?.Invoke();
        }
    }
}