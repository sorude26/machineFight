using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// VR操縦時のコックピット内のモニターの開閉動作等を管理するクラス
/// </summary>
public class UIMonitorAnimator : MonoBehaviour
{
    public static UIMonitorAnimator Instance { get; private set; }

    public bool IsAllOpen => true;

    private void Awake()
    {
        Instance = this;
    }
}
