using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILegState
{
    void ExecuteEnter(LegStateContext context);
    void ExecuteUpdate(LegStateContext context);
    void ExecuteExit(LegStateContext context);
}
