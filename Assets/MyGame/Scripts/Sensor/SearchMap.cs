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
            //目標座標があればtrue
            if (CheckPoint(neigher, start)) { return true; }
        }
        //次の確認座標がなければ終了
        var next = GetMinimumCostOpenPoint();
        if (next == null) { return false; }
        //次の座標に移る前に閉じる
        start.State = SearchStateType.Close;
        return CheckNeighor(next);
    }
    private bool CheckPoint(TPoint point,TPoint  parent)
    {
        //待機状態であればOpenする
        if (point.State == SearchStateType.Idle)
        {
            point.State = SearchStateType.Open;
            //探索座標の評価を付ける
            int pDis = point.IndexID > _target.IndexID ? point.IndexID - _target.IndexID : _target.IndexID - point.IndexID;
            point.DistanceCost = pDis / _maxH + pDis % _maxH;//単純な距離
            point.Cost = parent.Cost + 1;//移動コスト計算
            point.TotalCost = point.DistanceCost + point.Cost;//座標の評価
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
}
