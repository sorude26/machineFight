using MyGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWeapon : MonoBehaviour
{
    [SerializeField]
    private ShotBullet _bullet = default;
    [SerializeField]
    private Transform _muzzle = default;
    [SerializeField]
    private float _speed = 200f;
    [SerializeField]
    private int _power = 10;
    private void Start()
    {
        PlayerInput.SetEnterInput(InputType.Fire1, Shot);
    }
    private void Shot()
    {
        var bullet = ShotBulletPool.GetObject(_bullet);
        bullet.transform.position = _muzzle.position;
        bullet.Shot(new BulletParam(_muzzle.forward, _speed, _power));
    }
}
