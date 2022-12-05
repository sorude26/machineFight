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
    Transform _cameraBase = default;
    [SerializeField]
    private float _curveSpeed = 1f;
    [SerializeField]
    private float _resetSpeed = 1f;
    [SerializeField]
    private float _maxCurve = 30f;
    [SerializeField]
    private float _minDis = 5f;
    [SerializeField]
    Vector3 _normalPos = default;
    [SerializeField]
    Vector3 _changePos = default;
    
    private void LateUpdate()
    {
        if (_noneLerp == false)
        {
            return;
        }
        _camera.localPosition = _changePos;
        _cameraBase.localRotation = Quaternion.identity;
        transform.forward = _lookRotationTarget.forward;
        transform.position = _lookTarget.position;
    }
    private void FixedUpdate()
    {
        if (_noneLerp == true)
        {
            return;
        }
        _camera.localPosition = _normalPos;
        transform.forward = Vector3.Lerp(transform.forward, _rotationTarget.forward, _rotationSpeed * Time.fixedDeltaTime);
        float speed = (transform.position - _followTarget.position).sqrMagnitude;
        transform.position = Vector3.Lerp(transform.position, _followTarget.position, speed * _followSpeed * Time.fixedDeltaTime);
        var target =Vector3.Cross(_followTarget.position - transform.position ,transform.forward);
        if (target.y < -_minDis)
        {
            _cameraBase.localRotation = Quaternion.Lerp(_cameraBase.localRotation, Quaternion.Euler(0,0,_maxCurve), _curveSpeed * Time.fixedDeltaTime);
        }
        else if (target.y > _minDis)
        {
            _cameraBase.localRotation = Quaternion.Lerp(_cameraBase.localRotation, Quaternion.Euler(0, 0, -_maxCurve), _curveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            _cameraBase.localRotation = Quaternion.Lerp(_cameraBase.localRotation,Quaternion.identity, _resetSpeed * Time.fixedDeltaTime);
        }
    }
    public void ChangeMode()
    {
        _noneLerp = !_noneLerp;
    }
}
