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
    [SerializeField]
    private GameObject _hitEffect = default;
    [SerializeField]
    private bool _penetrate = default;
    [SerializeField]
    private float _radius = 0f;
    [SerializeField]
    private ShakeParam _hitShakeParam = default;
    private float _timer = 0f;
    private float _speed = 5f;
    private int _power = 1;
    private int _exPower = 0;
    private int _exCount = 0;
    private float _exRadius = 0;
    private int _frameCount = default;
    private Vector3 _beforePos = default;
    private ExplosionBullet _explosion = new ExplosionBullet();

    private void FixedUpdate()
    {
        MoveBullet();
        HitCheck();
    }
    protected virtual void ActiveEnd()
    {
        gameObject.SetActive(false);
    }
    protected virtual void HitBullet(Vector3 hitPos)
    {
        PlayHitEffect(hitPos);
        if (_penetrate == true) { return; }
        StartCoroutine(HitActionImpl());
    }
    protected void PlayShake()
    {
        StageShakeController.PlayShake(transform.position + _hitShakeParam.Pos, _hitShakeParam.Power, _hitShakeParam.Time);
    }
    private IEnumerator HitActionImpl()
    {
        yield return _explosion.ExplosionImpl(transform.position, _exPower, _exCount, _exRadius, _hitLayer);
        ActiveEnd();
    }
    private void PlayHitEffect(Vector3 hitPos)
    {
        var effect = ObjectPoolManager.Instance.Use(_hitEffect);
        effect.transform.position = hitPos;
        effect.transform.forward = transform.forward;
        effect.gameObject.SetActive(true);
        PlayShake();
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
        if (gameObject.activeInHierarchy == false) { return; }
        _frameCount++;
        if (_frameCount < _rayFrame) { return; }
        _frameCount = 0;
        if (_radius > 0)
        {
            if (Physics.SphereCast(_beforePos, _radius, transform.forward, out RaycastHit hit, Vector3.Distance(_beforePos, transform.position), _hitLayer))
            {
                HitAction(hit);
            }
        }
        else
        {
            if (Physics.Raycast(_beforePos, transform.forward, out RaycastHit hit, Vector3.Distance(_beforePos, transform.position), _hitLayer))
            {
                HitAction(hit);
            }
        }
        _beforePos = transform.position;
    }
    private void HitAction(RaycastHit hit)
    {
        if (hit.collider.TryGetComponent(out IDamageApplicable target))
        {
            target.AddlyDamage(_power);
        }
        HitBullet(hit.point);
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
        _exPower = param.ExplosionPower;
        _exCount = param.ExplosionCount;
        _exRadius = param.Radius;
        _timer = _lifeTime;
        _frameCount = 0;
        _beforePos = transform.position;
        gameObject.SetActive(true);
    }
}
[System.Serializable]
public struct BulletParam
{
    public float Speed;
    public int Power;
    public int ExplosionPower;
    public int ExplosionCount;
    public float Radius;
    public Vector3 Dir;
    public BulletParam(Vector3 dir, float speed, int power)
    {
        Dir = dir;
        Speed = speed;
        Power = power;
        ExplosionPower = 0;
        ExplosionCount = 0;
        Radius = 0;
    }
    public BulletParam(Vector3 dir,float speed,int power,int exPower,int count,float radius)
    {
        Dir = dir;
        Speed = speed;
        Power = power;
        ExplosionPower = exPower;
        ExplosionCount = count;
        Radius = radius;
    }
}
