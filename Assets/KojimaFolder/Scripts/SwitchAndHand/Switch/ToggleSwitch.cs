using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSwitch : Switch
{
    [SerializeField]
    Transform _onPosition;
    [SerializeField]
    Transform _offPosition;
    [SerializeField]
    Transform _switchMovablePartObject;

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

    protected override void SwitchUpdateOnLockIn()
    {
        base.SwitchUpdateOnLockIn();
        Vector3 local = transform.InverseTransformPoint(_lockinHand.ReferencePositionWorld);
        if (local.y > 0)
        {
            TurnOn();
        }
        else
        {
            TurnOff();
        }
    }
}
