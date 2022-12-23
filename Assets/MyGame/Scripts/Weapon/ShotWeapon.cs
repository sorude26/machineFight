using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotWeapon : WeaponBase
{
    [SerializeField]
    private ShotBullet _bullet = default;
    [SerializeField]
    private ParticleSystem _muzzleFlashEffect = default;
    [SerializeField]
    private Transform _muzzle = default;
    [SerializeField]
    private BulletParam _bulletParam = default;
    [SerializeField,Range(1,100)]
    private int _shotCount = 1;
    [SerializeField, Range(0, 30)]
    private int _subCount = 0;
    [SerializeField]
    protected float _triggerInterval = 0.2f;
    [SerializeField]
    protected float _shotInterval = 0.2f;
    [SerializeField]
    protected float _diffusivity = 0.01f;
    [SerializeField]
    private int _shotSEID = 3;
    [SerializeField]
    private float _shotSEVolume = 0.1f;
    [SerializeField]
    private int _setSEShotCount = -1;

    private int _seCount = 0;
   
    private bool _isTrigerOn = false;
    public override void Initialize()
    {
        if (_maxAmmunitionCapacity > 0)
        {
            _currentAmmunition = _maxAmmunitionCapacity;
        }
        if (_magazineCount > 0)
        {
            _currentMagazine = _magazineCount;
        }
    }
    protected void Shot(Transform target = null)
    {
        
        var bullet = ShotBulletPool.GetObject(_bullet);
        bullet.transform.position = _muzzle.position;
        _bulletParam.Dir = Diffusivity(_muzzle.forward);
        _bulletParam.Power = _power;
        _bulletParam.Speed = _speed;
        bullet.Shot(_bulletParam, target);
        if (_setSEShotCount >= 0)
        {
            _seCount++;
            if (_seCount > _setSEShotCount)
            {
                _seCount = 0;
                SoundManager.Instance.PlaySE(_shotSEID, bullet.gameObject, _shotSEVolume);
            }
        }
        PlayShake();        
    }
    protected Vector3 Diffusivity(Vector3 target)
    {
        if (_diffusivity > 0)
        {
            target.x += Random.Range(-_diffusivity, _diffusivity);
            target.y += Random.Range(-_diffusivity, _diffusivity);
            target.z += Random.Range(-_diffusivity, _diffusivity);
        }
        return target;
    }
    public override void Fire(Transform target)
    {
        if (IsFire == true || IsWait == true || _isTrigerOn == true || gameObject.activeInHierarchy == false)
        {
            return;
        }
        _isTrigerOn = true;
        _weaponFireEvent?.Invoke();
        StartCoroutine(TriggerWait());
        StartCoroutine(FireImpl(target));
    }
    protected IEnumerator FireImpl(Transform target = null)
    {
        IsFire = true;
        _count = 0;
        yield return WaitTime(_fireDelay);
        while (_count < _shotCount && IsFire)
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySE(_seFireID,_muzzle.position, _seFireVolume);
            }
            if (_muzzleFlashEffect != null)
            {
                _muzzleFlashEffect.Play();
            }
            Shot(target);
            if (_magazineCount >= 0)
            {
                _currentMagazine--;
                if (_currentMagazine <= 0)
                {
                    IsFire = false;
                    IsWait = true;
                }
            }
            if (_maxAmmunitionCapacity >= 0)
            {
                _currentAmmunition--;
                if (_currentAmmunition <= 0)
                {
                    IsFire = false;
                    IsWait = true;
                }
            }
            for (int i = 0; i < _subCount; i++)
            {
                Shot(target);
            }
            _count++;
            _onCount?.Invoke();
            yield return WaitTime(_shotInterval);
        }
        IsFire = false;
    }
    private IEnumerator TriggerWait()
    {
        yield return WaitTime(_triggerInterval);
        _isTrigerOn = false;
    }
    public override void Reload()
    {
        if (_currentAmmunition == 0)
        {
            return;
        }
        IsWait = false;
        if (_magazineCount > 0)
        {
            _currentMagazine = _magazineCount;
            if (_currentMagazine > _currentAmmunition)
            {
                _currentMagazine = _currentAmmunition;
            }
        }
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySE(_seReloadID, _muzzle.position, _seReloadVolume);
        }
        _onCount?.Invoke();
    }
    public override void RefillAmmunition(float percent)
    {
        if (_maxAmmunitionCapacity <= 0)
        {
            return;
        }
        int ammunition = (int)(_maxAmmunitionCapacity * percent);
        _currentAmmunition += ammunition;
        if (_currentAmmunition > _maxAmmunitionCapacity)
        {
            _currentAmmunition = _maxAmmunitionCapacity;
        }
        _onCount?.Invoke();
    }
    public override void SetParam(WeaponParam param)
    {
        _power = param.Power;
        _speed = param.Speed;
        _maxAmmunitionCapacity = param.MaxAmmunitionCapacity;
        _magazineCount = param.MagazineCount;
        _shotCount = param.ShotCount;
        _subCount = param.SubCount;
        _triggerInterval = param.TriggerInterval;
        _shotInterval = param.ShotInterval;
        _diffusivity = param.Diffusivity;
        _bulletParam.ExplosionPower = param.ExPower;
        _bulletParam.ExplosionCount = param.ExCount;
        _bulletParam.Radius = param.ExRadius;
    }
}
