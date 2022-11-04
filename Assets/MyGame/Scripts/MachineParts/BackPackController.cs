using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BackPackController : MonoBehaviour, IPartsModel
{
    [SerializeField]
    private int _id = default;
    [SerializeField]
    private BoosterController _booster = default;
    [SerializeField]
    private Transform _partsBase = default;
    [SerializeField]
    private Transform _lockBase = default;
    [SerializeField]
    private float _maxUpAngle = -10f;
    [SerializeField]
    private float _minDownAngle = 40f;
    [SerializeField]
    private float _maxLockAngle = 5f;
    [SerializeField]
    private float _followSpeed = 2f;
    [SerializeField]
    private UnityEvent _initializeEvent = default;
    [SerializeField]
    private UnityEvent _backPackBurstEvent = default;
    [SerializeField]
    private WeaponBase _backPackWeapon = default;
    public Transform CameraLock = default;
    public int ID { get => _id; }
    public BoosterController Booster { get => _booster; }
    public WeaponBase BackPackWeapon { get => _backPackWeapon; }
    public event Action OnBackPackBurst = default;
    private void Start()
    {
        _initializeEvent?.Invoke();
    }
    public void ExecuteFixedUpdate(Transform target)
    {
        if (target == null)
        {
            _lockBase.forward = CameraLock.forward;
        }
        else
        {
            _lockBase.LookAt(target);
        }
        var lockRotation = ClampRotation(_lockBase.localRotation);
        _partsBase.localRotation = Quaternion.Lerp(_partsBase.localRotation, lockRotation, _followSpeed * Time.fixedDeltaTime);
    }
    public void ExecuteBackPackBurst()
    {
        _backPackBurstEvent?.Invoke();
        OnBackPackBurst?.Invoke();
    }
    private Quaternion ClampRotation(Quaternion angle)
    {
        angle.x /= angle.w;
        angle.y /= angle.w;
        angle.z /= angle.w;
        angle.w = 1f;
        float angleX = Mathf.Atan(angle.x) * Mathf.Rad2Deg * 2f;
        angleX = Mathf.Clamp(angleX, _maxUpAngle, _minDownAngle);
        angle.x = Mathf.Tan(angleX * Mathf.Deg2Rad * 0.5f);
        float angleY = Mathf.Atan(angle.y) * Mathf.Rad2Deg * 2f;
        angleY = Mathf.Clamp(angleY, -_maxLockAngle, _maxLockAngle);
        angle.y = Mathf.Tan(angleY * Mathf.Deg2Rad * 0.5f);
        return angle;
    }
}
