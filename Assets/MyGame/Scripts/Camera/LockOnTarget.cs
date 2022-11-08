using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LockOnTarget : MonoBehaviour
{
    [Tooltip("���b�N�I�������܂ł̎���")]
    [SerializeField]
    private float _lockOnTime = 1;
    [SerializeField]
    private DamageChecker _damageChecker = default;
    private float _timer = 0;
    /// <summary> ���b�N�I�����t���O </summary>
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
    /// �ݒ莞�Ԃ𒴂���ƃ��b�N�I��
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
    /// ���b�N�I������
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
