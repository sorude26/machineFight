using MyGame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class BodyController : MonoBehaviour
{
    private const float ATTACK_ANGLE = 0.91f;
    [SerializeField]
    private BodyParam _param = default;
    [SerializeField]
    private HandController _rHand = default;
    [SerializeField]
    private HandController _lHand = default;
    [SerializeField]
    private BoosterController _boster = null;
    [SerializeField]
    private Rigidbody _moveRb = default;
    private MoveController _moveController = default;
    public Transform BodyBase = null;
    public Transform Lock = null;

    public Transform TestTarget = null;
    public bool Check = false;
    private bool _isShoot = false;
    private float _jetTimer = 0;
    private void Start()
    {
        if (_moveRb != null)
        {
            _moveController = new MoveController(_moveRb);
        }
    }
    private void FixedUpdate()
    {
        transform.position = BodyBase.position;
        transform.forward = Lock.forward;
    }
    public void ExecuteFixedUpdate(bool isFall)
    {
        _lHand?.PartsMotion();
        _rHand?.PartsMotion();

        if (TestTarget != null)
        {
            Vector3 targetDir = TestTarget.position - transform.position;
            targetDir.y = 0.0f;
            if (ChackAngle(targetDir))
            {
                _lHand.SetLockOn(TestTarget.position);
                _rHand.SetLockOn(TestTarget.position);
                _isShoot = true;
            }
            else if (_isShoot == true)
            {
                _lHand.ResetAngle();
                _rHand.ResetAngle();
            }
            else
            {
                _lHand?.SetCameraAim();
                _rHand?.SetCameraAim();
            }
        }
        else
        {
            _lHand?.SetCameraAim();
            _rHand?.SetCameraAim();
        }
        if (_boster != null && _boster.IsBoost == true && isFall == false)
        {
            _boster.StopBooster();
        }
        if (_jetTimer > 0)
        {
            _jetTimer -= Time.fixedDeltaTime;
        }
    }
    private bool ChackAngle(Vector3 targetDir)
    {
        var angle = Vector3.Dot(targetDir.normalized, transform.forward);
        if (Check)
        {
            Debug.Log(angle);
        }
        return angle > ATTACK_ANGLE;
    }
    public void BoostMove(Vector3 dir)
    {
        if (_boster != null && _boster.IsBoost == false)
        {
            _boster.StartBooster();
        }
        if (_jetTimer > 0)
        {
            return;
        }
        if (dir != Vector3.zero)
        {
            dir = dir.x * Lock.right + dir.z * Lock.forward;
            dir = dir.normalized * _param.BoostMoveSpeed;
            dir.y = _moveRb.velocity.y;
        }
        else
        {
            dir.y = _moveRb.velocity.y * _param.BoostUpPower;
        }
        _moveController.VelocityMove(dir);
    }
    public void UpBoost()
    {
        if (_boster != null)
        {
            if (_boster.IsBoost == false)
            {
                _boster.StartBooster();
            }
            _boster.MainBoost();
        }
        _moveController.VelocityMove(Vector3.up * _param.UpPower);
    }
    public void AngleBoost(Vector3 dir, bool isFall)
    {
        if (isFall == false)
        {
            return;
        }
        _jetTimer = _param.JetTime;
        if (_boster != null && _boster.IsBoost == false)
        {
            _boster.StartBooster();
        }
        if (dir.x > 0 && Mathf.Abs(dir.z) <= 0.6f)
        {
            _boster.LeftBoost();
        }
        else if (dir.x < 0 && Mathf.Abs(dir.z) <= 0.6f)
        {
            _boster.RightBoost();
        }
        else if (dir.z > 0)
        {
            _boster.MainBoost();
        }
        else
        {
            _boster.BackBoost();
        }
        if (dir != Vector3.zero)
        {
            dir = dir.x * Lock.right + dir.z * Lock.forward;
            dir = dir.normalized * _param.JetPower + Vector3.up;
        }
        else
        {
            dir.y = -_param.JetPower;
        }
        _moveController.VelocityMove(dir);
    }
    public void ShotLeft()
    {
        _lHand.StartShot();
    }
    public void ShotRight()
    {
        _rHand.StartShot();
    }
}

[Serializable]
public struct BodyParam
{
    public float BoostMoveSpeed;
    public float BoostUpPower;
    public float UpPower;
    public float JetPower;
    public float JetTime;
}