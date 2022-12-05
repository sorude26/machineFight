using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightStick : Switch
{
    [SerializeField]
    float _holdMoveOffsetX;

    protected override void LockIn(SwitchCtrlHand from)
    {
        float offset = _holdMoveOffsetX;
        if (!from.IsRightHand)
        {
            offset = -offset;
        }
        var pos = _holdPosition.localPosition;
        pos.x = offset;
        _holdPosition.localPosition = pos;
        base.LockIn(from);
    }
}
