using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreManager
{
    public static float time;
    public static string GetTimeScore()
    {
        return ((int)time).ToString();
    }
}
