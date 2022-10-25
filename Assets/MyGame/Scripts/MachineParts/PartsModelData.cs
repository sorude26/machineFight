using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private BoosterController[] _allBooster = default;
    [SerializeField]
    private WeaponBase[] _allWeaponParts = default;
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
    public BoosterController GetBooster(int id)
    {
        return _allBooster.Where(parts => parts.ID == id).FirstOrDefault();
    }
    public WeaponBase GetWeapon(int id)
    {
        return _allWeaponParts.Where(weapon => weapon.ID == id).FirstOrDefault();
    }
}

public interface IPartsModel
{
    public int ID { get; }
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
}