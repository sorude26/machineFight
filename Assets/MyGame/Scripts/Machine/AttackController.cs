using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace MyGame.MachineFrame
{
    public class AttackController : MonoBehaviour
    {
        [SerializeField]
        private Transform _target = default;
        [SerializeField]
        private HandAimController _leftHand = default;
        [SerializeField]
        private HandAimController _rightHand = default;
        [SerializeField]
        private Rig _bodyRig = default;
        [SerializeField]
        private Transform _body = default;
        [SerializeField]
        private Transform _controlTarget = default;
        [SerializeField]
        private float _attackWaitTime = 1f;
        private float _resetSpeed = 2f;
        private bool _isAttack = false;
        private bool _isLockOn = false;
        private WaitForFixedUpdate _waitFixed = new WaitForFixedUpdate();
        private WaitForSeconds _waitAttack = default;
        private Quaternion _bodyRotaion = Quaternion.Euler(0, 0, 0);
        private Vector3 _targetCurrent = default;
        private Vector3 _targetBefore = default;
        private Vector3 _targetTwoBefore = default;
        public float BodyRSpeed { get; set; } = 8.0f;
        public float BodyTurnRange { get; set; } = 0.4f;
        public float WeaponShotSpeed { get; set; } = 120f;
        private void Start()
        {
            _leftHand.OnAttackEnd += ResetAngle;
            _rightHand.OnAttackEnd += ResetAngle;
        }
        public void Attack1()
        {
            if (ChackAngle())
            {
                return;
            }
            Attack();
            _leftHand.Attack(_target);
            _rightHand.Attack(_target);
        }
        private void Update()
        {
            if (_isLockOn)
            {
                _bodyRig.weight += _resetSpeed * Time.deltaTime;
            }
            else if (_bodyRig.weight > 0)
            {
                _bodyRig.weight -= Time.deltaTime;
            }
        }
        private void FixedUpdate()
        {
            _body.localRotation = Quaternion.Lerp(_body.localRotation, _bodyRotaion, BodyRSpeed * Time.fixedDeltaTime);
            _leftHand.PartsMotion();
            _rightHand.PartsMotion();
        }
        private void ResetAngle()
        {
            _bodyRotaion = Quaternion.identity;
            _isLockOn = false;
            _isAttack = false;
        }
        /// <summary>
        /// ターゲットの方向に胴体旋回する
        /// </summary>
        /// <param name="targetPos"></param>
        private void LockOn(Vector3 targetPos)
        {
            Vector3 targetDir = targetPos - _body.position;
            targetDir.y = 0.0f; //水平方向にする
            if (BodyTurnRange > 0 && Vector3.Dot(targetDir.normalized, _body.forward) < BodyTurnRange)
            {
                return;
            }
            _targetCurrent = ShotPrediction.Circle(_body.position, targetPos, _targetBefore, _targetTwoBefore, WeaponShotSpeed);
            targetDir = _targetCurrent - _body.position;
            targetDir.y = 0.0f; //水平方向にする
            _controlTarget.forward = targetDir;
            _bodyRotaion = _controlTarget.localRotation;
            _targetTwoBefore = _targetBefore;
            _targetBefore = targetPos;
        }
        private bool ChackAngle()
        {
            Vector3 targetDir = _target.position - _body.position;
            targetDir.y = 0.0f;
            return BodyTurnRange > 0 && Vector3.Dot(targetDir.normalized, _body.forward) < BodyTurnRange;
        }
        public void Attack()
        {
            if (_waitAttack is null)
            {
                _waitAttack = new WaitForSeconds(_attackWaitTime);
            }
            if (_isAttack)
            {
                return;
            }
            _isAttack = true;
            _isLockOn = true;
            StartCoroutine(AttackImpl());
        }
        private IEnumerator AttackImpl()
        {
            while (_isAttack)
            {
                LockOn(_target.position);                
                yield return _waitFixed;
            }
        }
    }
}