using Oculus.Interaction.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//メモ コントローラーを左右個別に扱った場合
//上ボタン　Two
//下ボタン  One
//人差し指トリガー  PrimaryIndexTrigger
//中指トリガー  PrimaryHandTrigger
//サムスティック押し込み  Button.PrimaryThumbstick
//サムスティック Axis2D.PrimaryThumbstick

//左手スタートボタン Button.start
//右手オキュラスボタン なし？

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
