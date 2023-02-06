using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// VR使用時、UIをメインモニターに表示するようにする
/// </summary>
public class UICanvasReplacer : MonoBehaviour
{
    private void Start()
    {
        if (OVRManager.isHmdPresent || (PlayerVrCockpit.Instance?.IsDebagVR ?? false))
        {
            var canvas = GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = UICameraLocater.UICamera ?? MainCameraLocator.MainCamera;
        }
    }
}
