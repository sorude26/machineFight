using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VrEyeLocater : MonoBehaviour
{
    static VrEyeLocater _instance;
    private void Awake()
    {
        _instance = this;
    }

    public static Transform GetPosition()
    {
        return _instance?.transform ?? null;
    }
}
