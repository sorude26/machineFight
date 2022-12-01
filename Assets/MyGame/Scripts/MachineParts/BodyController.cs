using MyGame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour, IPartsModel
{
    private const float ATTACK_ANGLE = 0.6f;
    private const float FRONT_ANGLE = 0.6f;
    [SerializeField]
    private int _id = default;
    [SerializeField]
    private BodyParam _param = default;
    [SerializeField]
    private HandController _lHand = default;
    [SerializeField]
    private HandController _rHand = default;
    [SerializeField]
    private BoosterController _boster = null;
    [SerializeField]
    private Transform _headJoint = default;
    [SerializeField]
    private Transform _backPackJoint = default;
    [SerializeField]
    private Transform _lHandJoint = default;
    [SerializeField]
    private Transform _rHandJoint = default;
    [SerializeField]
    private DamageChecker _damageChecker = default;
    [SerializeField]
    private Animator _bodyAnime = default;
    private MoveController _moveController = default;
    private BackPackController _backPack = default;
    private string _lSAttack = "SaberSlashL";
    private string _rSAttack = "SaberSlashR";
    private float _changeTime = 0.1f;
    private bool _isShoot = false;
    private bool _isInitialized = false;
    private float _jetTimer = 0;
    private List<BoosterController> _boosters = new List<BoosterController>();
    public event Action OnBodyDestroy = default;
    public Transform BodyBase = null;
    public Transform Lock = null;
    public Transform AttackTarget = null;
    public bool IsDown = false;
    public event Action UseBooster = default;
    public int ID { get => _id; }
    public Transform HeadJoint { get => _headJoint; } 
    public DamageChecker DamageChecker { get => _damageChecker; }
    public BackPackController BackPack { get => _backPack; }
    public HandController LeftHand { get => _lHand; }
    public HandController RightHand { get => _rHand; }
    public void Initialize(MoveController moveController)
    {
        _moveController = moveController;
        _boosters.Add(_boster);
        _isInitialized = true;
    }
    public void SetParam(BodyParam param)
    {
        _param = param;
        _damageChecker.SetHp(_param.Hp);
    }
    public void SetHands(HandController lhand,HandController rhand)
    {
        _lHand = lhand;
        _lHand.transform.SetParent(_lHandJoint);
        _lHand.transform.localPosition = Vector3.zero;
        _lHand.transform.localRotation = Quaternion.identity;
        _lHand.transform.localScale = new Vector3(-1, 1, 1);
        _rHand = rhand;
        _rHand.transform.SetParent(_rHandJoint);
        _rHand.transform.localPosition = Vector3.zero;
        _rHand.transform.localRotation = Quaternion.identity;
    }
    public void SetBackPack(BackPackController backPack)
    {
        backPack.transform.SetParent(_backPackJoint);
        backPack.transform.localPosition = Vector3.zero;
        backPack.transform.localRotation = Quaternion.identity;
        _boosters.Add(backPack.Booster);
        _backPack = backPack;
    }
    public void AddBooster(BoosterController booster)
    {
        _boosters.Add(booster);
    }
    public void BackPackBurst()
    {
        _backPack?.ExecuteBackPackBurst(AttackTarget);
    }
    private void FixedUpdate()
    {
        if (_isInitialized == false) { return; }
        transform.position = BodyBase.position;
        if (IsDown == true)
        {
            transform.forward = Vector3.Lerp(transform.forward, BodyBase.forward, Time.deltaTime);
            return;            
        }
        transform.forward = Lock.forward;
    }
    public void ExecuteFixedUpdate(bool isFall)
    {
        _lHand?.PartsMotion();
        _rHand?.PartsMotion();
        _backPack.ExecuteFixedUpdate(AttackTarget);
        if (AttackTarget != null && IsDown == false)
        {
            Vector3 targetDir = AttackTarget.position - transform.position;
            targetDir.y = 0.0f;
            if (ChackAngle(targetDir))
            {
                _lHand.SetLockOn(AttackTarget.position);
                _rHand.SetLockOn(AttackTarget.position);
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
        else if(IsDown == false)
        {
            _lHand?.SetCameraAim();
            _rHand?.SetCameraAim();
        }
        if (_boster != null && _boster.IsBoost == true)
        {
            if (isFall == false || IsDown == true)
            {
                StopBooster();
            }
        }
        if (_jetTimer > 0)
        {
            _jetTimer -= Time.fixedDeltaTime;
        }
    }
    private bool ChackAngle(Vector3 targetDir)
    {
        var angle = Vector3.Dot(targetDir.normalized, transform.forward);
        return angle > ATTACK_ANGLE;
    }
    public void BoostMove(Vector3 dir,bool floatMode)
    {
        if (IsDown == true)
        {
            return;
        }
        _moveController.MoveDecelerate();
        if (_boster != null && _boster.IsBoost == false)
        {
            StartJetBoosters();
        }
        if (_jetTimer > 0)
        {
            return;
        }
        if (dir != Vector3.zero)
        {
            dir = dir.x * Lock.right + dir.z * Lock.forward;
            dir = dir.normalized * _param.BoostMoveSpeed;
        }
        else
        {
            dir = Vector3.up * _param.BoostUpPower;
        }
        if (floatMode == true && dir.x != 0 && dir.z != 0)
        {
            dir *= 2f;
            _moveController.VelocityMove(dir);
        }
        else
        {
            _moveController.AddMove(dir);
        }
    }
    public void UpBoost()
    {
        if (IsDown == true)
        {
            return;
        }
        UseBooster?.Invoke();
        if (_boster != null)
        {
            if (_boster.IsBoost == false)
            {
                StartJetBoosters();
            }
            StartMainBooster();
        }
        _moveController.AddImpulse(Vector3.up * _param.UpPower);

    }
    public void AngleBoost(Vector3 dir, bool isFall,bool isFloat)
    {
        if (isFall == false || IsDown == true)
        {
            return;
        }
        UseBooster?.Invoke();
        _jetTimer = _param.JetTime;
        if (_boster != null && _boster.IsBoost == false)
        {
            StartJetBoosters();
        }
        if (dir.x > 0 && Mathf.Abs(dir.z) <= FRONT_ANGLE)
        {
            StartLeftBooster();
        }
        else if (dir.x < 0 && Mathf.Abs(dir.z) <= FRONT_ANGLE)
        {
            StartRightBooster();
        }
        else if (dir.z > 0)
        {
            StartJetBoosters();
            StartLeftBooster();
            StartRightBooster();
        }
        else
        {
            StartBackBooster();
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
        if (isFloat == true)
        {
            dir *= 2f;
        }
        _moveController.VelocityMove(dir);
    }
    private void StartJetBoosters()
    {
        foreach (var booster in _boosters)
        {
            booster.StartBooster();
        }
        _lHand.ShoulderBoost.StartBooster();
        _rHand.ShoulderBoost.StartBooster();
    }
    private void StartMainBooster()
    {
        foreach (var booster in _boosters)
        {
            booster.MainBoost();
        }
        _lHand.ShoulderBoost.MainBoost();
        _rHand.ShoulderBoost.MainBoost();
    }
    private void StartBackBooster()
    {
        foreach (var booster in _boosters)
        {
            booster.BackBoost();
        }
        _lHand.ShoulderBoost.BackBoost();
        _rHand.ShoulderBoost.BackBoost();
    }
    private void StartLeftBooster()
    {
        foreach (var booster in _boosters)
        {
            booster.LeftBoost();
        }
        _lHand.ShoulderBoost.LeftBoost();
    }
    private void StartRightBooster()
    {
        foreach (var booster in _boosters)
        {
            booster.RightBoost();
        }
        _rHand.ShoulderBoost.RightBoost();
    }
    private void StopBooster()
    {
        foreach (var booster in _boosters)
        {
            booster.StopBooster();
        }
        _lHand.ShoulderBoost.StopBooster();
        _rHand.ShoulderBoost.StopBooster();
    }
    public void ShotLeft()
    {
        if (_lHand.WeaponBase.Type == WeaponType.HandSaber)
        {
            if (_lHand.IsAttack == false)
            {
                _bodyAnime.CrossFadeInFixedTime(_lSAttack, _changeTime);
                _lHand.MeleeAttack();
            }
        }
        else
        {
            _lHand.StartShot(AttackTarget);
        }
    }
    public void ShotRight()
    {
        if (_rHand.WeaponBase.Type == WeaponType.HandSaber)
        {
            if (_rHand.IsAttack == false)
            {
                _bodyAnime.CrossFadeInFixedTime(_rSAttack, _changeTime);
                _rHand.MeleeAttack();
            }
        }
        else
        {
            _rHand.StartShot(AttackTarget);
        }
    }
    public void DestroyBody()
    {
        OnBodyDestroy?.Invoke();
        StopBooster();
    }
}

[Serializable]
public struct BodyParam
{
    public int Hp;
    public float BoostMoveSpeed;
    public float BoostUpPower;
    public float UseGeneratorPower;
    public float EnergyConsumption;
    public float UpPower;
    public float JetPower;
    public float JetTime;
}