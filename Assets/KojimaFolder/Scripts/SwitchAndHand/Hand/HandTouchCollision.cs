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

    public Switch GetActiveSwitch()
    {
        Switch result = Switch.SwitchList.
            OrderBy(a => GetSqrLengsh(a)).FirstOrDefault();
        if (result == null) return null;
        if (GetSqrLengsh(result) > _touchSqrLength)
        {
            //距離が長い場合はnullをreturn
            return null;
        }
        return result;
    }
    float GetSqrLengsh(Switch swich)
    {
        return (this.transform.position - swich.HoldPosition.position).sqrMagnitude;
    }
}
