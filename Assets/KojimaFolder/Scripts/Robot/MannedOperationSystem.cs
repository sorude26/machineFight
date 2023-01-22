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
    /// �V�X�e�����N���ς݂�
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
        yield return new WaitForSeconds(1f);

        //�ڑ��J�n
        _connectText.text = CONNECTING_MESSAGE;
        yield return new WaitForSeconds(2.0f);

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
    }

    private void ChangeMainUI()
    {

    }
}
