using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform _cameraTarget = default;
    [SerializeField]
    private Transform _forwardTarget = default;
    [SerializeField]
    private float _followSpeed = 5f;
    [SerializeField]
    private float _lockSpeed = 20f;
    [SerializeField]
    private float _upSpeed = 1f;
    private Quaternion _cameraRot = default;
    private Quaternion _forwardRot = default;
    [SerializeField]
    private float _minDownAngle = -30f;
    [SerializeField]
    private float _maxUpAngle = 40f;
    private void FixedUpdate()
    {
        _cameraTarget.localRotation = Quaternion.Lerp(_cameraTarget.localRotation, _cameraRot, _followSpeed * Time.fixedDeltaTime);
        _forwardTarget.localRotation = Quaternion.Lerp(_forwardTarget.localRotation, _forwardRot, _followSpeed * Time.fixedDeltaTime);
    }
    public void FreeLock(Vector2 dir)
    {
        _cameraRot = _cameraTarget.localRotation;
        _forwardRot = _forwardTarget.localRotation;
        _cameraRot *= Quaternion.Euler(0, dir.x * _lockSpeed, 0);
        _forwardRot *= Quaternion.Euler(dir.y * _upSpeed, 0, 0);
        _forwardRot = ClampRotationX(_forwardRot);
    }
    public void ResetLock()
    {
        _cameraRot = Quaternion.identity;
        _forwardRot = Quaternion.identity;
    }
    private Quaternion ClampRotationX(Quaternion angle)
    {
        angle.x /= angle.w;
        angle.y /= angle.w;
        angle.z /= angle.w;
        angle.w = 1f;
        float angleX = Mathf.Atan(angle.x) * Mathf.Rad2Deg * 2f;
        angleX = Mathf.Clamp(angleX, _minDownAngle, _maxUpAngle);
        angle.x = Mathf.Tan(angleX * Mathf.Deg2Rad * 0.5f);
        return angle;
    }
}
