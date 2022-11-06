using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryItem : ItemBase
{
    [SerializeField]
    private int _recoveryPoint = 20;
    public override void CatchItem(ItemCatcher catcher)
    {
        catcher.Player.RecoveryHp(_recoveryPoint);
    }
}
