using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadController : MonoBehaviour, IPartsModel
{
    [SerializeField]
    private int _id = default;
    public int ID { get => _id; }
    [SerializeField]
    private PartsColorChanger _partsColorChanger = default;
    public void ChangeColor(int id)
    {
        if (_partsColorChanger != null)
        {
            _partsColorChanger.ChangeColor(id);
        }
    }
}
