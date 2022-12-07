using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraLocator : MonoBehaviour
{
    public static Camera _mainCamera;
    public static Camera MainCamera => _mainCamera;

    private void Awake()
    {
        _mainCamera = GetComponent<Camera>();
    }
}
