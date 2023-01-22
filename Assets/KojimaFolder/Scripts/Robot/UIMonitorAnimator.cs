using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// VR���c���̃R�b�N�s�b�g���̃��j�^�[�̊J���쓙���Ǘ�����N���X
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
