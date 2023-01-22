using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// VR操縦時のコックピット内モニターを管理するクラス
/// </summary>
public class MannedOperationSystem : MonoBehaviour
{
    const string CONNECTING_MESSAGE = "CONNECTING TO MAINSYSTEM...";
    const string CONNECTED_MESSAGE = "CONNECTED";
    const string ONLINE_MESSAGE = "READY TO GO";
    const string CONNECTED_POPUP_MESSAGE = "拡張機能が接続されました";

    public static MannedOperationSystem Instance { get; private set; }

    [SerializeField]
    Switch _systemStartSwitch;
    [SerializeField]
    Text _connectText;
    [SerializeField]
    GameObject _mainCanvas;
    [SerializeField]
    GameObject _systemCanvas;
    [SerializeField]
    GameObject _monitorCover;

    bool _online;
    bool _connectedToDesctopUI;
    /// <summary>
    /// システムが起動済みか
    /// </summary>
    public bool IsOnline => _online && _connectedToDesctopUI;

    private void Awake()
    {
        Instance = this;
        if (_systemStartSwitch != null)
        {
            _systemStartSwitch.OnTurnOn += TrySystemStart;
        }
        _monitorCover.SetActive(true);
    }

    private void TrySystemStart()
    {
        //既に起動中ならなにもしない
        if (_online) return;
        //モニターがすべて開いていなければ起動しない
        if (!UIMonitorAnimator.Instance?.IsAllOpen ?? true) return;
        SystemStart();
    }

    private void SystemStart()
    {
        _online = true;
        StartCoroutine(SystemStartSequence());
    }

    IEnumerator SystemStartSequence()
    {
        //画面起動
        _connectText.text = "";
        _monitorCover.SetActive(false);
        _systemCanvas.SetActive(true);
        _mainCanvas.SetActive(false);
        yield return new WaitForSeconds(1f);

        //接続開始
        _connectText.text = CONNECTING_MESSAGE;
        yield return new WaitForSeconds(2.0f);

        //接続完了
        _connectText.text = CONNECTED_MESSAGE;
        PopUpLog.CreatePopUp(CONNECTED_POPUP_MESSAGE);
        //メイン画面のUI変更
        ChangeMainUI();
        yield return new WaitForSeconds(2f);

        //操縦可能
        _connectText.text = ONLINE_MESSAGE;
        _connectedToDesctopUI = true;
        yield return new WaitForSeconds(1f);

        //全UI表示
        _systemCanvas.SetActive(false);
    }

    private void ChangeMainUI()
    {

    }
}
