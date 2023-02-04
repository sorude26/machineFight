using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// VR操縦時のコックピット内のモニターの開閉動作等を管理するクラス
/// </summary>
public class UIMonitorAnimator : MonoBehaviour
{
    const int SWITCH_COUNT = 5;
    const string ANIMATOR_OPEN_KEY = "open";
    const int SE_MAIN_ID = 55;
    const float SE_MAIN_VOLUME = 0.5f;
    const int SE_SUB_ID = 56;
    const float SE_SUB_VOLUME = 0.5f;

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
        _rightDownSwitch.OnTurnOn += () => StartCoroutine(MonitorOpen(_rightDown, _rightDownSwitch));
        _radarSwitch.OnTurnOn += () => StartCoroutine(MonitorOpen(_radar, _radarSwitch));
        _centarDownSwitch.OnTurnOn += () => StartCoroutine(MonitorOpen(_centerDown, _centarDownSwitch));
        _leftDownSwitch.OnTurnOn += () => StartCoroutine(MonitorOpen(_leftDown, _leftDownSwitch));
    }

    private void Start()
    {
        //最初にメインモニターを起動させる
        VRTutorialManager.Instance.MakeIcon(_mainSwitch.transform);
    }

    private IEnumerator MainOpen(Animator animator)
    {
        //メインモニターのアイコンを削除
        VRTutorialManager.Instance.DeleteIcon(_mainSwitch.transform);

        animator.SetBool(ANIMATOR_OPEN_KEY, true);
        SoundManager.Instance.PlaySE(SE_MAIN_ID, animator.gameObject, SE_MAIN_VOLUME);
        yield return new WaitForSeconds(3.0f);
        _monitorOpenCount++;
        _isMainOpen = true;
        //各モニターを起動させるアイコンを表示
        VRTutorialManager.Instance.MakeIcon(_rightDownSwitch.transform);
        VRTutorialManager.Instance.MakeIcon(_radarSwitch.transform);
        VRTutorialManager.Instance.MakeIcon(_centarDownSwitch.transform);
        VRTutorialManager.Instance.MakeIcon(_leftDownSwitch.transform);
    }

    private IEnumerator MonitorOpen(Animator animator, Switch iconSwitch)
    {
        while (!_isMainOpen)
        {
            yield return null;
        }
        if (animator.GetBool(ANIMATOR_OPEN_KEY))
        {
            yield break;
        }
        //モニターのアイコンを削除
        VRTutorialManager.Instance.DeleteIcon(iconSwitch.transform);

        _monitorOpenCount++;
        animator.SetBool(ANIMATOR_OPEN_KEY, true);
        SoundManager.Instance.PlaySE(SE_SUB_ID, animator.gameObject, SE_SUB_VOLUME);
    }

    private IEnumerator MonitorOpen(Animator[] animators, Switch iconSwitch)
    {
        while (!_isMainOpen)
        {
            yield return null;
        }
        if (animators[0].GetBool(ANIMATOR_OPEN_KEY))
        {
            yield break;
        }
        //モニターのアイコンを削除
        VRTutorialManager.Instance.DeleteIcon(iconSwitch.transform);

        _monitorOpenCount++;
        foreach (var item in animators)
        {
            item.SetBool(ANIMATOR_OPEN_KEY, true);
            SoundManager.Instance.PlaySE(SE_SUB_ID, item.gameObject, SE_SUB_VOLUME);
            yield return new WaitForSeconds(0.25f);
        }
    }
}
