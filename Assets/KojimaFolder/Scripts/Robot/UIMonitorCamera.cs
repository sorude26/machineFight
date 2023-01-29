using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMonitorCamera : MonoBehaviour
{
    [SerializeField]
    Camera _camera;
    private void FixedUpdate()
    {
        _camera.Render();
    }
}
