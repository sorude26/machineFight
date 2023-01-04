using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class FloorEvent : MonoBehaviour
{
    [SerializeField]
    private Text _text = default;
    [SerializeField]
    private GameObject _clearMark = default;
    [SerializeField]
    private GameObject _clossMark = default;
    private int floorID;
    public event Action<int> OnFloorClear = default;

    public void FloorClear()
    {
        OnFloorClear?.Invoke(floorID);
        if (_clearMark != null)
        {
            _clearMark.SetActive(true);
        }
        if (_clossMark != null)
        {
            _clossMark.SetActive(false);
        }
    }
    public void SetID(int id)
    {
        floorID = id;
        if(_text != null)
        {
            _text.text = $"{id}";
        }
    }
}
