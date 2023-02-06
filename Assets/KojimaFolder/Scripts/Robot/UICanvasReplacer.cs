using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// VR�g�p���AUI�����C�����j�^�[�ɕ\������悤�ɂ���
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
