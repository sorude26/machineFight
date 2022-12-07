using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandTouchCollision : MonoBehaviour
{
    const float TOUCH_LENGTH = 0.1f;
    float _touchSqrLength;

    private void Awake()
    {
        _touchSqrLength = TOUCH_LENGTH * TOUCH_LENGTH;
    }

    public Switch GetActiveSwitch(Switch.HoldTypes holdType)
    {
        Switch result = Switch.SwitchList.Where(a => a.HoldType == holdType).
            OrderBy(a => GetSqrLengsh(a)).FirstOrDefault();
        if (result == null) return null;
        if (GetSqrLengsh(result) > _touchSqrLength)
        {
            //‹——£‚ª’·‚¢ê‡‚Ínull‚ğreturn
            return null;
        }
        return result;
    }
    float GetSqrLengsh(Switch swich)
    {
        return (this.transform.position - swich.HoldPosition.position).sqrMagnitude;
    }
}
