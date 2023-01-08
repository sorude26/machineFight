using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StageData
{
    public static int StageID { get; set; }
    public static int StageLevel { get; set; }
    public static string StageName { get; set; }
}
[System.Serializable]
public struct StageGuideData
{
    public string StageName;
    public int Level;
    public Sprite StageImage;
    public string Mission;
    public string TargetSceneName;
}