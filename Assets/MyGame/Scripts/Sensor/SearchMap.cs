using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchMap<TPoint> where TPoint: IMapPoint<TPoint>
{ 
    private List<TPoint> points;
    TPoint _target;
    private int _maxH;
    private bool CheckNeighor(TPoint start)
    {
        foreach (var neigher in start.ConnectPoint)
        {
            //�ڕW���W�������true
            if (CheckPoint(neigher, start)) { return true; }
        }
        //���̊m�F���W���Ȃ���ΏI��
        var next = GetMinimumCostOpenPoint();
        if (next == null) { return false; }
        //���̍��W�Ɉڂ�O�ɕ���
        start.State = SearchStateType.Close;
        return CheckNeighor(next);
    }
    private bool CheckPoint(TPoint point,TPoint  parent)
    {
        //�ҋ@��Ԃł����Open����
        if (point.State == SearchStateType.Idle)
        {
            point.State = SearchStateType.Open;
            //�T�����W�̕]����t����
            int pDis = point.IndexID > _target.IndexID ? point.IndexID - _target.IndexID : _target.IndexID - point.IndexID;
            point.DistanceCost = pDis / _maxH + pDis % _maxH;//�P���ȋ���
            point.Cost = parent.Cost + 1;//�ړ��R�X�g�v�Z
            point.TotalCost = point.DistanceCost + point.Cost;//���W�̕]��
            point.Parent = parent;
        }
        return false;
    }
    private TPoint GetMinimumCostOpenPoint()
    {
        TPoint pos = default;
        int score = 0;
        int cost = int.MaxValue;
        foreach (var point in points)
        {
            if (point.State != SearchStateType.Open) //Open�̍��W�̂ݕ]�����s��
            {
                continue;
            }
            if (point.TotalCost < cost) //��葍�R�X�g���Ⴂ���W��ێ�����
            {
                cost = point.TotalCost;
                pos = point;
                score = point.Cost;
            }
            else if (point.TotalCost == cost && point.Cost < score) //���R�X�g�������ł���΂��R�X�g���Ⴂ���W��ێ�����
            {
                pos = point;
                score = point.Cost;
            }
        }
        return pos;
    }
}
