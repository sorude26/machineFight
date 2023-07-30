using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAddRenderTexture : MonoBehaviour
{
    [SerializeField] RenderTexture _renderTexture;
    Camera _camera;
    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_camera.targetTexture == null)
        {
            _camera.targetTexture = _renderTexture;
        }
    }
}
