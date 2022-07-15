using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class NavigationMap
{
    private int _maxHorizontalIndex = default;
    private List<NaviStagePoint> _naviMap = default;
    private SearchMap<NaviStagePoint> _searchMap = default;
    private NaviStagePoint _currentTarget = default;
    private Thread _therad = default;
    private int _power = default;
    public NavigationMap(List<NaviStagePoint> naviMap,int maxH)
    {
        _maxHorizontalIndex = maxH;
        _naviMap = naviMap;
    }
    private void MakeFootprints()
    {
        _searchMap.DataClear();
        _searchMap.MakeFootprints(_currentTarget, _power);
    }
    public void Initialization()
    {
        _searchMap = new SearchMap<NaviStagePoint>(_maxHorizontalIndex);
    }
    public void MakeFootprints(Transform target,int power)
    {
        var tPoint = _naviMap.OrderBy(point => Vector3.Distance(point.Pos, target.position)).FirstOrDefault();
        if (tPoint == null) { return; }
        _currentTarget = tPoint;
        _power = power;
        _therad = new Thread(new ThreadStart(MakeFootprints));
        _therad.Start();
    }
    public Vector3 GetMoveDir(Transform user)
    {
        var uPoint = _naviMap.OrderBy(point => Vector3.Distance(point.Pos, user.position)).FirstOrDefault();
        if (uPoint == null) { return Vector3.zero; }
        var target = uPoint.ConnectPoint.Where(point => uPoint.Footprints + 1 == point.Footprints).OrderBy(point => Vector3.Distance(point.Pos, user.position)).FirstOrDefault();
        if (target == null) { return Vector3.zero; }
        var dir = target.Pos - user.transform.position;
        dir.y = 0;
        return dir.normalized;
    }
}
