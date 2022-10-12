using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandController : MonoBehaviour
{
    private const float ATTACK_ANGLE = 0.999f;
    [SerializeField]
    private Transform _shoulder = default;
    [SerializeField]
    private Transform _arm = default;
    [SerializeField]
    private Transform _aimShoulder = default;
    [SerializeField]
    private Transform _aimArm = default;
    [SerializeField]
    private TestWeapon _weapon;
    private bool _firstShot = false;
    private Vector3 _targetCurrent = default;
    private Vector3 _targetBefore = default;
    private Vector3 _targetTwoBefore = default;
    private Quaternion _topRotaion = Quaternion.identity;
    private Quaternion _handRotaion = Quaternion.identity;
    public float PartsRotaionSpeed = 3.0f;

    public Transform TargetTrans = default;
    private void LockOn(Vector3 targetPos)
    {
        _targetCurrent = ShotPrediction.Circle(_arm.position, targetPos, _targetBefore, _targetTwoBefore, _weapon.Speed);
        _aimShoulder.LookAt(_targetCurrent);
        _aimArm.LookAt(_targetCurrent);
        _targetTwoBefore = _targetBefore;
        _targetBefore = targetPos;
    }
    private void ResetAngle()
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
        bool isAttack = true;
        while (isAttack == true || _weapon.IsShooting == true)
        {
            if (ChackAngle())
            {
                _weapon.Fire();
                isAttack = false;
                yield return null;
            }
            yield return null;
        }
    }
}
