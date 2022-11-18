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
    public static void OnButtonFirstSelect(GameObject content)
    {
        foreach (Transform button in content.transform)
        {
            Debug.Log(button.GetComponent<PartsButton>()._partsCategory);
        }
        EventSystem.current.SetSelectedGameObject(content.transform.GetChild(0).gameObject);
        Debug.Log(EventSystem.current.currentSelectedGameObject.GetComponent<PartsButton>()._partsCategory);
    }


    /// <summary>
    /// ボタンの選択状態を初期化(何も選択していない状態)にする
    /// </summary>
    public static void OnButtonNonSelect()
    {
        EventSystem.current.SetSelectedGameObject(null);
        Debug.Log("Null");
    }
}
