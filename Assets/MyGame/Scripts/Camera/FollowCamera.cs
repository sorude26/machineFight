using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField]
    Transform _followTarget = default;
    [SerializeField]
    Transform _rotationTarget = default;
    [SerializeField]
    float _followSpeed = 1f;
    [SerializeField]
    float _rotationSpeed = 1f;
    [SerializeField]
    bool _noneLerp = false;
    [SerializeField]
    Transform _lookTarget = default;
    [SerializeField]
    Transform _lookRotationTarget = default;
    [SerializeField]
    Transform _camera = default;
    [SerializeField]
    Vector3 _normalPos = default;
    [SerializeField]
    Vector3 _changePos = default;
    
    private void LateUpdate()
    {
        if (!_noneLerp)
        {
            return;
        }
        _camera.localPosition = _changePos;
        transform.forward = _lookRotationTarget.forward;
        transform.position = _lookTarget.position;
    }
    private void FixedUpdate()
    {
        if (_noneLerp)
        {
            return;
        }
        _camera.localPosition = _normalPos;
        transform.forward = Vector3.Lerp(transform.forward, _rotationTarget.forward, _rotationSpeed * Time.deltaTime);
        float speed = (transform.position - _followTarget.position).sqrMagnitude;
        transform.position = Vector3.Lerp(transform.position, _followTarget.position, speed * _followSpeed * Time.deltaTime);
    }
}
