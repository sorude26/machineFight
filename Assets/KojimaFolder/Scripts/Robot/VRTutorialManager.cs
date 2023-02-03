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
    /// メインモニタ、レーダーを開けてチュートリアルを表示できるようにする
    /// </summary>
    public void TutorialSetup()
    {

    }

    /// <summary>
    /// 操作してほしい部分にアイコンを出現させる
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
    /// アイコンを削除する
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
