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
        EventSystem.current.SetSelectedGameObject(content.transform.GetChild(1).gameObject);
    }


    /// <summary>
    /// ボタンの選択状態を初期化(何も選択していない状態)にする
    /// </summary>
    public static void OnButtonNonSelect()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    /// <summary>
    /// 今選択しているゲームオブジェクトを返す
    /// </summary>
    /// <returns></returns>
    public static Object OnGetCurrentButton()
    {
        return EventSystem.current.currentSelectedGameObject;
    }
}
