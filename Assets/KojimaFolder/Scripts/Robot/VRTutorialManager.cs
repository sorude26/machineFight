using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRTutorialManager : MonoBehaviour
{
    public static VRTutorialManager Instance { get; private set; }
    [SerializeField]
    GameObject _iconPrefab;

    private Dictionary<Transform, GameObject> icons = new Dictionary<Transform, GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// ���C�����j�^�A���[�_�[���J���ă`���[�g���A����\���ł���悤�ɂ���
    /// </summary>
    public void TutorialSetup()
    {

    }

    /// <summary>
    /// ���삵�Ăق��������ɃA�C�R�����o��������
    /// </summary>
    public void MakeIcon(Transform pos)
    {
        var icon = Instantiate(_iconPrefab);
        icon.transform.position = pos.position;
        icon.transform.rotation = Quaternion.identity;
        icon.transform.parent = pos;
        icons.Add(pos, icon);
    }

    /// <summary>
    /// �A�C�R�����폜����
    /// </summary>
    public void DeleteIcon(Transform pos)
    {
        if (icons.ContainsKey(pos))
        {
            Destroy(icons[pos]);
            icons.Remove(pos);
        }
    }
}
