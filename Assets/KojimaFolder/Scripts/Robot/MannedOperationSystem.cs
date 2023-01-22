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
    const string CONNECTED_POPUP_MESSAGE = "�g���@�\���ڑ�����܂���";

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
    /// �V�X�e�����N���ς݂�
    /// </summary>
    public bool IsOnline => _online && _connectedToDesctopUI;

    private void Awake()
    {
        Instance = this;
        _systemStartSwitch.OnTurnOn += TrySystemStart;
    }

    private void TrySystemStart()
    {
        //���ɋN�����Ȃ�Ȃɂ����Ȃ�
        if (_online) return;
        //���j�^�[�����ׂĊJ���Ă��Ȃ���΋N�����Ȃ�
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
