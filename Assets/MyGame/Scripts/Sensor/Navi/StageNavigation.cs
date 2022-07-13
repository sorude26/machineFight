using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageNavigation
{
    private int _maxHorizontalIndex = default;
    private List<NaviStagePoint> _naviMap = default;
    private SearchMap<NaviStagePoint> _searchMap = default;
    public StageNavigation(List<NaviStagePoint> naviMap,int maxH)
    {
        _maxHorizontalIndex = maxH;
        _naviMap = naviMap;
    }
    public void Initialization()
    {
        _searchMap = new SearchMap<NaviStagePoint>(_maxHorizontalIndex);
    }
    public Vector3 GetMoveDir(Transform User, Transform Target)
    {
        var tPoint = _naviMap.OrderBy(point => Vector3.Distance(point.Pos, Target.position)).FirstOrDefault();
        var uPoint = _naviMap.OrderBy(point => Vector3.Distance(point.Pos, User.position)).FirstOrDefault();
        if (tPoint == null || uPoint == null) { return Vector3.zero; }
        var target = _searchMap.GetMoveTarget(tPoint, uPoint);
        if (target == null) { return Vector3.zero; }
        var dir = target.Pos - User.transform.position;
        dir.y = 0;
        return dir.normalized;
    }
    public void DataClear()
    {
        _searchMap.DataClear(_naviMap);
    }
}
