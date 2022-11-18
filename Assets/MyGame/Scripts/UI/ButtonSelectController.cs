using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class ButtonSelectController
{
    /// <summary>
    /// �{�^�����A�N�e�B�u�ɂȂ����Ƃ���ԏ�̃{�^����I�����Ă����Ԃɂ���
    /// </summary>
    /// <param name="window">�{�^���̐e�I�u�W�F�N�g</param>
    public static void OnButtonFirstSelect(GameObject content)
    {
        EventSystem.current.SetSelectedGameObject(content.transform.GetChild(0).gameObject);
    }


    /// <summary>
    /// �{�^���̑I����Ԃ�������(�����I�����Ă��Ȃ����)�ɂ���
    /// </summary>
    public static void OnButtonNonSelect()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}