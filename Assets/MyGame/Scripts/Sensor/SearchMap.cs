using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �T���@�\�N���X
/// </summary>
/// <typeparam name="TPoint"></typeparam>
public class SearchMap<TPoint> where TPoint: IMapPoint<TPoint>
{
    private int _maxH = default;
    private TPoint _goal = default;
    private List<TPoint> _openPoints = new List<TPoint>();
    public SearchMap(int maxH)
    {
        _maxH = maxH;
    }
    private void CheckNeighor(TPoint start)
    {
        foreach (var neigher in start.ConnectPoint)
        {
            //�ڕW���W������ΏI��
            if (CheckPoint(neigher, start)) { return; }
        }
        //���̊m�F���W���Ȃ���ΏI��
        var next = GetMinimumCostOpenPoint();
        if (next == null) { return; }
        //���̍��W�Ɉڂ�O�ɕ���
        start.State = SearchStateType.Close;
        CheckNeighor(next);
    }
    private bool CheckPoint(TPoint point,TPoint parent)
    {
        //�ڕW�ł���ΏI������
        if (point.IndexID == _goal.IndexID) 
        {
            SetNextTarget(parent);
            return true; 
        }
        //�ҋ@��Ԃł����Open����
        if (point.State == SearchStateType.Idle)
        {
            point.State = SearchStateType.Open;
            //�T�����W�̕]����t����
            //�l�̒Ⴂ����index���ň����A���_����̍��W�ʒu�ɂ���
            int index = point.IndexID > _goal.IndexID ? point.IndexID - _goal.IndexID : _goal.IndexID - point.IndexID;
            //�c���̋����Ɖ����̋����𑫂��Ď��������o��
            point.DistanceCost = index / _maxH + index % _maxH;
            //�ړ������̐ݒ�
            //int dis = point.IndexID > parent.IndexID ? point.IndexID - parent.IndexID : parent.IndexID - point.IndexID;
            //point.Cost = parent.Cost + dis / _maxH + dis % _maxH;
            point.Cost = parent.Cost + 1;
            //���W�̕]��
            point.TotalCost = point.DistanceCost + point.Cost;
            point.Parent = parent;
            //OpenList�ɒǉ�
            _openPoints.Add(point);
        }
        return false;
    }
    /// <summary>
    /// �ŏ��R�X�g��Open���W��Ԃ�
    /// </summary>
    /// <returns></returns>
    private TPoint GetMinimumCostOpenPoint()
    {
        TPoint pos = default;
        int score = 0;
        int cost = int.MaxValue;
        foreach (var point in _openPoints)
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
    /// <summary>
    /// ���̈ړ��ڕW��ݒ肷��
    /// </summary>
    /// <param name="point"></param>
    private void SetNextTarget(TPoint point)
    {
        if (point.Parent == null) { return; }
        _goal = point;
        SetNextTarget(point.Parent);
    }
    /// <summary>
    /// �w�肵�����W�֌��������߂̎��̍��W��Ԃ�
    /// </summary>
    /// <param name="targetPoint"></param>
    /// <param name="start"></param>
    /// <param name="map"></param>
    /// <returns></returns>
    public TPoint GetMoveTarget(TPoint targetPoint,TPoint start)
    {
        _goal = targetPoint;
        start.State = SearchStateType.Close;
        CheckNeighor(start);
        return _goal;
    }
    public void DataClear(List<TPoint> map)
    {
        foreach (var point in map)
        {
            point.Parent = default;
            point.State = SearchStateType.Idle;
        }
        _openPoints.Clear();
    }
}
