using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PartsCategory
{
    Head,
    Body,
    LHand,
    RHand,
    Leg,
    Booster,
    LWeapon,
    RWeapon
}

public class PartsButton : MonoBehaviour
{
    private PlayerData _playerData;
    public PartsCategory _partsCategory = default;
    public int _partsId = default;

    /// <summary>
    /// 機体のパーツを変更する
    /// </summary>
    /// <param name="partsId">パーツのID</param>
    public void Customize()
    {
        _playerData = PlayerData.instance;
        _playerData.PartsLog();
        PartsBuildParam partsData = _playerData.BuildPreset;
        switch (_partsCategory)
        {
            case PartsCategory.Head:
                partsData.Head = _partsId;
                break;
            case PartsCategory.Body:
                partsData.Body = _partsId;
                break;
            case PartsCategory.LHand:
                partsData.LHand = _partsId;
                break;
            case PartsCategory.RHand:
                partsData.RHand = _partsId;
                break;
            case PartsCategory.Leg:
                partsData.Leg = _partsId;
                break;
            case PartsCategory.Booster:
                partsData.Booster = _partsId;
                break;
            case PartsCategory.LWeapon:
                partsData.LWeapon = _partsId;
                break;
            case PartsCategory.RWeapon:
                partsData.RWeapon = _partsId;
                break;
        }

        _playerData.BuildPreset = partsData;
        _playerData.Build();
    }
}
