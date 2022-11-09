using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Generator�ɐ��䂳���Enemy
/// </summary>
public class GeneratorEnemyContoroller : MonoBehaviour
{
    [SerializeField]
    private DamageChecker[] damageCheckers = null;
    [SerializeField]
    private LockOnTarget[] _targets = null;
    /// <summary>
    /// ���S���p�C�x���g
    /// </summary>
    public event Action OnDeadEvent = default;
    public bool IsActive { get; private set; }
    /// <summary>
    /// ����������
    /// </summary>
    /// <param name="pos"></param>
    public void InitializeEnemy(Transform pos)
    {
        transform.position = pos.position;
        gameObject.SetActive(true);
        foreach (DamageChecker checker in damageCheckers)
        {
            checker.StartSet();
        }
        foreach (LockOnTarget target in _targets)
        {
            target.Initialize();
        }
    }
    private void OnEnable()
    {
        IsActive = true;
    }
    private void OnDisable()
    {
        IsActive = false;
        OnDeadEvent?.Invoke();
    }
}
