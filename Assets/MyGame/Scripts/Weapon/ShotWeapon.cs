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
    protected int _exPower = 0;
    [SerializeField]
    protected int _exCount = 0;
    [SerializeField]
    protected int _exRadius = 0;
   
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
    protected void Shot()
    {
        var bullet = ShotBulletPool.GetObject(_bullet);
        bullet.transform.position = _muzzle.position;
        bullet.Shot(new BulletParam(Diffusivity(_muzzle.forward), _speed, _power,_exPower,_exCount,_exRadius));
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
    
    public override void Fire()
    {
        if (IsFire == true || IsWait == true || _isTrigerOn == true)
        {
            return;
        }
        _isTrigerOn = true;
        StartCoroutine(TriggerWait());
        StartCoroutine(FireImpl());
    }
    protected IEnumerator FireImpl()
    {
        IsFire = true;
        _count = 0;
        while (_count < _shotCount && IsFire)
        {
            if (_muzzleFlashEffect != null)
            {
                _muzzleFlashEffect.Play();
            }
            Shot();
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
                Shot();
            }
            _count++;
            _onCount?.Invoke();
            yield return WaitShot();
        }
        IsFire = false;
    }
    private IEnumerator TriggerWait()
    {
        float timer = 0;
        while (timer < _triggerInterval)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        _isTrigerOn = false;
    }
    private IEnumerator WaitShot()
    {
        float timer = 0;
        while (timer < _shotInterval)
        {
            timer += Time.deltaTime;
            yield return null;
        }
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
        _exPower = param.ExPower;
        _exCount = param.ExCount;
        _exRadius = param.ExRadius;
    }
}
