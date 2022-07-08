using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField]
    protected Transform _followTarget = default;
    [SerializeField]
    protected Transform _rotationTarget = default;
    [SerializeField]
    protected float _followSpeed = 1f;
    [SerializeField]
    protected float _rotationSpeed = 1f;

    private void FixedUpdate()
    {
        TurnTargetDirection();
        GetCloserToTarget();
    }
    /// <summary>
    /// 目標に回転方向を合わせる
    /// </summary>
    protected void TurnTargetDirection()
    {
        if (!_followTarget) { return; }
        transform.forward = Vector3.Lerp(transform.forward, _rotationTarget.forward, _rotationSpeed * Time.fixedDeltaTime);
    }
    /// <summary>
    /// 目標に近づく
    /// </summary>
    protected void GetCloserToTarget()
    {
        if (!_rotationTarget) { return; }
        transform.position = Vector3.Lerp(transform.position, _followTarget.position, _followSpeed * Time.fixedDeltaTime);
    }
}
