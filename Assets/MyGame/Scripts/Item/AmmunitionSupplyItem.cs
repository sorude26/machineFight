using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmunitionSupplyItem : ItemBase
{
    [SerializeField]
    private float refillAmmunition = 0.05f;
    public override void CatchItem(ItemCatcher catcher)
    {
        catcher.Player.RefillAmmunition(refillAmmunition);
    }
}
