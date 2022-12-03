using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OculusGameInput
{
    public static bool GetPinchIn(OVRInput.Controller controller)
    {
        return OVRInput.GetDown(OVRInput.Button.Two, controller);
    }
    public static bool GetPinchOut(OVRInput.Controller controller)
    {
        return !OVRInput.Get(OVRInput.Button.Two, controller);
    }

    public static bool GetGrabIn(OVRInput.Controller controller)
    {
        return false;
    }
    public static bool GetGrabOut(OVRInput.Controller controller)
    {
        return false;
    }
}
