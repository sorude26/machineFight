using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LockOnTarget : MonoBehaviour
{
    [Tooltip("ロックオンされるまでの時間")]
    [SerializeField]
    private float _lockOnTime = 1;
    private float _timer = 0;
    /// <summary> ロックオン中フラグ </summary>
    public bool IsLockOn { get; private set; }
    private void OnEnable()
    {
        if (LockOnController.Instance == null) { return; }
        LockOnController.Instance.LockOnTargets.Add(this);
    }
    private void OnDisable()
    {
        IsLockOn = false;
    }
    /// <summary>
    /// 設定時間を超えるとロックオン
    /// </summary>
    /// <param name="lockSpeed"></param>
    public void SetLockOn(float lockSpeed = 1)
    {
        if (IsLockOn == true)
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
}
