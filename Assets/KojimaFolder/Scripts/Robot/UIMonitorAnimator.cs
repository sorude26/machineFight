using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

/// <summary>
/// VR操縦時のコックピット内のモニターの開閉動作等を管理するクラス
/// </summary>
public class UIMonitorAnimator : MonoBehaviour
{
    const int SWITCH_COUNT = 5;
    const string ANIMATOR_OPEN_KEY = "open";

    public static UIMonitorAnimator Instance { get; private set; }

    [Header("アニメーター")]
    [SerializeField]
    Animator _main;
    [SerializeField]
    Animator[] _rightDown;
    [SerializeField]
    Animator _radar;
    [SerializeField]
    Animator[] _centerDown;
    [SerializeField]
    Animator[] _leftDown;

    [Header("スイッチ")]
    [SerializeField]
    Switch _mainSwitch;
    [SerializeField]
    Switch _rightDownSwitch;
    [SerializeField]
    Switch _radarSwitch;
    [SerializeField]
    Switch _centarDownSwitch;
    [SerializeField]
    Switch _leftDownSwitch;

    bool _isMainOpen = false;
    int _monitorOpenCount;

    public bool IsAllOpen => _monitorOpenCount >= SWITCH_COUNT;

    private void Awake()
    {
        Instance = this;
        _mainSwitch.OnTurnOn += () => StartCoroutine(MainOpen(_main));
        _rightDownSwitch.OnTurnOn += () => StartCoroutine(MonitorOpen(_rightDown));
        _radarSwitch.OnTurnOn += () => StartCoroutine(MonitorOpen(_radar));
        _centarDownSwitch.OnTurnOn += () => StartCoroutine(MonitorOpen(_centerDown));
        _leftDownSwitch.OnTurnOn += () => StartCoroutine(MonitorOpen(_leftDown));
    }

    private IEnumerator MainOpen(Animator animator)
    {
        animator.SetBool(ANIMATOR_OPEN_KEY, true);
        yield return new WaitForSeconds(3.0f);
        _monitorOpenCount++;
        _isMainOpen = true;
    }

    private IEnumerator MonitorOpen(Animator animator)
    {
        while (!_isMainOpen)
        {
            yield return null;
        }
        if (animator.GetBool(ANIMATOR_OPEN_KEY))
        {
            yield break;
        }
        _monitorOpenCount++;
        animator.SetBool(ANIMATOR_OPEN_KEY, true);
    }

    private IEnumerator MonitorOpen(Animator[] animators)
    {
        while (!_isMainOpen)
        {
            yield return null;
        }
        if (animators[0].GetBool(ANIMATOR_OPEN_KEY))
        {
            yield break;
        }
        _monitorOpenCount++;
        foreach (var item in animators)
        {
            
            item.SetBool(ANIMATOR_OPEN_KEY, true);
            yield return new WaitForSeconds(0.25f);
        }
    }
}
