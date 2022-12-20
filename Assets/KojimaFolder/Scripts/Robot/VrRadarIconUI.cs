using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VrRadarIconUI : MonoBehaviour
{
    private void Awake()
    {
        this.gameObject.SetActive(false);
    }
    public void Show(float time)
    {
        this.gameObject.SetActive(true);
        StartCoroutine(ShowforSeconds(time));
    }

    IEnumerator ShowforSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        this.gameObject.SetActive(false);
    }
}
