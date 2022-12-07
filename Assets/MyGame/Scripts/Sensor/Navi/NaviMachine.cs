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
        private int _naviPower = 0;
        [SerializeField]
        private float _transSpeed = 5f;
        [SerializeField]
        private float _naviInterval = 1f;
        private float _timer = 0f;
        private Vector3 _currentDir = Vector3.zero;
        [SerializeField]
        private PartsBuildParam _buildParam;
        [SerializeField]
        private LockOnTarget _bodyTarget = default;
        [SerializeField]
        private GameObject _deadEffct = default;
        [SerializeField]
        private float _explosionTime = 3f;
        [SerializeField]
        private ShakeParam _exParam = default;
        [SerializeField]
        private PopController[] _popControllers = default;
        private void Start()
        {
            SetRandamBuildDat();
            _machineController.Initialize(_buildParam);
            _bodyTarget.SetChecker(_machineController.DamageChecker);
            _machineController.BodyController.LeftHand.WeaponBase.AnLimitAmmunition();
            _machineController.BodyController.RightHand.WeaponBase.AnLimitAmmunition();
            if (_machineController.BodyController.BackPack.BackPackWeapon != null)
            {
                _machineController.BodyController.BackPack.BackPackWeapon.AnLimitAmmunition();
            }
            _machineController.BodyController.AttackTarget = NavigationManager.Instance.Target;
            _machineController.BodyController.OnBodyDestroy += ExplosionMachine;
        }
        private void FixedUpdate()
        {
            if (_machineController.IsInitalized == false)
            {
                return;
            }
            _timer += Time.fixedDeltaTime;
            if (_timer > _naviInterval)
            {
                _timer = 0;
                _currentDir = NavigationManager.Instance.GetMoveDir(_body, _naviPower);
            }
            Vector3 dir = Vector3.zero;
            if (_currentDir != Vector3.zero)
            {
                _lockTrans.forward = Vector3.Lerp(_lockTrans.forward, _currentDir, _transSpeed * Time.fixedDeltaTime);
                dir = Vector3.forward;
            }
            _machineController.ExecuteFixedUpdate(dir);
        }
        public void ExeExecuteJet()
        {
            _machineController.ExecuteJet(_currentDir.normalized);
        }
        private void SetRandamBuildDat()
        {
            for (int i = 0; i < PartsBuildParam.PARTS_TYPE_NUM; i++)
            {
                _buildParam[(PartsType)i] = PartsManager.Instance.AllParamData.GetRandamPartsId((PartsType)i);
            }
        }
        public void ExplosionMachine()
        {
            StartCoroutine(ExplosionImpl());
        }
        private IEnumerator ExplosionImpl()
        {
            yield return new WaitForSeconds(_explosionTime);
            var effect = ObjectPoolManager.Instance.Use(_deadEffct);
            effect.transform.position = _body.position;
            effect.SetActive(true);
            _exParam.Pos = _body.position;
            StageShakeController.PlayShake(_exParam);
            if (_popControllers != null)
            {
                foreach (var controller in _popControllers)
                {
                    controller.PopItem();
                }
            }
            gameObject.SetActive(false);
        }
    }
}