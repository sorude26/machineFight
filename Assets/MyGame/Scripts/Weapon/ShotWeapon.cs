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
    protected float _shotInterval = 0.2f;
    [SerializeField]
    protected float _diffusivity = 0.01f;
    [SerializeField]
    protected int _exPower = 0;
    [SerializeField]
    protected int _exCount = 0;
    [SerializeField]
    protected int _exRadius = 0;
    [SerializeField]
    protected int _maxAmmunitionCapacity = -1;
    [SerializeField]
    protected int _magazineCount = -1;
    private int _currentAmmunition = default;
    private int _currentMagazine = default;
    private int _count = 0;
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
        if (IsFire == true || IsWait == true)
        {
            return;
        }
        StartCoroutine(FireImpl());
    }
    protected IEnumerator FireImpl()
    {
        IsFire = true;
        _count = 0;
        while (_count < _shotCount && _currentAmmunition >= 0 && _currentMagazine >= 0)
        {
            if (_muzzleFlashEffect != null)
            {
                _muzzleFlashEffect.Play();
            }
            Shot();
            if (_magazineCount >= 0)
            {
                _currentMagazine--;
            }
            if (_maxAmmunitionCapacity >= 0)
            {
                _currentAmmunition--;
            }
            for (int i = 0; i < _subCount; i++)
            {
                Shot();
            }
            _count++;
            yield return WaitShot();
        }
        IsFire = false;
    }
    private IEnumerator WaitShot()
    {
        float timer = 0;
        while (timer < _shotInterval)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        if (_magazineCount >= 0 && _currentMagazine <= 0)
        {
            IsWait = true;
        }
    }
    public override void Reload()
    {
        IsWait = false;
        if (_magazineCount > 0)
        {
            _currentMagazine = _magazineCount;
        }
    }
}
