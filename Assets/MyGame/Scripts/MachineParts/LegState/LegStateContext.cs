using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegStateContext
{
    private ILegState _currentState = default;
    public Transform LegTransform = default;
    public Transform BodyTransform = default;
    public LegStateContext()
    {
        _currentState.ExecuteEnter(this);
    }

    public void ExecuteUpdate(Vector3 moveDir,bool groundCheck)
    {
        _currentState.ExecuteUpdate(this);
    }
    public void ChangeState(ILegState legState)
    {
        _currentState.ExecuteExit(this);
        _currentState = legState;
        _currentState.ExecuteEnter(this);
    }
}
