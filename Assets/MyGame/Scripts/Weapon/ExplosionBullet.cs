using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBullet
{
    private const float INTERVAL_TIME = 0.1f;
    private readonly WaitForSeconds WAIT_SECONDS = new WaitForSeconds(INTERVAL_TIME);
    public IEnumerator ExplosionImpl(Vector3 center, int damage,int count,float radius,LayerMask layer)
    {
        for (int i = 0; i < count; i++)
        {
            Explosion(center, damage, radius,layer);
            yield return WAIT_SECONDS;
        }
    }
    private void Explosion(Vector3 center,int damage, float radius,LayerMask layer)
    {
        var cols = Physics.OverlapSphere(center, radius,layer);
        foreach (var col in cols)
        {
            if (col.TryGetComponent<IDamageApplicable>(out var hit))
            {
                hit.AddlyDamage(damage);
            }
        }
    }
}
