using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IPartsModel
{
    [SerializeField]
    private int _id = default;
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
    public int MaxAmmunitionCapacity => _maxAmmunitionCapacity;
    public int MagazineCount => _magazineCount;
    public int _currentAmmunition = default;
    public int _currentMagazine = default;
    public int _count = 0;
    public bool IsFire;
    public bool IsWait;
    public int ID { get => _id; }
    public float Speed { get => _speed; }
    protected void PlayShake()
    {
        StageShakeController.PlayShake(transform.position + _fireShakeParam.Pos, _fireShakeParam.Power, _fireShakeParam.Time);
    }
    public abstract void Initialize();
    public abstract void Fire();
    public virtual void Reload() { }
}
