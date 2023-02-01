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
    const int SE_SISTEMSTART_ID = 59;
    const float SE_SYSTEMSTART_VOLUME = 1.0f;
    const int SE_AMBIENT_ID = 60;
    const float SE_AMBIENT_VOLUME = 0.7f;

    public static MannedOperationSystem Instance { get; private set; }

    
    [SerializeField]
    Text _connectText;
    [SerializeField]
    GameObject _mainCanvas;
    [SerializeField]
    GameObject _systemCanvas;
    [SerializeField]
    GameObject _monitorCover;
    [SerializeField]
    Light _cockpitLight;
    [SerializeField]
    GameObject[] _disactiveUIObjects;

    bool _online;
    bool _connectedToDesctopUI;

    Switch SystemStartSwitch => PlayerVrCockpit.Instance.SystemStartSwitch;
    /// <summary>
    /// システムが起動済みか
    /// </summary>
    public bool IsOnline => _online && _connectedToDesctopUI;

    private void Awake()
    {
        Instance = this;
        _monitorCover.SetActive(true);
        _cockpitLight.color = Color.black;
    }

    private IEnumerator Start()
    {
        while(PlayerVrCockpit.Instance == null)
        {
            yield return null;
        }

        if (SystemStartSwitch != null)
        {
            SystemStartSwitch.OnTurnOn += TrySystemStart;
        }
    }

    [ContextMenu("SystemStart")]
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
        //サウンド再生開始
        SoundManager.Instance.PlaySE(SE_SISTEMSTART_ID, SE_SYSTEMSTART_VOLUME);
        SoundManager.Instance.PlaySELoop(SE_AMBIENT_ID, this.gameObject, SE_AMBIENT_VOLUME);
        //ライト起動
        _cockpitLight.color = Color.white;
        yield return new WaitForSeconds(2f);

        //接続開始
        _connectText.text = CONNECTING_MESSAGE;
        yield return new WaitForSeconds(3.0f);

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
        _mainCanvas.SetActive(true);
    }

    private void ChangeMainUI()
    {
        foreach (var item in _disactiveUIObjects)
        {
            item.SetActive(false);
        }
    }
}
