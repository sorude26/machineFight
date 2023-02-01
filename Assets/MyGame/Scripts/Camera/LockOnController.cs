using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���b�N�I���@�\�𑀍삷��
public class LockOnController : MonoBehaviour
{
    public static LockOnController Instance { get; private set; }
    /// <summary> ���b�N�I���͈� </summary>
    public const float LOCK_ON_ANGLE = 0.9f;
    [Tooltip("�J�����ʒu")]
    [SerializeField]
    private Transform _cameraTrans = default;
    [Tooltip("��Q���C���[")]
    [SerializeField]
    private LayerMask _wallLayer = default;
    [SerializeField]
    private RectTransform _markCanvas = default;
    [SerializeField]
    private GameObject _inRangeMarkPrefab = default;
    /// <summary> ���b�N�I�����x </summary>
    public float LockOnSpeed { get; set; } = 2;
    /// <summary> ���b�N�I������ </summary>
    public float LockOnRange { get; set; } = 500;
    /// <summary> ���b�N�I���Ώ� </summary>
    private List<LockOnTarget> _lockOnTargets = new List<LockOnTarget>();
    public IReadOnlyCollection<LockOnTarget> LockOnTargets => _lockOnTargets;
    private int _targetNum = 0;
    private void Awake()
    {
        Instance = this;
    }
    private void FixedUpdate()
    {
        foreach (var target in _lockOnTargets)
        {
            LockOnCheck(target);
        }
    }
    public void AddTarget(LockOnTarget target)
    {
        var mark = Instantiate(_inRangeMarkPrefab, _markCanvas);
        mark.gameObject.SetActive(false);
        target.SetTargetMark(mark.GetComponent<RectTransform>());
        _lockOnTargets.Add(target);
    }
    /// <summary>
    /// �^�[�Q�b�g��Ԃ�
    /// </summary>
    /// <returns></returns>
    public LockOnTarget GetTarget()
    {
        if (_lockOnTargets[_targetNum].IsLockOn)
        {
            return _lockOnTargets[_targetNum];
        }
        for (int i = 0; i < _lockOnTargets.Count; i++)
        {
            ChangeTargetNum();
            if (_lockOnTargets[_targetNum].IsLockOn)
            {
                return _lockOnTargets[_targetNum];
            }
        }
        return null;
    }
    /// <summary>
    /// �Ώېؑւ����s��
    /// </summary>
    public void ChangeTargetNum()
    {
        _targetNum++;
        if (_targetNum >= _lockOnTargets.Count)
        {
            _targetNum = 0;
        }
    }
    /// <summary>
    /// �Ώۂ����b�N�I���͈͂����肷��
    /// </summary>
    /// <param name="target"></param>
    private void LockOnCheck(LockOnTarget target)
    {
        if (Vector3.Distance(target.transform.position,_cameraTrans.position) > LockOnRange)
        {
            if (target.IsLockOn == true)
            {
                target.LiftLockOn();
            }
            return;
        }
        Vector3 targetDir = target.transform.position - _cameraTrans.position;
        float range = Vector3.Distance(_cameraTrans.position, target.transform.position);
        if (ChackAngle(targetDir) && !Physics.Raycast(_cameraTrans.position, targetDir, range, _wallLayer))
        {
            target.SetLockOn(LockOnSpeed * Vector3.Dot(targetDir.normalized, _cameraTrans.forward));
        }
        else if (target.IsLockOn == true)
        {
            target.LiftLockOn();
        }
    }
    /// <summary>
    /// �͈͓�������s��
    /// </summary>
    /// <param name="targetDir"></param>
    /// <returns></returns>
    private bool ChackAngle(Vector3 targetDir)
    {
        var angle = Vector3.Dot(targetDir.normalized, _cameraTrans.forward);
        return angle > LOCK_ON_ANGLE;
    }
}