using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// VR���c���̃R�b�N�s�b�g�����j�^�[���Ǘ�����N���X
/// </summary>
public class MannedOperationSystem : MonoBehaviour
{
    const string CONNECTING_MESSAGE = "CONNECTING TO MAINSYSTEM...";
    const string CONNECTED_MESSAGE = "CONNECTED";
    const string ONLINE_MESSAGE = "READY TO GO";
    const string CONNECTED_POPUP_MESSAGE = "�g���@�\���ڑ�����܂���";
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
    /// �V�X�e�����N���ς݂�
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
        //���ɋN�����Ȃ�Ȃɂ����Ȃ�
        if (_online) return;
        //���j�^�[�����ׂĊJ���Ă��Ȃ���΋N�����Ȃ�
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
        //��ʋN��
        _connectText.text = "";
        _monitorCover.SetActive(false);
        _systemCanvas.SetActive(true);
        _mainCanvas.SetActive(false);
        //�T�E���h�Đ��J�n
        SoundManager.Instance.PlaySE(SE_SISTEMSTART_ID, SE_SYSTEMSTART_VOLUME);
        SoundManager.Instance.PlaySELoop(SE_AMBIENT_ID, this.gameObject, SE_AMBIENT_VOLUME);
        //���C�g�N��
        _cockpitLight.color = Color.white;
        yield return new WaitForSeconds(2f);

        //�ڑ��J�n
        _connectText.text = CONNECTING_MESSAGE;
        yield return new WaitForSeconds(3.0f);

        //�ڑ�����
        _connectText.text = CONNECTED_MESSAGE;
        PopUpLog.CreatePopUp(CONNECTED_POPUP_MESSAGE);
        //���C����ʂ�UI�ύX
        ChangeMainUI();
        yield return new WaitForSeconds(2f);

        //���c�\
        _connectText.text = ONLINE_MESSAGE;
        _connectedToDesctopUI = true;
        yield return new WaitForSeconds(1f);

        //�SUI�\��
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
