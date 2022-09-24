using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 登録された振動イベントを操作するクラス
/// </summary>
public static class StageShakeController
{
    public static event Action<Vector3, float, float> OnPlayStageShake;
    /// <summary>
    /// 登録済み振動イベントを呼ぶ
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="power"></param>
    /// <param name="time"></param>
    public static void PlayShake(Vector3 pos, float power = 1f, float time = 0.1f)
    {
        OnPlayStageShake?.Invoke(pos, power, time);
    }
    public static void PlayShake(ShakeParam shakeParam)
    {
        OnPlayStageShake?.Invoke(shakeParam.Pos, shakeParam.Power, shakeParam.Time);
    }
}
[Serializable]
public struct ShakeParam
{
    public Vector3 Pos;
    public float Power;
    public float Time;
    public ShakeParam(Vector3 pos,float power,float time)
    {
        Pos = pos;
        Power = power;
        Time = time;
    }
}
