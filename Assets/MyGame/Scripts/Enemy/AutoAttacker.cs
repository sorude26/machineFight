using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class AutoAttacker : MonoBehaviour
{
    [SerializeField]
    private float _lockOnWide = 0.6f;
    [Tooltip("カメラ位置")]
    [SerializeField]
    private Transform _bodyTrans = default;
    [Tooltip("障害レイヤー")]
    [SerializeField]
    private LayerMask _wallLayer = default;
    [SerializeField]
    private float LockOnRange = 500;
    [SerializeField]
    private float _attackInterval = 3f;
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
        _lockBody.forward = Vector3.Lerp(_lockBody.forward, _playerLock.forward, _lockSpeed * Time.fixedDeltaTime);
    }
    private void CheckLockOn()
    {
        if (Vector3.Distance(_player.position, _bodyTrans.position) > LockOnRange)
        {
            return;
        }
        Vector3 targetDir = _player.position - _bodyTrans.position;
        float range = Vector3.Distance(_bodyTrans.position, _player.position);
        if (ChackAngle(targetDir) && !Physics.Raycast(_bodyTrans.position, targetDir, range, _wallLayer))
        {
            _playerLock.LookAt(_player);
            if (_attackTimer <= 0)
            {
                ExecuteAttack();
            }
        }
        else
        {
            _playerLock.forward = _attackerMoveBody.forward;
        }
    }
    private void ExecuteAttack()
    {
        _onAttackEvent?.Invoke();
        _attackTimer = _attackInterval;
    }
    /// <summary>
    /// 範囲内判定を行う
    /// </summary>
    /// <param name="targetDir"></param>
    /// <returns></returns>
    private bool ChackAngle(Vector3 targetDir)
    {
        var angle = Vector3.Dot(targetDir.normalized, _bodyTrans.forward);
        return angle > _lockOnWide;
    }
}
