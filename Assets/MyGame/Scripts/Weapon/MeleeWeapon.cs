using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : WeaponBase
{
    [SerializeField]
    private BladePoint[] _bladePoints = default;
    public override void Initialize()
    {
        foreach (var blade in _bladePoints)
        {
            blade.SetPower(_power);
        }
    }
    public override void Fire()
    {
        foreach(var blade in _bladePoints)
        {
            blade.OnBlade = true;
        }
    }
    public override void StopFire()
    {
        foreach (var blade in _bladePoints)
        {
            blade.OnBlade = false;
        }
    }
}