using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBullet : ShotBullet
{
    [SerializeField]
    private Transform _targetlocker = default;
    [SerializeField]
    private float _homingSpeed = 1f;
    [SerializeField]
    private float _homingStartTime = 0f;
    [SerializeField]
    private float _homingEndTime = 5f;
    [SerializeField]
    private int _lockFrame = 0;
    private Transform _target = null;
    private float _homingTimer = 0;
    private int _lockCount = 0;
    protected override void MoveBullet()
    {
        if (_speed <= 0) { return; }
        if (_lockCount >= _lockFrame && _homingTimer < _homingEndTime && _homingTimer > _homingStartTime && _target != null && _target.gameObject.activeInHierarchy == true)
        {
            _lockCount = 0;
            _targetlocker.LookAt(_target);
            transform.forward = Vector3.Lerp(transform.forward, _targetlocker.forward, _homingSpeed * Time.fixedDeltaTime);
        }
        _lockCount++;
        transform.position += transform.forward * _speed * Time.fixedDeltaTime;
        _timer -= Time.fixedDeltaTime;
        _homingTimer += Time.fixedDeltaTime;
        if (_timer <= 0)
        {
            if (_explosionEnd == true)
            {
                HitBullet(transform.position);
                return;
            }
            ActiveEnd();
        }
    }
    public override void Shot(BulletParam param, Transform target = null)
    {
        _target = target;
        transform.forward = param.Dir;
        _speed = param.Speed;
        _power = param.Power;
        _exPower = param.ExplosionPower;
        _exCount = param.ExplosionCount;
        _exRadius = param.Radius;
        _homingSpeed = param.HomingSpeed;
        _homingStartTime = param.HomingStartTime;
        _homingEndTime = param.HomingEndTime;
        _timer = _lifeTime;
        _homingTimer = 0;
        _frameCount = 0;
        _beforePos = transform.position;
        gameObject.SetActive(true);
    }
}
