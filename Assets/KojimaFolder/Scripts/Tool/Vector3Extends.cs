using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extends
{
    /// <summary>
    /// -180 Å` 180Ç≈ê≥ãKâªÇ∑ÇÈ
    /// </summary>
    public static Vector3 NomalizeRotate180(this Vector3 a)
    {
        a.x = ((a.x + 180f) % 360f) - 180f;
        if (a.x < -180)
        {
            a.x += 360f;
        }
        a.y = ((a.y + 180f) % 360f) - 180f;
        if (a.y < -180)
        {
            a.y += 360f;
        }
        a.z = ((a.z + 180f) % 360f) - 180f;
        if (a.z < -180)
        {
            a.z += 360f;
        }
        return a;
    }
    public static Vector3 NomalizeRotate360(this Vector3 a)
    {
        a.x %= 360;
        if (a.x < 0)
        {
            a.x += 360f;
        }
        a.y %= 360;
        if (a.y < 0)
        {
            a.y += 360f;
        }
        a.z %= 360;
        if (a.z < 0)
        {
            a.z += 360f;
        }
        return a;
    }
}
