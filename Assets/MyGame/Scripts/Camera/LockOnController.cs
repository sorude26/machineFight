using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ロックオン機能を操作する
public class LockOnController : MonoBehaviour
{
    public static LockOnController Instance { get; private set; }
    /// <summary> ロックオン範囲 </summary>
    public const float LOCK_ON_ANGLE = 0.9f;
    [Tooltip("カメラ位置")]
    [SerializeField]
    private Transform _cameraTrans = default;
    [Tooltip("障害レイヤー")]
    [SerializeField]
    private LayerMask _wallLayer = default;
    /// <summary> ロックオン速度 </summary>
    public float LockOnSpeed { get; set; } = 2;
    /// <summary> ロックオン距離 </summary>
    public float LockOnRange { get; set; } = 500;
    /// <summary> ロックオン対象 </summary>
    public List<LockOnTarget> LockOnTargets = new List<LockOnTarget>();
    private int _targetNum = 0;
    private void Awake()
    {
        Instance = this;
    }
    private void FixedUpdate()
    {
        foreach (var target in LockOnTargets)
        {
            LockOnCheck(target);
        }
    }
    /// <summary>
    /// ターゲットを返す
    /// </summary>
    /// <returns></returns>
    public Transform GetTarget()
    {
        if (LockOnTargets[_targetNum].IsLockOn)
        {
            return LockOnTargets[_targetNum].transform;
        }
        for (int i = 0; i < LockOnTargets.Count; i++)
        {
            ChangeTargetNum();
            if (LockOnTargets[_targetNum].IsLockOn)
            {
                return LockOnTargets[_targetNum].transform;
            }
        }
        return null;
    }
    /// <summary>
    /// 対象切替えを行う
    /// </summary>
    public void ChangeTargetNum()
    {
        _targetNum++;
        if (_targetNum >= LockOnTargets.Count)
        {
            _targetNum = 0;
        }
    }
    /// <summary>
    /// 対象がロックオン範囲か判定する
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
            target.SetLockOn(LockOnSpeed);
        }
        else if (target.IsLockOn == true)
        {
            target.LiftLockOn();
        }
    }
    /// <summary>
    /// 範囲内判定を行う
    /// </summary>
    /// <param name="targetDir"></param>
    /// <returns></returns>
    private bool ChackAngle(Vector3 targetDir)
    {
        var angle = Vector3.Dot(targetDir.normalized, _cameraTrans.forward);
        return angle > LOCK_ON_ANGLE;
    }
}