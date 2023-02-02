using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Drawing;

public class NavigationMap
{
    private const float MIN_RANGE = 15f;
    private int _power = default;
    private int _maxHorizontalIndex = default;
    private NaviPoint _currentTarget = default;
    private Thread _therad = default;
    private List<NaviPoint> _naviMap = default;
    private SearchMap<NaviPoint> _searchMap = default;
    public List<NaviPoint> NaviMap { get { return _naviMap; } }
    public NavigationMap(List<NaviPoint> naviMap,int maxH)
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
        _searchMap = new SearchMap<NaviPoint>(_maxHorizontalIndex);
    }
    public void MakeFootprints(Transform target,int power)
    {
        var tPoint = GetNearPoint(target.position);
        if (tPoint == null) { return; }
        _currentTarget = tPoint;
        _power = power;
        _therad = new Thread(new ThreadStart(MakeFootprints));
        _therad.Start();
    }
    public Vector3 GetMoveDir(Transform user,int power)
    {
        if (Vector3.Distance(_currentTarget.Pos,user.position) < MIN_RANGE)
        {
            var minRangeDir = _currentTarget.Pos - user.position;
            minRangeDir.y = 0;
            return minRangeDir;
        }
        var uPoint = GetNearPoint(user.position);
        if (uPoint == null) { return Vector3.zero; }
        if (uPoint.Footprints <= power)
        {
            return Vector3.zero;
        }
        var target = GetNextPoint(user.position, uPoint);
        if (target == null) { return Vector3.zero; }
        var dir = target.Pos - user.position;
        dir.y = 0;
        return dir.normalized;
    }

    private NaviPoint GetNearPoint(Vector3 pos)
    {
        float minDis = float.MaxValue;
        float range = 0;
        NaviPoint nearPoint = null;
        foreach (var naviPoint in _naviMap)
        {
            range = Vector3.Distance(naviPoint.Pos, pos);
            if (naviPoint.IsNoEntry == false && range < minDis)
            {
                minDis = range;
                nearPoint = naviPoint;
            }
        }
        return nearPoint;
    }
    private NaviPoint GetNextPoint(Vector3 pos, NaviPoint point)
    {
        float minDis = float.MaxValue;
        float range = 0;
        NaviPoint nearPoint = null;
        foreach (var naviPoint in point.ConnectPoint)
        {
            range = Vector3.Distance(naviPoint.Pos, pos);
            if (naviPoint.IsNoEntry == false && point.Footprints + 1 == naviPoint.Footprints && range < minDis)
            {
                minDis = range;
                nearPoint = naviPoint;
            }
        }
        return nearPoint;
    }
}
