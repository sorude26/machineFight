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
    const string CONNECTED_POPUP_MESSAGE = "拡張機能が接続されました";

    public static MannedOperationSystem Instance { get; private set; }

    [SerializeField]
    Switch _systemStartSwitch;
    [SerializeField]
    Text _connectText;
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
        _systemStartSwitch.OnTurnOn += TrySystemStart;
    }

    private void TrySystemStart()
    {
        //既に起動中ならなにもしない
        if (_online) return;
        //モニターがすべて開いていなければ起動しない
        if (!UIMonitorAnimator.Instance?.IsAllOpen ?? true) return;
        SystemStart();
        _online = true;
    }

    private void SystemStart()
    {
        _connectText.text = "";
        _monitorCover.SetActive(false);
    }
}
