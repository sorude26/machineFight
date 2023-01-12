using MyGame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour, IPartsModel
{
    private const float ATTACK_ANGLE = 0.6f;
    private const float FRONT_ANGLE = 0.6f;
    private const float FLOAT_SPEED_MIN = 1.5f;
    private const float FLOAT_SPEED_DESKTOP = 2.0f;
    private const float FLOAT_SPEED_MAX = 3.0f;
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
    [SerializeField]
    private PartsColorChanger _partsColorChanger = default;
    [SerializeField]
    protected int _boosterSEID = 18;
    [SerializeField]
    protected float _boosterSEVolume = 0.1f;
    private MoveController _moveController = default;
    private BackPackController _backPack = default;
    private string _lSAttack = "SaberSlashL";
    private string _rSAttack = "SaberSlashR";
    private string _lPAttack = "PunchL";
    private string _rPAttack = "PunchR";
    private float _changeTime = 0.1f;
    private bool _isShoot = false;
    private bool _isInitialized = false;
    private float _jetTimer = 0;
    private float _floatSpeed = FLOAT_SPEED_DESKTOP;
    private List<BoosterController> _boosters = new List<BoosterController>();
    public event Action OnBodyDestroy = default;
    public Transform BodyBase = null;
    public Transform Lock = null;
    public Transform AttackTarget = null;
    public bool IsDown = false;
    public event Action UseBooster = default;
    public int ID { get => _id; }
    public float FloatSpeed { get => _floatSpeed; }
    public Transform HeadJoint { get => _headJoint; }
    public DamageChecker DamageChecker { get => _damageChecker; }
    public BackPackController BackPack { get => _backPack; }
    public HandController LeftHand { get => _lHand; }
    public HandController RightHand { get => _rHand; }
    #region SetUpMethods
    public void Initialize(MoveController moveController)
    {
        _moveController = moveController;
        _boosters.Add(_boster);
        _isInitialized = true;
        if (_lHand != null)
        {
            _lHand.InitializeHand();
        }
        if (_rHand != null)
        {
            _rHand.InitializeHand();
        }
    }
    public void SetParam(BodyParam param, in PartsHandData handL = default, in PartsHandData handR = default)
    {
        _param = param;
        _damageChecker.SetHp(_param.Hp);
        if (_lHand != null)
        {
            _lHand.ReloadSpeed = handL.ReloadSpeed;
            _lHand.PartsRotaionSpeed = handL.AimSpeed;
            _lHand.WeaponParam = handL.WeaponParam;
        }
        if (_rHand != null)
        {
            _rHand.ReloadSpeed = handR.ReloadSpeed;
            _rHand.PartsRotaionSpeed = handR.AimSpeed;
            _rHand.WeaponParam = handR.WeaponParam;
        }
    }
    public void SetHands(HandController lhand, HandController rhand)
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
    public void ChangeColor(int id)
    {
        if (_partsColorChanger != null)
        {
            _partsColorChanger.ChangeColor(id);
        }
    }
    #endregion
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
        else if (IsDown == false)
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
    #region ActionMethods
    #region BoosterMethods

    /// <summary>
    /// VR用メソッド、ホバー速度を変更する
    /// </summary>
    /// <param name="value">ホバー速度(0～1で指定)</param>
    public void SetFloatSpeed(float value)
    {
        _floatSpeed = Mathf.Lerp(FLOAT_SPEED_MIN, FLOAT_SPEED_MAX, value);
    }
    public void BoostMove(Vector3 dir, bool floatMode)
    {
        if (IsDown == true)
        {
            return;
        }
        _moveController.MoveDecelerate();
        if (_boster != null && _boster.IsBoost == false)
        {
            StartJetBoosters();
            PlayBoosterSE();
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
            dir *= _floatSpeed;
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
        PlayBoosterSE();
        _moveController.AddImpulse(Vector3.up * _param.UpPower);

    }
    public void AngleBoost(Vector3 dir, bool isFall, bool isFloat)
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
            dir *= _floatSpeed;
        }
        PlayBoosterSE();
        _moveController.VelocityMove(dir);
    }
    private void PlayBoosterSE()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySE(_boosterSEID, transform.position, _boosterSEVolume);
        }
    }
    public void StartJetBoosters()
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
    #endregion
    public void BackPackBurst()
    {
        _backPack?.ExecuteBackPackBurst(AttackTarget);
    }
    public void ShotLeft()
    {
        if (_lHand.WeaponBase.Type == WeaponType.HandGun)
        {
            _lHand.StartShot(AttackTarget);
            return;
        }
        if (_lHand.WeaponBase.Type == WeaponType.HandSaber)
        {
            if (_lHand.IsAttack == false)
            {
                _bodyAnime.CrossFadeInFixedTime(_lSAttack, _changeTime);
                _lHand.MeleeAttack();
                MeleeAttackMove();
            }
        }
        else if (_lHand.WeaponBase.Type == WeaponType.Knuckle)
        {
            if (_lHand.IsAttack == false && _rHand.IsAttack == false)
            {
                _bodyAnime.CrossFadeInFixedTime(_lPAttack, _changeTime);
                _lHand.MeleeAttack();
                MeleeAttackMove();
            }
        }
    }
    public void ShotRight()
    {
        if (_rHand.WeaponBase.Type == WeaponType.HandGun)
        {
            _rHand.StartShot(AttackTarget);
            return;
        }
        if (_rHand.WeaponBase.Type == WeaponType.HandSaber)
        {
            if (_rHand.IsAttack == false)
            {
                _bodyAnime.CrossFadeInFixedTime(_rSAttack, _changeTime);
                _rHand.MeleeAttack();
                MeleeAttackMove();
            }
        }
        else if (_rHand.WeaponBase.Type == WeaponType.Knuckle)
        {
            if (_lHand.IsAttack == false && _rHand.IsAttack == false)
            {
                _bodyAnime.CrossFadeInFixedTime(_rPAttack, _changeTime);
                _rHand.MeleeAttack();
                MeleeAttackMove();
            }
        }
    }
    private void MeleeAttackMove()
    {
        StartMainBooster();
        Vector3 dir = Lock.forward;
        if (AttackTarget != null)
        {
            dir = AttackTarget.position - transform.position;
        }
        _moveController.VelocityMove(dir.normalized * _param.JetPower);
    }
    #endregion
    public void DestroyBody()
    {
        OnBodyDestroy?.Invoke();
        StopBooster();
    }
    public void StartUpBody()
    {
        BodyBase.localRotation = Quaternion.identity;
        Lock.localRotation = Quaternion.identity;
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