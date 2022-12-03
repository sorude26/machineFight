using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extends
{
    /// <summary>
    /// -180 Å` 180Ç≈ê≥ãKâªÇ∑ÇÈ
    /// </summary>
    public static Vector3 NomalizeRotate(this Vector3 a)
    {
        a.x = ((a.x + 180f) % 360f) - 180f;
        a.y = ((a.y + 180f) % 360f) - 180f;
        a.z = ((a.z + 180f) % 360f) - 180f;
        return a;
    }
}
