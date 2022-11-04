using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleWeapon : WeaponBase
{
    [SerializeField]
    private WeaponBase[] _weapons = default;
   
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
        if (_maxAmmunitionCapacity > 0 && _currentAmmunition <= 0)
        {
            return;
        }
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
}
