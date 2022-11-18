using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LockOnTarget : MonoBehaviour
{
    [Tooltip("ロックオンされるまでの時間")]
    [SerializeField]
    private float _lockOnTime = 1;
    [SerializeField]
    private DamageChecker _damageChecker = default;
    private float _timer = 0;
    private RectTransform _targetMark = default;
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
            LockOnController.Instance.AddTarget(this);
            _isInitialized = true;
        }
        _isActive = true;
    }
    private void OnDisable()
    {
        IsLockOn = false;
        _isActive = false;
        if (_targetMark != null)
        {
            _targetMark.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 設定時間を超えるとロックオン
    /// </summary>
    /// <param name="lockSpeed"></param>
    public void SetLockOn(float lockSpeed = 1)
    {
        if (IsLockOn == true)
        {
            if (_targetMark != null)
            {
                _targetMark.position = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
            }
            return;
        }
        if (_isActive == false)
        {
            return;
        }
        _timer += lockSpeed * Time.fixedDeltaTime;
        if (_timer >= _lockOnTime)
        {
            IsLockOn = true;
            if (_targetMark != null)
            {
                _targetMark.gameObject.SetActive(true);
            }
        }
    }
    /// <summary>
    /// ロックオン解除
    /// </summary>
    public void LiftLockOn()
    {
        IsLockOn = false;
        _timer = 0;
        if (_targetMark != null)
        {
            _targetMark.gameObject.SetActive(false);
        }
    }
    public void SetChecker(DamageChecker checker)
    {
        _damageChecker = checker;
    }
    public void SetTargetMark(RectTransform markRect)
    {
        _targetMark = markRect;
    }
}
