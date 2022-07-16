using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaviPoint:IMapPoint<NaviPoint>
{
    public Vector3 Pos { get; }
    public int IndexID { get; }   
    public int Cost { get; set; }
    public int DistanceCost { get; set; }
    public int TotalCost { get; set; }
    public int Footprints { get; set; }
    public bool IsNoEntry { get; set; }
    public NaviPoint Parent { get; set; }
    public SearchStateType State { get; set; }
    public List<NaviPoint> ConnectPoint { get; set; } = new List<NaviPoint>();
    public NaviPoint(Vector3 pos, int id)
    {
        Pos = pos;
        IndexID = id;
    }
}