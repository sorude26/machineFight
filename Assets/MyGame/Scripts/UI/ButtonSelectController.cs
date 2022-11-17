using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class ButtonSelectController
{
    /// <summary>
    /// ボタンがアクティブになったとき一番上のボタンを選択している状態にする
    /// </summary>
    /// <param name="window">ボタンの親オブジェクト</param>
    public static void OnButtonFirstSelect(GameObject window)
    {
        Debug.Log(window.transform.GetChild(0).gameObject);
        EventSystem.current.SetSelectedGameObject(window.transform.GetChild(0).gameObject);
    }
}
