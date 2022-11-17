using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LockOnTarget : MonoBehaviour
{
    [Tooltip("ロックオンされるまでの時間")]
    [SerializeField]
    private float _lockOnTime = 1;
    [SerializeField]
    private DamageChecker _damageChecker = default;
    private float _timer = 0;
    /// <summary> ロックオン中フラグ </summary>
    public bool IsLockOn { get; private set; }
    public DamageChecker DamageChecker { get => _damageChecker; }
    private bool _isActive = true;
    private bool _isInitialized = false;

    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        if (_isInitialized == false)
        {
            LockOnController.Instance.LockOnTargets.Add(this);
            _isInitialized = true;
        }
        _isActive = true;
    }
    private void OnDisable()
    {
        IsLockOn = false;
        _isActive = false;
    }
    /// <summary>
    /// 設定時間を超えるとロックオン
    /// </summary>
    /// <param name="lockSpeed"></param>
    public void SetLockOn(float lockSpeed = 1)
    {
        if (IsLockOn == true || _isActive == false)
        {
            return;
        }
        _timer += lockSpeed * Time.fixedDeltaTime;
        if (_timer >= _lockOnTime)
        {
            IsLockOn = true;
        }
    }
    /// <summary>
    /// ロックオン解除
    /// </summary>
    public void LiftLockOn()
    {
        IsLockOn = false;
        _timer = 0;
    }
    public void SetChecker(DamageChecker checker)
    {
        _damageChecker = checker;
    }
}
