using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Generatorに制御されるEnemy
/// </summary>
public class GeneratorEnemyContoroller : MonoBehaviour
{
    [SerializeField]
    private DamageChecker[] damageCheckers = null;
    [SerializeField]
    private LockOnTarget[] _targets = null;
    /// <summary>
    /// 死亡時用イベント
    /// </summary>
    public event Action OnDeadEvent = default;
    public bool IsActive { get; private set; }
    /// <summary>
    /// 初期化処理
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
