using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleWeapon : WeaponBase
{
    [SerializeField]
    private WeaponBase[] _weapons = default;
    [SerializeField]
    private int _maxShotCount = -1;
    private int _shotCount = 0;
    public override void Initialize()
    {
        foreach (var weapon in _weapons)
        {
            weapon?.Initialize();
        }
    }
    public override void Fire()
    {
        if (_maxShotCount > 0 && _shotCount >= _maxShotCount)
        {
            return;
        }
        PlayShake();
        foreach (var weapon in _weapons)
        { 
            weapon?.Fire();
        }
        if (_maxShotCount > 0)
        {
            _shotCount++;
        }
    }
}
