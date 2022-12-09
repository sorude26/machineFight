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
    private float _timer = 0;
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
        foreach (var hit in Physics.OverlapSphere(transform.position, _radius, _hitLayer))
        {
            HitAction(hit);
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
    public void SetPower(int power)
    {
        _power = power;
    }
    public void OnBlade()
    {
        _timer = _onBladeTime;
        _onEffect.Play();
    }
}
