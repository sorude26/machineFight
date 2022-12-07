using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtends
{
    public static Vector3 GetPositionFromMe(this Transform t, Transform a)
    {
        return t.InverseTransformPoint(a.position);
    }
}
