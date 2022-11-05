using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleWeapon : WeaponBase
{
    [SerializeField]
    private WeaponBase[] _weapons = default;
    [SerializeField]
    private float _triggerInterval = 0.2f;
    private float _timer = 0;
    private void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
    }
    public override void Initialize()
    {
        foreach (var weapon in _weapons)
        {
            weapon?.Initialize();
        }
        _currentAmmunition = _maxAmmunitionCapacity;
    }
    public override void Fire()
    {
        if (_maxAmmunitionCapacity > 0 && _currentAmmunition <= 0 || _timer > 0)
        {
            return;
        }
        _timer = _triggerInterval;
        PlayShake();
        foreach (var weapon in _weapons)
        { 
            weapon?.Fire();
        }
        if (_currentAmmunition > 0)
        {
            _currentAmmunition--;
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
}
