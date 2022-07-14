using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaviStagePoint:IMapPoint<NaviStagePoint>
{
    public Vector3 Pos { get; }
    public int IndexID { get; }
    public SearchStateType State { get; set; }
    public List<NaviStagePoint> ConnectPoint { get; set; } = new List<NaviStagePoint>();
    public int Cost { get; set; }
    public int DistanceCost { get; set; }
    public int TotalCost { get; set; }
    public int Footprints { get; set; }
    public NaviStagePoint Parent { get; set; }
    public NaviStagePoint(Vector3 pos, int id)
    {
        Pos = pos;
        IndexID = id;
    }
}