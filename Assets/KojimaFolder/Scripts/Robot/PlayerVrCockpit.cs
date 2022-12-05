using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVrCockpit : MonoBehaviour
{
    private static PlayerVrCockpit _instance;
    public static PlayerVrCockpit Instance => _instance;

    private void Awake()
    {
        _instance = this;
    }
}
