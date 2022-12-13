using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class WeaponBase : MonoBehaviour, IPartsModel
{
    [SerializeField]
    private int _id = default;
    [SerializeField]
    protected WeaponType _weaponType = default;
    [SerializeField]
    protected int _power = 10;
    [SerializeField]
    protected float _speed = 200f;
    [SerializeField]
    protected ShakeParam _fireShakeParam = default;
    [SerializeField]
    protected int _maxAmmunitionCapacity = -1;
    [SerializeField]
    protected int _magazineCount = -1;
    [SerializeField]
    private PartsColorChanger _partsColorChanger = default;
    [SerializeField]
    protected UnityEvent _weaponFireEvent = default;
    [SerializeField]
    protected int _seFireID = 3;
    [SerializeField]
    protected float _seFireVolume = 0.2f;
    [SerializeField]
    protected int _seReloadID = 32;
    [SerializeField]
    protected float _seReloadVolume = 0.2f;
    protected Action _onCount = default;
    public int MaxAmmunitionCapacity => _maxAmmunitionCapacity;
    public int MagazineCount => _magazineCount;
    public int _currentAmmunition = default;
    public int _currentMagazine = default;
    public int _count = 0;
    public bool IsFire;
    public bool IsWait;
    public event Action OnCount 
    { 
        remove 
        { 
            _onCount -= value;
        }
        add 
        { 
            _onCount += value; 
        } 
    }
    public int ID { get => _id; }
    public float Speed { get => _speed; }
    public WeaponType Type { get => _weaponType; }
    protected void PlayShake()
    {
        StageShakeController.PlayShake(transform.position + _fireShakeParam.Pos, _fireShakeParam.Power, _fireShakeParam.Time);
    }
    public virtual void SetParam(WeaponParam param)
    {
        _power = param.Power;
        _speed = param.Speed;
        _maxAmmunitionCapacity = param.MaxAmmunitionCapacity;
        _magazineCount = param.MagazineCount;
    }
    public void ChangeColor(int id)
    {
        if (_partsColorChanger != null)
        {
            _partsColorChanger.ChangeColor(id);
        }
    }
    public abstract void Initialize();
    public virtual void Fire() { Fire(null); }
    public abstract void Fire(Transform target = null);
    public virtual void Reload() { }
    public virtual void StopFire() { }
    public virtual void RefillAmmunition(float percent) { }
    public void AnLimitAmmunition()
    {
        _maxAmmunitionCapacity = -1;
    }
}
public enum WeaponType
{
    HandGun,
    HandSaber,
    Knuckle,
}
