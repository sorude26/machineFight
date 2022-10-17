using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LockOnTarget : MonoBehaviour
{
    [Tooltip("���b�N�I�������܂ł̎���")]
    [SerializeField]
    private float _lockOnTime = 1;
    private float _timer = 0;
    /// <summary> ���b�N�I�����t���O </summary>
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
    /// �ݒ莞�Ԃ𒴂���ƃ��b�N�I��
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
    /// ���b�N�I������
    /// </summary>
    public void LiftLockOn()
    {
        IsLockOn = false;
        _timer = 0;
    }
}
