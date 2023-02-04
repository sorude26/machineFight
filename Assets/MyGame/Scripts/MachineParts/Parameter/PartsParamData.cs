using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PartsParamData : ScriptableObject
{
    [SerializeField]
    private PartsBodyData[] _bodyData;
    [SerializeField]
    private PartsHeadData[] _headData;
    [SerializeField]
    private PartsHandData[] _handData;
    [SerializeField]
    private PartsLegData[] _legData;
    [SerializeField]
    private PartsBackPackData[] _backData;
    [SerializeField]
    private PartsWeaponData[] _weaponData;
    [SerializeField]
    private PartsBuildParam[] _rank0PopParts;
    [SerializeField]
    private PartsBuildParam[] _rank1PopParts;
    [SerializeField]
    private PartsBuildParam[] _rank2PopParts;
    public PartsBodyData GetPartsBody(int id)
    {
        return _bodyData.Where(data => data.ID == id).FirstOrDefault();
    }
    public PartsHeadData GetPartsHead(int id)
    {
        return _headData.Where(data => data.ID == id).FirstOrDefault();
    }
    public PartsHandData GetPartsHand(int id)
    {
        return _handData.Where(data => data.ID == id).FirstOrDefault();
    }
    public PartsLegData GetPartsLeg(int id)
    {
        return _legData.Where(data => data.ID == id).FirstOrDefault();
    }
    public PartsBackPackData GetPartsBack(int id)
    {
        return _backData.Where(data => data.ID == id).FirstOrDefault();
    }
    public PartsWeaponData GetPartsWeapon(int id)
    {
        return _weaponData.Where(data => data.ID == id).FirstOrDefault();
    }
    public int GetRandamPartsId(PartsType type)
    {
        switch (type)
        {
            case PartsType.Head:
                return _headData[Random.Range(0,_headData.Length)].ID;
            case PartsType.Body:
                return _bodyData[Random.Range(0, _bodyData.Length)].ID;
            case PartsType.LHand:
                return _handData[Random.Range(0, _handData.Length)].ID;
            case PartsType.RHand:
                return _handData[Random.Range(0, _handData.Length)].ID;
            case PartsType.Leg:
                return _legData[Random.Range(0, _legData.Length)].ID;
            case PartsType.BackPack:
                return _backData[Random.Range(0, _backData.Length)].ID;
            case PartsType.LWeapon:
                return _weaponData[Random.Range(0,_weaponData.Length)].ID;
            case PartsType.RWeapon:
                return _weaponData[Random.Range(0, _weaponData.Length)].ID;
            default:
                break;
        }
        return 0;
    }
    public int GetRandamPartsId(PartsType type, int rank)
    {
        if (rank == 1)
        {
            return _rank1PopParts[Random.Range(0, _rank0PopParts.Length)][type];
        }
        else if (rank == 2)
        {
            return _rank2PopParts[Random.Range(0, _rank0PopParts.Length)][type];
        }
        return _rank0PopParts[Random.Range(0, _rank0PopParts.Length)][type];
    }
}
