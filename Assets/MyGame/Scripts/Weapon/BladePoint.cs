using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladePoint : MonoBehaviour
{
    [SerializeField]
    private int _power = 1;
    [SerializeField]
    private int _rayFrame = 1;
    [SerializeField]
    private float _radius = 1f;
    [SerializeField]
    private LayerMask _hitLayer = default;
    [SerializeField]
    private GameObject _hitEffect = default;
    private Vector3 _beforePos = default;
    private int _frameCount = default;
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
        HitBlade(hit.point);
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
