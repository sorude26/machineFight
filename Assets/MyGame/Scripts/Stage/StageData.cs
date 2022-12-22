using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageData
{
    public static int StageLevel { get; set; }
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