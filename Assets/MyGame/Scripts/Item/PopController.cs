using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PopController : MonoBehaviour
{
    protected float _diffusivity = 0.1f;
    public abstract void PopItem();
    protected Vector3 Diffusivity(Vector3 target)
    {
        if (_diffusivity > 0)
        {
            target.x += Random.Range(-_diffusivity, _diffusivity);
            target.y += Random.Range(-_diffusivity, _diffusivity);
            target.z += Random.Range(-_diffusivity, _diffusivity);
        }
        return target;
    }
}
