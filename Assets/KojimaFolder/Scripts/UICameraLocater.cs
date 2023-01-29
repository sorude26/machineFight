using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICameraLocater : MonoBehaviour
{
    static Camera _uiCamera;
    public static Camera UICamera => _uiCamera;

    private void Awake()
    {
        _uiCamera = GetComponent<Camera>();
    }
}
