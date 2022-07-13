using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 探索機能クラス
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
            //目標座標があれば終了
            if (CheckPoint(neigher, start)) { return; }
        }
        //次の確認座標がなければ終了
        var next = GetMinimumCostOpenPoint();
        if (next == null) { return; }
        //次の座標に移る前に閉じる
        start.State = SearchStateType.Close;
        CheckNeighor(next);
    }
    private bool CheckPoint(TPoint point,TPoint parent)
    {
        //目標であれば終了する
        if (point.IndexID == _goal.IndexID) 
        {
            SetNextTarget(parent);
            return true; 
        }
        //待機状態であればOpenする
        if (point.State == SearchStateType.Idle)
        {
            point.State = SearchStateType.Open;
            //探索座標の評価を付ける
            //値の低い方のindex数で引き、原点からの座標位置にする
            int index = point.IndexID > _goal.IndexID ? point.IndexID - _goal.IndexID : _goal.IndexID - point.IndexID;
            //縦軸の距離と横軸の距離を足して実距離を出す
            point.DistanceCost = index / _maxH + index % _maxH;
            //移動歩数の設定
            //int dis = point.IndexID > parent.IndexID ? point.IndexID - parent.IndexID : parent.IndexID - point.IndexID;
            //point.Cost = parent.Cost + dis / _maxH + dis % _maxH;
            point.Cost = parent.Cost + 1;
            //座標の評価
            point.TotalCost = point.DistanceCost + point.Cost;
            point.Parent = parent;
            //OpenListに追加
            _openPoints.Add(point);
        }
        return false;
    }
    /// <summary>
    /// 最小コストのOpen座標を返す
    /// </summary>
    /// <returns></returns>
    private TPoint GetMinimumCostOpenPoint()
    {
        TPoint pos = default;
        int score = 0;
        int cost = int.MaxValue;
        foreach (var point in _openPoints)
        {
            if (point.State != SearchStateType.Open) //Openの座標のみ評価を行う
            {
                continue;
            }
            if (point.TotalCost < cost) //より総コストが低い座標を保持する
            {
                cost = point.TotalCost;
                pos = point;
                score = point.Cost;
            }
            else if (point.TotalCost == cost && point.Cost < score) //総コストが同率であればよりコストが低い座標を保持する
            {
                pos = point;
                score = point.Cost;
            }
        }
        return pos;
    }
    /// <summary>
    /// 次の移動目標を設定する
    /// </summary>
    /// <param name="point"></param>
    private void SetNextTarget(TPoint point)
    {
        if (point.Parent == null) { return; }
        _goal = point;
        SetNextTarget(point.Parent);
    }
    /// <summary>
    /// 指定した座標へ向かうための次の座標を返す
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
