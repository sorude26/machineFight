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
        //VR���[�h�ł͖ڂ̈ʒu�ɃA�[�f�B�I���X�i�[�����邽�߁A���C���J�����̃��X�i�[�͔�A�N�e�B�u������
        if (OVRManager.isHmdPresent || PlayerVrCockpit.Instance.IsDebagVR)
        {
            GetComponent<AudioListener>().enabled = false;
        }
    }
}
