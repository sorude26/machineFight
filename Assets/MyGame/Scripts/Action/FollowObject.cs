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
    private void Start()
    {
        if (_followTarget == null) 
        { 
            _followTarget = transform;
        }
        if (_rotationTarget == null) 
        {
            _rotationTarget = transform;
        }
    }
    private void FixedUpdate()
    {
        RotationControl();
        MoveControl();
    }
    protected void RotationControl()
    {
        transform.forward = Vector3.Lerp(transform.forward, _rotationTarget.forward, _rotationSpeed * Time.fixedDeltaTime);
    }
    protected void MoveControl()
    {
        transform.position = Vector3.Lerp(transform.position, _followTarget.position, _followSpeed * Time.fixedDeltaTime);
    }
}
