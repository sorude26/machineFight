using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotBullet : MonoBehaviour
{
    [SerializeField]
    protected float _lifeTime = 1f;
    [SerializeField]
    protected int _rayFrame = 5;
    [SerializeField]
    protected LayerMask _hitLayer = default;
    [SerializeField]
    protected GameObject _hitEffect = default;
    [SerializeField]
    protected bool _penetrate = default;
    [SerializeField]
    protected float _radius = 0f;
    [SerializeField]
    protected ShakeParam _hitShakeParam = default;
    [SerializeField]
    protected float _gravity = 0f;
    [SerializeField]
    protected int _seHitID = 12;
    [SerializeField]
    protected float _seHitVolume = 0.01f;
    protected float _timer = 0f;
    protected float _speed = 5f;
    protected int _power = 1;
    protected int _exPower = 0;
    protected int _exCount = 0;
    protected float _exRadius = 0;
    protected int _frameCount = default;
    protected Vector3 _beforePos = default;
    protected ExplosionBullet _explosion = new ExplosionBullet();

    private void FixedUpdate()
    {
        MoveBullet();
        HitCheck();
    }
    protected virtual void ActiveEnd()
    {
        gameObject.SetActive(false);
    }
    public void HitBullet()
    {
        HitBullet(transform.position);
    }
    protected virtual void HitBullet(Vector3 hitPos)
    {
        PlayHitEffect(hitPos);
        if (_penetrate == true || gameObject.activeInHierarchy == false) { return; }
        StartCoroutine(HitActionImpl());
    }
    protected void PlayShake()
    {
        StageShakeController.PlayShake(transform.position + _hitShakeParam.Pos, _hitShakeParam.Power, _hitShakeParam.Time);
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySE(_seHitID, transform.position, _seHitVolume);
        }
    }
    protected IEnumerator HitActionImpl()
    {
        _speed = 0f;
        yield return _explosion.ExplosionImpl(transform.position, _exPower, _exCount, _exRadius, _hitLayer);
        ActiveEnd();
    }
    protected void PlayHitEffect(Vector3 hitPos)
    {
        var effect = ObjectPoolManager.Instance.Use(_hitEffect);
        effect.transform.position = hitPos;
        effect.transform.forward = transform.forward;
        effect.gameObject.SetActive(true);
        PlayShake();
    }
    protected virtual void MoveBullet()
    {
        if(_speed <= 0) { return; }
        transform.forward = Vector3.Lerp(transform.forward, transform.forward + Vector3.down * _gravity, Time.fixedDeltaTime);
        transform.position += transform.forward * _speed * Time.fixedDeltaTime;
        _timer -= Time.fixedDeltaTime;
        if (_timer <= 0)
        {
            ActiveEnd();
        }
    }
    protected void HitCheck()
    {
        if (gameObject.activeInHierarchy == false || _speed <= 0) { return; }
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
    protected void HitAction(RaycastHit hit)
    {
        if (hit.collider.TryGetComponent(out IDamageApplicable target))
        {
            target.AddlyDamage(_power);
        }
        HitBullet(hit.point);
    }
    public virtual void Shot(BulletParam param, Transform target = null)
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
    public float HomingSpeed;
    public float HomingStartTime;
    public float HomingEndTime;
    public Vector3 Dir;
    public BulletParam(Vector3 dir,float speed,int power,int exPower,int count,float radius,float homSpeed = 0,float homSTime = 0, float homETime = 0)
    {
        Dir = dir;
        Speed = speed;
        Power = power;
        ExplosionPower = exPower;
        ExplosionCount = count;
        Radius = radius;
        HomingSpeed = homSpeed;
        HomingStartTime = homSTime;
        HomingEndTime = homETime;
    }
}
