using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class AutoAttacker : MonoBehaviour
{
    [SerializeField]
    private float _lockOnWide = 0.6f;
    [Tooltip("�J�����ʒu")]
    [SerializeField]
    private Transform _bodyTrans = default;
    [Tooltip("��Q���C���[")]
    [SerializeField]
    private LayerMask _wallLayer = default;
    [SerializeField]
    private float _rayWide = 1f;
    [SerializeField]
    private float LockOnRange = 500;
    [SerializeField]
    private float _attackMaxInterval = 3f;
    [SerializeField]
    private float _attackIntervalDiffusivity = 0f;
    [SerializeField]
    private UnityEvent _onAttackEvent = default;
    [SerializeField]
    private Transform _attackerMoveBody = default;
    [SerializeField]
    private Transform _playerLock = default;
    [SerializeField]
    private Transform _lockBody = default;
    [SerializeField]
    private float _lockSpeed = 5f;
    private float _attackTimer = 0f;
    private Transform _player = default;
    public bool IsAttackMode { get; private set; }
    private void Start()
    {
        _player = NavigationManager.Instance.Target;
    }
    private void FixedUpdate()
    {
        if (_attackTimer > 0)
        {
            _attackTimer -= Time.fixedDeltaTime;
        }
        CheckLockOn();
        _lockBody.localRotation = Quaternion.Lerp(_lockBody.localRotation, _playerLock.localRotation, _lockSpeed * Time.fixedDeltaTime);
    }
    private void CheckLockOn()
    {
        if (Vector3.Distance(_player.position, _bodyTrans.position) > LockOnRange)
        {
            IsAttackMode = false;
            return;
        }
        Vector3 targetDir = _player.position - _bodyTrans.position;
        float range = Vector3.Distance(_bodyTrans.position, _player.position);
        if (ChackAngle(targetDir) && !Physics.SphereCast(_bodyTrans.position, _rayWide, targetDir, out RaycastHit hit, range, _wallLayer))
        {
            IsAttackMode = true;
            _playerLock.LookAt(_player);
            if (_attackTimer <= 0)
            {
                ExecuteAttack();
            }
        }
        else
        {
            IsAttackMode = false;
            _playerLock.forward = _attackerMoveBody.forward;
        }
    }
    private void ExecuteAttack()
    {
        _onAttackEvent?.Invoke();
        _attackTimer = Random.Range(_attackMaxInterval - _attackIntervalDiffusivity, _attackMaxInterval);
    }
    /// <summary>
    /// �͈͓�������s��
    /// </summary>
    /// <param name="targetDir"></param>
    /// <returns></returns>
    private bool ChackAngle(Vector3 targetDir)
    {
        var angle = Vector3.Dot(targetDir.normalized, _bodyTrans.forward);
        return angle > _lockOnWide;
    }
}
