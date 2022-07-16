using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationMapCreater : MonoBehaviour
{
    [SerializeField]
    private float _pointSpanRange = 1f;
    [SerializeField]
    private Transform _startPoint = null;
    [SerializeField]
    private Transform _endPoint = null;
    [SerializeField]
    private LayerMask _navigationLayer = default;
    private float _rayRange = 400f;
    private float _createSpan = default;
    private int _indexCount = default;
    private int _maxHorizontalIndex = 1;
    private SquaresIndex _mapIndex = default;
    private List<NaviPoint> _naviMap = new List<NaviPoint>();
    
    public NavigationMap CreateMap()
    {
        Vector3 start = _startPoint.position;
        Vector3 end = _endPoint.position;
        _createSpan = _pointSpanRange / 2;
        while (true)
        {
            SetNaviPoint(start);
            start.x += _createSpan;
            _maxHorizontalIndex++;
            if (end.x < start.x)
            {
                start.x = _startPoint.position.x;
                start.z += _createSpan;
                if (end.z < start.z)
                {
                    break;
                }
                _maxHorizontalIndex = 0;
            }
        }
        Debug.Log($"CreateEnd Horizontal:{_maxHorizontalIndex},Vertical:{_indexCount / _maxHorizontalIndex}");
        _mapIndex = new SquaresIndex(_maxHorizontalIndex, _indexCount / _maxHorizontalIndex);
        foreach (var point in _naviMap)
        {
            SetNeighorPoint(point);
        }
        Debug.Log($"ConnectEnd TotalIndex:{_indexCount},TotalCount:{_naviMap.Count}");
        return new NavigationMap(_naviMap, _maxHorizontalIndex);
    }
    private void SetNaviPoint(Vector3 start)
    {
        if(Physics.Raycast(start, Vector3.down, out RaycastHit hit, _rayRange, _navigationLayer))
        {
            _naviMap.Add(new NaviPoint(hit.point, _indexCount));
        }
        _indexCount++;
    }
    private void SetNeighorPoint(NaviPoint point)
    {
        foreach (var neighor in _mapIndex.GetNeighor(point.IndexID))
        //foreach (var neighor in _mapIndex.GetNeighorCross(point.IndexID))
        {
            //�}�b�v�Ɋ܂܂�Ă�����W���m�F����
            var check = _naviMap.Where(map => map.IndexID == neighor).FirstOrDefault();
            if (check == null) { continue; }
            //�w�苗�����ł���Ηאڒn�_�Ƃ��Ēǉ�����
            if (Vector3.Distance(check.Pos, point.Pos) < _pointSpanRange)
            {
                point.ConnectPoint.Add(check);
            }         
        }
    }
}
