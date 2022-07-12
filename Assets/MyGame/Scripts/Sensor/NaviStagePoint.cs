using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaviStagePoint:IMapPoint<NaviStagePoint>
{
    public Vector3 Pos { get; }
    public int IndexID { get; }
    public SearchStateType State { get; set; }
    public List<NaviStagePoint> ConnectPoint { get; set; }
    public int Cost { get; set; }
    public int DistanceCost { get; set; }
    public int TotalCost { get; set; }
    public NaviStagePoint Parent { get; set; }
    public NaviStagePoint(Vector3 pos, int id)
    {
        Pos = pos;
        IndexID = id;
    }
}
public interface IMapPoint<TMapPoint> where TMapPoint : IMapPoint<TMapPoint>
{
    public int IndexID { get; }
    public int Cost { get; set; }
    public int DistanceCost { get; set; }
    public int TotalCost { get; set; }
    public SearchStateType State { get; set; }
    public List<TMapPoint> ConnectPoint { get; set; }
    public TMapPoint Parent { get; set; }
}
public enum SearchStateType
{
    Idle,
    Open,
    Close,
}