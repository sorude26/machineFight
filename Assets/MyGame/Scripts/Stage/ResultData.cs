using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// リザルトデータクラス
/// </summary>
public static class ResultData
{
    /// <summary>
    /// ステージでのトータル被ダメージ
    /// </summary>
    public static int TotalDamage { get; set; }
    /// <summary>
    /// 総撃破数
    /// </summary>
    public static int TotalCount { get; set; }
    /// <summary>
    /// ターゲットの撃破数
    /// </summary>
    public static int TotalTargetCount { get; set; }
    /// <summary>
    /// ターゲットの数
    /// </summary>
    public static int TotalTargetNum { get; set; }
    /// <summary>
    /// ボスの撃破数
    /// </summary>
    public static int TotalBossCount { get; set; }
    /// <summary>
    /// ボスの数
    /// </summary>
    public static int TotalBossNum { get; set; }
    /// <summary>
    /// ステージをクリアしたかのフラグ
    /// </summary>
    public static bool IsStageClear { get; set; }
}
