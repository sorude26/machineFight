using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandTouchCollision : MonoBehaviour
{
    public Switch GetActiveSwitch(Switch.HoldTypes holdType)
    {
        return Switch.SwitchList.Where(a => a.HoldType == holdType).
            OrderBy(a => GetSqrLengsh(a)).FirstOrDefault();
    }
    float GetSqrLengsh(Switch swich)
    {
        return (this.transform.position - swich.HoldPosition.position).sqrMagnitude;
    }
}
