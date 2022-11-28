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
    private LayerMask _hitLayer = default;
    [SerializeField]
    private GameObject _hitEffect = default;
    public bool OnBlade { get; set; }
    private void FixedUpdate()
    {
        if (OnBlade == false)
        {
            return;
        }
        HitCheck();
    }
    private void HitCheck()
    {
        if (gameObject.activeInHierarchy == false) { return; }
        foreach(var hit in Physics.OverlapSphere(transform.position, _radius, _hitLayer))
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
}
