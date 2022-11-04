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
}
