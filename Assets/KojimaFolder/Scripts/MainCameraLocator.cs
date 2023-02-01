using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraLocator : MonoBehaviour
{
    static Camera _mainCamera;
    public static Camera MainCamera => _mainCamera;

    private void Awake()
    {
        _mainCamera = GetComponent<Camera>();
        
    }

    private void Start()
    {
        //VRモードでは目の位置にアーディオリスナーがあるため、メインカメラのリスナーは非アクティブ化する
        if (OVRManager.isHmdPresent || PlayerVrCockpit.Instance.IsDebagVR)
        {
            GetComponent<AudioListener>().enabled = false;
        }
    }
}
