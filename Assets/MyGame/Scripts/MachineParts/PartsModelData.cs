using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class PartsModelData : ScriptableObject
{
    [SerializeField]
    private HeadController[] _allHeadParts = default;
    [SerializeField]
    private BodyController[] _allBodyParts = default;
    [SerializeField]
    private HandController[] _allHandParts = default;
    [SerializeField]
    private LegController[] _allLegParts = default;
    [SerializeField]
    private BackPackController[] _allBackPack = default;
    [SerializeField]
    private WeaponBase[] _allWeaponParts = default;
    [SerializeField]
    private ColorSetData[] _allColorSetData = default;
    public HeadController GetPartsHead(int id)
    {
        return _allHeadParts.Where(parts => parts.ID == id).FirstOrDefault();
    }
    public BodyController GetPartsBody(int id)
    {
        return _allBodyParts.Where(parts => parts.ID == id).FirstOrDefault();
    }
    public HandController GetPartsHand(int id)
    {
        return _allHandParts.Where(parts => parts.ID == id).FirstOrDefault();
    }
    public LegController GetPartsLeg(int id)
    {
        return _allLegParts.Where(parts => parts.ID == id).FirstOrDefault();
    }
    public BackPackController GetBackPack(int id)
    {
        return _allBackPack.Where(parts => parts.ID == id).FirstOrDefault();
    }
    public WeaponBase GetWeapon(int id)
    {
        return _allWeaponParts.Where(weapon => weapon.ID == id).FirstOrDefault();
    }
    public ColorSetData GetColor(int id)
    {
        return _allColorSetData.Where(colorData => colorData.ID == id).FirstOrDefault();
    }
}

public interface IPartsModel
{
    public int ID { get; }
    public void ChangeColor(int id);
}
[System.Serializable]
public struct PartsBuildParam
{
    public int Head;
    public int Body;
    public int LHand;
    public int RHand;
    public int Leg;
    public int Booster;
    public int LWeapon;
    public int RWeapon;
    public int ColorId;

    public static readonly int PARTS_TYPE_NUM = Enum.GetValues(typeof(PartsType)).Length;
    public int this[PartsType type]
    {
        get
        {
            switch (type)
            {
                case PartsType.Head:
                    return Head;
                case PartsType.Body:
                    return Body;
                case PartsType.LHand:
                    return LHand;
                case PartsType.RHand:
                    return RHand;
                case PartsType.Leg:
                    return Leg;
                case PartsType.BackPack:
                    return Booster;
                case PartsType.LWeapon:
                    return LWeapon;
                case PartsType.RWeapon:
                    return RWeapon;
                default:
                    break;
            }
            return -1;
        }
        set
        {
            switch (type)
            {
                case PartsType.Head:
                    Head = value;
                    break;
                case PartsType.Body:
                    Body = value;
                    break;
                case PartsType.LHand:
                    LHand = value;
                    break;
                case PartsType.RHand:
                    RHand = value;
                    break;
                case PartsType.Leg:
                    Leg = value;
                    break;
                case PartsType.BackPack:
                    Booster = value;
                    break;
                case PartsType.LWeapon:
                    LWeapon = value;
                    break;
                case PartsType.RWeapon:
                    RWeapon = value;
                    break;
                default:
                    break;
            }
        }
    }
}
public enum PartsType
{
    Head = 0,
    Body = 1,
    LHand = 2,
    RHand = 3,
    Leg = 4,
    BackPack = 5,
    LWeapon = 6,
    RWeapon = 7,
}