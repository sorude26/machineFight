using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationWeapon : WeaponBase
{
    [SerializeField]
    private WeaponBase _weapon = default;
    [SerializeField]
    private Animator _attackAnime = default;
    [SerializeField]
    private string _attackName = default;
    private Transform _target = default;

    public override void Fire(Transform target = null)
    {
        _target = target;
        _attackAnime.Play(_attackName);
    }

    public override void Initialize()
    {
        if (_weapon != null)
        {
            _weapon.Initialize();
        }
    }
    private void AnimatorEventFire()
    {
        if (_weapon != null)
        {
            _weapon.Fire(_target);
        }
        _weaponFireEvent?.Invoke();
    }
}
