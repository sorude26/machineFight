using Oculus.Interaction.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� �R���g���[���[�����E�ʂɈ������ꍇ
//��{�^���@Two
//���{�^��  One
//�l�����w�g���K�[  PrimaryIndexTrigger
//���w�g���K�[  PrimaryHandTrigger
//�T���X�e�B�b�N��������  Button.PrimaryThumbstick
//�T���X�e�B�b�N Axis2D.PrimaryThumbstick

//����X�^�[�g�{�^�� Button.start
//�E��I�L�����X�{�^�� �Ȃ��H

public static class OculusGameInput
{
    public static bool GetPinchIn(OVRInput.Controller controller)
    {
        if (OVRInput.GetDown(OVRInput.Button.Two, controller) || OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, controller))
        {
            return OVRInput.Get(OVRInput.Button.Two, controller) && OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, controller);
        }
        return false;
    }
    public static bool GetPinchOut(OVRInput.Controller controller)
    {
        return !OVRInput.Get(OVRInput.Button.Two, controller) || !OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, controller);
    }

    public static bool GetGrabIn(OVRInput.Controller controller)
    {
        return OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, controller);
    }
    public static bool GetGrabOut(OVRInput.Controller controller)
    {
        return !OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, controller);

    }

    public static bool GetThumbIn(OVRInput.Controller controller)
    {
        return OVRInput.GetDown(OVRInput.Button.Two, controller);
    }
    public static bool GetThumbOut(OVRInput.Controller controller)
    {
        return !OVRInput.Get(OVRInput.Button.Two, controller);

    }
}
