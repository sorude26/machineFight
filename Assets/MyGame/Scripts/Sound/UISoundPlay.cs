using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundPlay : MonoBehaviour
{
    
    public void OnMenuSubmit()
    {
        SoundManager.Instance.PlaySE(25);
    }

    public void OnPartsSubmit()
    {
        SoundManager.Instance.PlaySE(26);
    }

    public void OnSelected()
    {
        SoundManager.Instance.PlaySE(27);
    }

    public void OnCancel()
    {
        SoundManager.Instance.PlaySE(24);
    }
}
