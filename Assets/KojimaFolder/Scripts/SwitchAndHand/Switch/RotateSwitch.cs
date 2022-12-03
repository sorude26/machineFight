using Newtonsoft.Json.Linq;
using OVRSimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSwitch : Switch
{
    [SerializeField]
    Transform _onPosition;
    [SerializeField]
    Transform _offPosition;
    [SerializeField]
    Transform _switchMovablePartObject;
    [SerializeField]
    float _holdRotateOffset;

    public override void TurnOn(bool isInit = false)
    {
        base.TurnOn(isInit);
        _switchMovablePartObject.transform.parent = _onPosition;
        _switchMovablePartObject.transform.localPosition = Vector3.zero;
        _switchMovablePartObject.transform.localRotation = Quaternion.identity;
    }

    public override void TurnOff(bool isInit = false)
    {
        base.TurnOff(isInit);
        _switchMovablePartObject.transform.parent = _offPosition;
        _switchMovablePartObject.transform.localPosition = Vector3.zero;
        _switchMovablePartObject.transform.localRotation = Quaternion.identity;
    }
    protected override void Awake()
    {
        base.Awake();
        TurnOff(true);
    }



    protected override void LockIn(SwitchCtrlHand from)
    {
        float offset = _holdRotateOffset;
        if (!from.IsRightHand)
        {
            offset = -offset;
        }
        _holdRotation.localRotation = Quaternion.Euler(0,offset,0);
        base.LockIn(from);
    }

    protected override void SwitchUpdateOnLockIn()
    {
        base.SwitchUpdateOnLockIn();
        Vector3 local = (Quaternion.Inverse(this.transform.rotation) * _lockinHand.transform.parent.rotation).eulerAngles.NomalizeRotate();
        if (local.z < 0)
        {
            TurnOn();
        }
        else
        {
            TurnOff();
        }
    }
}
