using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextColorChanger : MonoBehaviour
{
    [SerializeField]
    private Color _startColor = Color.white;
    [SerializeField]
    private Color _changeColor = Color.white;
    [SerializeField]
    private UnityEngine.UI.Text _text;
    public void OnSelect()
    {
        _text.color = _changeColor;
    }
    public void OnDeselect()
    {
        _text.color = _startColor;
    }
}
