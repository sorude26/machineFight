using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// “o˜^‚³‚ê‚½U“®ƒCƒxƒ“ƒg‚ğ‘€ì‚·‚éƒNƒ‰ƒX
/// </summary>
public static class StageShakeController
{
    public static event Action<Vector3, float, float> OnPlayStageShake;
    /// <summary>
    /// “o˜^Ï‚İU“®ƒCƒxƒ“ƒg‚ğŒÄ‚Ô
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
