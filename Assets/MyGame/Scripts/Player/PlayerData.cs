using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;

    public static PlayerData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerData();
            }
            return instance;
        }
    }
    private PartsBuildParam buildPreset = default;
    public PartsBuildParam BuildPreset { get { return buildPreset; } }
                                                                                   
    public enum PartsCategory
    {
        Head,
        Body,
        LHand,
        RHand,
        Leg,
        Booster,
        LWeapon,
        Rweapon
    }

    //Jsonに書き込み
    public void PresetSave()
    {
        
    }

    //Jsonから読み込み
    public void PresetLoad()
    {

    }

    /// <summary>
    /// 機体のパーツを変更する
    /// </summary>
    /// <param name="partsId">パーツのID</param>
    /// <param name="category">パーツの部位</param>
    public void Customize(int partsId, PartsCategory category)
    {
        switch (category)
        {
            case PartsCategory.Head:
                buildPreset.Head = partsId;
                break;
            case PartsCategory.Body:
                buildPreset.Body = partsId;
                break;
            case PartsCategory.LHand:
                buildPreset.LHand = partsId;
                break;
            case PartsCategory.RHand:
                buildPreset.RHand = partsId;
                break;
            case PartsCategory.Leg:
                buildPreset.Leg = partsId;
                break;
            case PartsCategory.Booster:
                buildPreset.Booster = partsId;
                break;
            case PartsCategory.LWeapon:
                buildPreset.LWeapon = partsId;
                break;
            case PartsCategory.Rweapon:
                buildPreset.RWeapon = partsId;
                break;
        }
    }
}
