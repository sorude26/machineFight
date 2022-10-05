using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.MachineFrame
{
    public class ShotWeapon : WeaponBase
    {
        [SerializeField]
        private Transform _muzzle = default;
        [SerializeField]
        private float _shotSpeed = 100;
        [SerializeField]
        private Bullet _bulletPrefab = default;
        public override void Fire()
        {
            var bullet = BulletPool.GetObject(_bulletPrefab);
            bullet.transform.position = _muzzle.position;
            bullet.Shot(_muzzle.forward * _shotSpeed);
        }
    }
}