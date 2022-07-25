using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 射撃関係の予測計算を行う
/// </summary>
public class ShotPrediction
{
    private const int PREDICTION_FLAME_COUNT = 3;
    /// <summary>
    /// 線形予測による予測目標座標を返す
    /// </summary>
    /// <param name="startPos">開始点</param>
    /// <param name="targetPos">目標点</param>
    /// <param name="beforePos">１フレーム前の座標</param>
    /// <param name="shotSpeed">弾速</param>
    /// <returns></returns>
    public static Vector3 Linear(Vector3 startPos, Vector3 targetPos, Vector3 beforePos, float shotSpeed)
    {
        var shotFixedSpeed = shotSpeed * Time.fixedDeltaTime;
        //現在の速度
        Vector3 currentMoveSpeed = targetPos - beforePos;
        //目標との距離
        Vector3 targetDis = targetPos - startPos;
        //移動距離２乗
        float moveRange = Vector3.SqrMagnitude(currentMoveSpeed) - shotFixedSpeed * shotFixedSpeed;
        //目標の速度と距離の差
        float targetAngleSub = Vector3.Dot(targetDis, currentMoveSpeed);
        //目標距離２乗
        float targetRange = Vector3.SqrMagnitude(targetDis);        
        if (moveRange == 0)
        {
            if (targetAngleSub == 0) { return targetPos; }
            return targetPos + currentMoveSpeed * (-targetRange / targetAngleSub / 2);
        }
        //弾の命中フレームの計算
        float squareRoot = Mathf.Sqrt(Mathf.Abs(targetAngleSub * targetAngleSub - moveRange * targetRange));
        float shotHitFrame = 0;
        float shotHitFrameA = (-targetAngleSub - squareRoot) / moveRange;
        float shotHitFrameB = (-targetAngleSub + squareRoot) / moveRange;
        if (shotHitFrameA > 0 || shotHitFrameB > 0)
        {
            shotHitFrame = shotHitFrameA > shotHitFrameB ? shotHitFrameA : shotHitFrameB;
        }
        return targetPos + currentMoveSpeed * shotHitFrame;
    }
    /// <summary>
    /// 円形予測による予測目標座標を返す
    /// </summary>
    /// <param name="attackerPos"></param>
    /// <param name="targetPos"></param>
    /// <param name="beforePos"></param>
    /// <param name="twoBeforePos"></param>
    /// <param name="shotSpeed"></param>
    /// <returns></returns>
    public static Vector3 Circle(Vector3 attackerPos, Vector3 targetPos, Vector3 beforePos, Vector3 twoBeforePos, float shotSpeed)
    {
        //3点から円形の中心点を求める
        Vector3 centerPos = Circumcenter(targetPos, beforePos, twoBeforePos);

        Vector3 axis = Vector3.Cross(beforePos - centerPos, targetPos - centerPos);
        float angle = Vector3.Angle(beforePos - centerPos, targetPos - centerPos);

        float predictionFlame = Vector3.Distance(targetPos, attackerPos) / (shotSpeed * Time.fixedDeltaTime);
        for (int i = 0; i < PREDICTION_FLAME_COUNT; i++)
        {
            predictionFlame = Vector3.Distance(RotateToPos(targetPos, centerPos, axis, angle * predictionFlame), attackerPos) / (shotSpeed * Time.fixedDeltaTime);
        }

        return RotateToPos(targetPos, centerPos, axis, angle * predictionFlame);
    }

    /// <summary>
    /// 三角形の頂点三点の位置から外心の位置を返す
    /// </summary>
    /// <param name="posA"></param>
    /// <param name="posB"></param>
    /// <param name="posC"></param>
    /// <returns></returns>
    private static Vector3 Circumcenter(Vector3 posA, Vector3 posB, Vector3 posC)
    {
        //三辺の長さの二乗を出す
        float edgeA = Vector3.SqrMagnitude(posB - posC);
        float edgeB = Vector3.SqrMagnitude(posC - posA);
        float edgeC = Vector3.SqrMagnitude(posA - posB);

        //重心座標系で計算する
        float a = edgeA * (-edgeA + edgeB + edgeC);
        float b = edgeB * (edgeA - edgeB + edgeC);
        float c = edgeC * (edgeA + edgeB - edgeC);

        float abc = a + b + c;
        if (abc == 0)//0割りを防ぐ
        {
            abc = 3;
        }
        return (posA * a + posB * b + posC * c) / abc;
    }
    /// <summary>
    /// 目標位置をセンター位置で軸と角度で回転させた位置を返す
    /// </summary>
    /// <param name="target"></param>
    /// <param name="center"></param>
    /// <param name="axis"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    private static Vector3 RotateToPos(Vector3 target, Vector3 center, Vector3 axis, float angle)
    {
        return Quaternion.AngleAxis(angle, axis) * (target - center) + center;
    }
}
