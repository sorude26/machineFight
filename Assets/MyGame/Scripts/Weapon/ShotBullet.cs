using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotBullet : MonoBehaviour
{
    [SerializeField]
    private float _lifeTime = 1f;
    [SerializeField]
    private int _rayFrame = 5;
    [SerializeField]
    private LayerMask _hitLayer = default;
    private float _timer = 0f;
    private float _speed = 5f;
    private int _power = 1;
    private int _frameCount = default;
    private Vector3 _beforePos = default;
    private void FixedUpdate()
    {
        MoveBullet();
        HitCheck();
    }
    protected virtual void ActiveEnd()
    {
        gameObject.SetActive(false);
    }
    protected virtual void HitBullet()
    {
        ActiveEnd();
    }
    private void MoveBullet()
    {
        transform.position += transform.forward * _speed * Time.fixedDeltaTime;
        _timer -= Time.fixedDeltaTime;
        if (_timer <= 0)
        {
            ActiveEnd();
        }
    }
    private void HitCheck()
    {
        _frameCount++;
        if (_frameCount < _rayFrame)
        {
            return;
        }
        _frameCount = 0;
        if (Physics.Raycast(_beforePos, transform.forward, out RaycastHit hit, Vector3.Distance(_beforePos, transform.position), _hitLayer))
        {
            if (hit.collider.TryGetComponent(out IDamageApplicable target))
            {
                target.AddlyDamage(_power);
            }
            HitBullet();
        }
        _beforePos = transform.position;
    }
    public void Shot(Vector3 dir,float speed,int power)
    {
        Shot(new BulletParam(dir, speed, power));
    }
    public void Shot(BulletParam param)
    {
        transform.forward = param.Dir;
        _speed = param.Speed;
        _power = param.Power;
        _timer = _lifeTime;
        _frameCount = 0;
        _beforePos = transform.position;
    }
}
public struct BulletParam
{
    public float Speed;
    public int Power;
    public Vector3 Dir;
    public BulletParam(Vector3 dir, float speed, int power)
    {
        Dir = dir;
        Speed = speed;
        Power = power;
    }
}
