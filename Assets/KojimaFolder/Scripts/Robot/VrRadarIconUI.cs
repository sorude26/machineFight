using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VrRadarIconUI : MonoBehaviour
{
    [SerializeField]
    Color _white;
    [SerializeField]
    Color _red;
    [SerializeField]
    Image _image;

    private void Awake()
    {
        this.gameObject.SetActive(false);
    }
    public void Show(float time)
    {
        this.gameObject.SetActive(true);
        StartCoroutine(ShowforSeconds(time));
    }
    public void SetColorToRed()
    {
        _image.color = _red;
    }

    public void SetColorToWhite()
    {
        _image.color = _white;
    }

    IEnumerator ShowforSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        this.gameObject.SetActive(false);
    }
}
