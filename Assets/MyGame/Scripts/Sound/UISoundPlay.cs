using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundPlay : MonoBehaviour
{
    private float _seVol = 0.5f;
    public void OnMenuSubmit()
    {
        if (SoundManager.Instance == null)
        {
            return;
        }
        SoundManager.Instance.PlaySE(25, _seVol);
    }

    public void OnPartsSubmit()
    {
        if (SoundManager.Instance == null)
        {
            return;
        }
        SoundManager.Instance.PlaySE(26, _seVol);
    }

    public void OnSelected()
    {
        if (SoundManager.Instance == null)
        {
            return;
        }
        SoundManager.Instance.PlaySE(27, _seVol);
    }

    public void OnCancel()
    {
        if (SoundManager.Instance == null)
        {
            return;
        }
        SoundManager.Instance.PlaySE(24, _seVol);
    }
}
