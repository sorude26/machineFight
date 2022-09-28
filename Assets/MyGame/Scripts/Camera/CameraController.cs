using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform _cameraTarget = default;
    [SerializeField]
    private float _followSpeed = 5f;
    [SerializeField]
    private float _lockSpeed = 20f;
    [SerializeField]
    private float _upSpeed = 1f;
    private Quaternion _cameraRot = default;
    private float _minY = -70f;
    private float _maxY = 70f;
    private float _angleY = 0;
    private float _minX = -2f;
    private float _maxX = 2f;
    private Vector2 _minInputLimit = new Vector2(0.1f, 0.5f);
    private Vector3 _startCameraPos = default;
    public Quaternion CameraRot { get => _cameraRot; }
    void Start()
    {
        _cameraRot = transform.localRotation;
        _startCameraPos = transform.localPosition;
    }
    private void FixedUpdate()
    {
        _cameraRot.x = _angleY;
        transform.localRotation = Quaternion.Lerp(transform.localRotation, _cameraRot, _followSpeed * Time.fixedDeltaTime);
    }
    public void FreeLock(Vector2 dir)
    {
        _cameraRot = _cameraTarget.localRotation;
        if (Mathf.Abs(dir.x) > _minInputLimit.x)
        {
            _cameraRot *= Quaternion.Euler(0, dir.x * _lockSpeed, 0);
        }
        if (Mathf.Abs(dir.y) > _minInputLimit.y)
        {
            _cameraRot *= Quaternion.Euler(dir.y * _upSpeed, 0, 0);
        }
        _cameraRot = ClampRotation(_cameraRot, _minY, _maxY);
        _angleY = _cameraRot.x;
    }
    public void ResetLock()
    {
        _angleY = 0;
        _cameraRot = _cameraTarget.localRotation;
    }
    private Quaternion ClampRotation(Quaternion angle, float minY, float maxY)
    {
        angle.x /= angle.w;
        angle.y /= angle.w;
        angle.z /= angle.w;
        angle.w = 1f;
        float angleY = Mathf.Atan(angle.y) * Mathf.Rad2Deg * 2f;
        angleY = Mathf.Clamp(angleY, minY, maxY);
        angle.y = Mathf.Tan(angleY * Mathf.Deg2Rad * 0.5f);
        float angleX = Mathf.Atan(angle.x) * Mathf.Rad2Deg * 2f;
        angleX = Mathf.Clamp(angleX, _minY, _maxY);
        angle.x = Mathf.Tan(angleX * Mathf.Deg2Rad * 0.5f);
        return angle;
    }
}
