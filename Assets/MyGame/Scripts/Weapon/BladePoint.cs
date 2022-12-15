using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladePoint : MonoBehaviour
{
    [SerializeField]
    private int _power = 1;
    [SerializeField]
    private float _radius = 1f;
    [SerializeField]
    private float _onBladeTime = 0.5f;
    [SerializeField]
    private LayerMask _hitLayer = default;
    [SerializeField]
    private GameObject _hitEffect = default;
    [SerializeField]
    private ParticleSystem _onEffect = default;
    [SerializeField]
    private bool _anSleepBlade = false;
    [SerializeField]
    private int _seID = 13;
    [SerializeField]
    private float _seVolume = 0.05f;
    [SerializeField]
    private int _seCount = 5;
    private float _timer = 0;
    private int _count = 0;
    private ShakeParam _shake = default;
    private void FixedUpdate()
    {
        if (_anSleepBlade == true)
        {
            HitCheck();
            return;
        }
        if (_timer <= 0)
        {
            return;
        }
        _timer -= Time.fixedDeltaTime;
        HitCheck();
    }
    private void HitCheck()
    {
        if (gameObject.activeInHierarchy == false) { return; }
        bool hitBlade = false;
        foreach (var hit in Physics.OverlapSphere(transform.position, _radius, _hitLayer))
        {
            HitAction(hit);
            hitBlade = true;
        }
        if (hitBlade == true)
        {
            _count--;
            if (_count < 0)
            {
                if (SoundManager.Instance != null)
                {
                    SoundManager.Instance.PlaySE(_seID,transform.position, _seVolume);
                }
                _shake.Pos = transform.position;
                StageShakeController.PlayShake(_shake);
                _count = _seCount;
            }
        }
    }
    private void HitAction(Collider hit)
    {
        if (hit.TryGetComponent(out IDamageApplicable target))
        {
            target.AddlyDamage(_power);
        }
        HitBlade(transform.position);
    }
    private void HitBlade(Vector3 pos)
    {
        PlayHitEffect(pos);
    }
    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
    private void PlayHitEffect(Vector3 hitPos)
    {
        var effect = ObjectPoolManager.Instance.Use(_hitEffect);
        effect.transform.position = hitPos;
        effect.transform.forward = transform.forward;
        effect.gameObject.SetActive(true);
    }
    public void SetPower(int power,ShakeParam param)
    {
        _power = power;
        _shake = param;
    }
    public void OnBlade()
    {
        _timer = _onBladeTime;
        _count = 0;
        _onEffect.Play();
    }
}
