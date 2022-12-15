using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : WeaponBase
{
    [SerializeField]
    private BladePoint[] _bladePoints = default;
    [SerializeField]
    private Animator _animator = default;
    [SerializeField]
    private string _attackName = default;
    public override void Initialize()
    {
        foreach (var blade in _bladePoints)
        {
            blade.SetPower(_power,_fireShakeParam);
        }
    }
    public override void Fire(Transform target)
    {
        foreach(var blade in _bladePoints)
        {
            blade.OnBlade();
        }
        if (_animator != null)
        {
            _animator.Play(_attackName);
        }
        _weaponFireEvent?.Invoke();
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySE(_seFireID,transform.position, _seFireVolume);
        }
    }
}
