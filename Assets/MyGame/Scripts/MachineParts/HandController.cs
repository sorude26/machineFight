using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour, IPartsModel
{
    private const float ATTACK_ANGLE = 0.999f;
    [SerializeField]
    private int _id = default;
    [SerializeField]
    private Transform _shoulder = default;
    [SerializeField]
    private Transform _arm = default;
    [SerializeField]
    private Transform _aimShoulder = default;
    [SerializeField]
    private Transform _aimArm = default;
    [SerializeField]
    private Transform _lockAim = default;
    [SerializeField]
    private Transform _grip = default;
    [SerializeField]
    private WeaponBase _weapon = default;
    [SerializeField]
    private Animator _handAnime = default;
    [SerializeField]
    private bool _anSetWeapon = false;
    [SerializeField]
    private BoosterController _shoulderBoosters = default;
    [SerializeField]
    private PartsColorChanger _partsColorChanger = default;
    private float _changeTime = 0.1f;
    private string _reloadName = "Reload";
    private string _gholdName = "HandWeaponAim";
    private string _sholdName = "SaberHold";
    private string _pholdName = "KnuckleHold";
    private string _attackName = "SaberAttack";
    private string _pAttackName = "Punch";
    private bool _isMeleeAttack = false;
    private bool _isReload = false;
    private bool _firstShot = false;
    private bool _isShooting = false;
    private Vector3 _targetCurrent = default;
    private Vector3 _targetBefore = default;
    private Vector3 _targetTwoBefore = default;
    private Quaternion _topRotaion = Quaternion.identity;
    private Quaternion _handRotaion = Quaternion.identity;
    public float PartsRotaionSpeed = 6.0f;
    public float ReloadSpeed = 1f;
    public float ReloadTime = 1f;
    public Transform TargetTrans = default;
    public int ID { get => _id; }
    public bool IsAttack { get; private set; }
    public WeaponBase WeaponBase { get => _weapon; }
    public BoosterController ShoulderBoost { get => _shoulderBoosters; }
    public WeaponParam WeaponParam { get; set; }
    public void InitializeHand()
    {
        if (_anSetWeapon == true)
        {
            _weapon.SetParam(WeaponParam);
            _weapon.Initialize();
        }
    }
    public void SetCameraAim()
    {
        _topRotaion = _lockAim.localRotation;
        _handRotaion = Quaternion.identity;
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
        if (_isMeleeAttack == true) { return; }
        if (_firstShot == false)
        {
            _targetBefore = targetPos;
            _targetTwoBefore = targetPos;
            _firstShot = true;
        }
        LockOn(targetPos);
    }
    public void SetWeapon(WeaponBase weapon)
    {
        if (_anSetWeapon == true)
        {
            weapon.gameObject.SetActive(false);
        }
        else
        {
            weapon.transform.SetParent(_grip);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
            _weapon = weapon;
        }
        _weapon.Initialize();
        SetHoldAnim();
    }
    public void ChangeColor(int id)
    {
        if (_partsColorChanger != null)
        {
            _partsColorChanger.ChangeColor(id);
        }
    }
    public void SetLockAim(Transform target)
    {
        _lockAim = target;
    }
    public void StartShot(Transform target = null)
    {
        if (_weapon.IsWait == true)
        {
            if (_isReload == false)
            {
                _handAnime.Play(_reloadName);
                _isReload = true;
                StartCoroutine(ReloadWait());
            }
            return;
        }
        IsAttack = true;
        if (_isShooting == true)
        {
            return;
        }
        _isShooting = true;
        StartCoroutine(AttackImpl(target));
    }
    private void ReloadWeapon()
    {
        _weapon.Reload();
        _isReload = false;
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
    private IEnumerator AttackImpl(Transform target)
    {
        while (IsAttack == true || _weapon.IsFire == true)
        {
            if (IsAttack == true && _weapon.IsFire == false && ChackAngle())
            {
                _weapon.Fire(target);
                IsAttack = false;
            }
            yield return null;
        }
        _isShooting = false;
        EndShot();
    }
    private IEnumerator ReloadWait()
    {
        float timer = ReloadTime;
        while (timer > 0)
        {
            timer -= ReloadSpeed * Time.deltaTime;
            yield return null;
        }
        ReloadWeapon();
    }
    private void SetHoldAnim()
    {
        switch (_weapon.Type)
        {
            case WeaponType.HandGun:
                _handAnime.CrossFadeInFixedTime(_gholdName, _changeTime);
                break;
            case WeaponType.HandSaber:
                _handAnime.CrossFadeInFixedTime(_sholdName, _changeTime);
                break;
            case WeaponType.Knuckle:
                _handAnime.CrossFadeInFixedTime(_pholdName, _changeTime);
                break;
            default:
                break;
        }
    }
    public void MeleeAttack()
    {
        if (IsAttack == true) { return; }
        switch (_weapon.Type)
        {
            case WeaponType.HandGun:
                _handAnime.CrossFadeInFixedTime(_pAttackName, _changeTime);
                break;
            case WeaponType.HandSaber:
                _handAnime.CrossFadeInFixedTime(_attackName, _changeTime);
                break;
            case WeaponType.Knuckle:
                _handAnime.CrossFadeInFixedTime(_pAttackName, _changeTime);
                break;
            default:
                break;
        }
        _weapon.Fire();
        IsAttack = true;
        _isMeleeAttack = true;
        StartCoroutine(MeleeAttackImpl());
    }
    private IEnumerator MeleeAttackImpl()
    {
        ResetAngle();
        float timer = ReloadTime;
        while (timer > 0)
        {
            timer -= ReloadSpeed * Time.deltaTime;
            yield return null;
        }
        IsAttack = false;
        _isMeleeAttack = false;
    }
}
