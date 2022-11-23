using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsDropItem : ItemBase
{
    [SerializeField]
    private float _viewTime = 2f;
    private PartsType _partsType;
    private int _partsID;
    public override void CatchItem(ItemCatcher catcher)
    {
        string get = "";
        switch (_partsType)
        {
            case PartsType.Head:
                get = PartsManager.Instance.AllParamData.GetPartsHead(_partsID).Name;
                break;
            case PartsType.Body:
                get = PartsManager.Instance.AllParamData.GetPartsBody(_partsID).Name;
                break;
            case PartsType.LHand:
                get = PartsManager.Instance.AllParamData.GetPartsHand(_partsID).Name;
                break;
            case PartsType.RHand:
                get = PartsManager.Instance.AllParamData.GetPartsHand(_partsID).Name;
                break;
            case PartsType.Leg:
                get = PartsManager.Instance.AllParamData.GetPartsLeg(_partsID).Name;
                break;
            case PartsType.BackPack:
                get = PartsManager.Instance.AllParamData.GetPartsBack(_partsID).Name;
                break;
            case PartsType.LWeapon:
                get = PartsManager.Instance.AllModelData.GetWeapon(_partsID).name;
                break;
            case PartsType.RWeapon:
                get = PartsManager.Instance.AllModelData.GetWeapon(_partsID).name;
                break;
            default:
                break;
        }
        PopUpLog.CreatePopUp($"{_partsType}\n{get} “üŽè‚µ‚Ü‚µ‚½", _viewTime);
        //Debug.Log($"Type:{_partsType} ID:{_partsID}");
    }
    public void SetData(PartsType type, int id)
    {
        _partsType = type;
        _partsID = id;
    }
}
