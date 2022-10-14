using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandController : MonoBehaviour
{
    private const float ATTACK_ANGLE = 0.999f;
    [SerializeField]
    private Transform _joint = default;
    [SerializeField]
    private Transform _shoulder = default;
    [SerializeField]
    private Transform _arm = default;
    [SerializeField]
    private Transform _aimShoulder = default;
    [SerializeField]
    private Transform _aimArm = default;
    [SerializeField]
    private TestWeapon _weapon = default;
    private bool _firstShot = false;
    private bool _isShooting = false;
    private Vector3 _targetCurrent = default;
    private Vector3 _targetBefore = default;
    private Vector3 _targetTwoBefore = default;
    private Quaternion _topRotaion = Quaternion.identity;
    private Quaternion _handRotaion = Quaternion.identity;
    public float PartsRotaionSpeed = 3.0f;
    public bool IsAttack;

    public Transform TargetTrans = default;
    private void FixedUpdate()
    {
        transform.position = _joint.position;
        transform.rotation = _joint.rotation;
    }
    private void LockOn(Vector3 targetPos)
    {
        _targetCurrent = ShotPrediction.Circle(_arm.position, targetPos, _targetBefore, _targetTwoBefore, _weapon.Speed);
        _aimShoulder.LookAt(_targetCurrent);
        _aimArm.LookAt(_targetCurrent);
        Quaternion topR = _aimShoulder.localRotation;
        topR.y = 0;
        _topRotaion = topR;
        _handRotaion = _aimArm.localRotation;
        _targetTwoBefore = _targetBefore;
        _targetBefore = targetPos;
    }
    public void ResetAngle()
    {
        _topRotaion = Quaternion.identity;
        _handRotaion = Quaternion.identity;
    }
    private bool ChackAngle()
    {
        var angle = Quaternion.Dot(_handRotaion, _arm.localRotation);
        return angle > ATTACK_ANGLE || angle < -ATTACK_ANGLE;
    }
    public void SetLockOn(Vector3 targetPos)
    {
        if (_firstShot == false)
        {
            _targetBefore = targetPos;
            _targetTwoBefore = targetPos;
            _firstShot = true;
        }
        LockOn(targetPos);
    }
    public void StartShot()
    {
        IsAttack = true;
        if (_isShooting == true)
        {
            return;
        }
        _isShooting = true;
        StartCoroutine(AttackImpl());
    }
    public void EndShot()
    {
        _firstShot = false;
        ResetAngle();
    }
    public void PartsMotion()
    {
        _shoulder.localRotation = Quaternion.Lerp(_shoulder.localRotation, _topRotaion, PartsRotaionSpeed * Time.fixedDeltaTime);
        _arm.localRotation = Quaternion.Lerp(_arm.localRotation, _handRotaion, PartsRotaionSpeed * Time.fixedDeltaTime);
    }
    private IEnumerator AttackImpl()
    {
        while (IsAttack == true || _weapon.IsShooting == true)
        {
            if (IsAttack == true && _weapon.IsShooting == false && ChackAngle())
            {
                _weapon.Fire();
                IsAttack = false;
            }
            yield return null;
        }
        _isShooting = false;
        EndShot();
    }
}
