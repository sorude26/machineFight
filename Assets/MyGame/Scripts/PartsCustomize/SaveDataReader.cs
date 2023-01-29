using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataReader
{
    private const int PARTS_DATA_NUM = 7;
    private static PartsData partsData = new PartsData();
    public static void SetData(PartsData data)
    {
        partsData = data;
    }
    public void ReadData()
    {
        for (int i = 0; i < PARTS_DATA_NUM; i++)
        {
            SetPartsData((PartsType)i);
        }
    }
    public void SetData()
    {
        partsData = new PartsData();
        for (int i = 0; i < PARTS_DATA_NUM; i++)
        {
            SavePartsData((PartsType)i);
        }
        Json.SavePartsData(partsData);
    }
    private void SetPartsData(PartsType type)
    {
        var data = this[type].Split(',');
        foreach (var part in data)
        {
            if (part == "")
            {
                return;
            }
            PlayerData.instance.PartsGet(type, int.Parse(part));
        }
    }
    private void SavePartsData(PartsType type)
    {
        var partsData = PlayerData.instance[type];
        foreach (var parts in partsData)
        {
            this[type] += parts.PartsID + ",";
        }
    }
    public string this[PartsType type]
    {
        get
        {
            switch (type)
            {
                case PartsType.Head:
                    return partsData.HeadData;
                case PartsType.Body:
                    return partsData.BodyData;
                case PartsType.LHand:
                    return partsData.LHandData;
                case PartsType.RHand:
                    return partsData.RHandData;
                case PartsType.Leg:
                    return partsData.LegData;
                case PartsType.BackPack:
                    return partsData.BackPackData;
                case PartsType.LWeapon:
                    return partsData.WeaponData;
                case PartsType.RWeapon:
                    return partsData.WeaponData;
                default:
                    break;
            }
            return "";
        }
        set
        {
            switch (type)
            {
                case PartsType.Head:
                    partsData.HeadData = value;
                    break;
                case PartsType.Body:
                    partsData.BodyData = value;
                    break;
                case PartsType.LHand:
                    partsData.LHandData = value;
                    break;
                case PartsType.RHand:
                    partsData.RHandData = value;
                    break;
                case PartsType.Leg:
                    partsData.LegData = value;
                    break;
                case PartsType.BackPack:
                    partsData.BackPackData = value;
                    break;
                case PartsType.LWeapon:
                    partsData.WeaponData = value;
                    break;
                case PartsType.RWeapon:
                    partsData.WeaponData = value;
                    break;
                default:
                    break;
            }
        }
    }
}

[System.Serializable]
public class PartsData
{
    public string HeadData;
    public string BodyData;
    public string RHandData;
    public string LHandData;
    public string LegData;
    public string BackPackData;
    public string WeaponData;
}

