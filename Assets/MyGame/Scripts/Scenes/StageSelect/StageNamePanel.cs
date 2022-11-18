using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageNamePanel : MonoBehaviour
{
    [SerializeField]
    private GameObject _baseObj = default;
    [SerializeField]
    private Text[] _stageName = default;
    public void SetPanel(string name, Quaternion angle, int number)
    {
        foreach (var item in _stageName)
        {
            item.text = name;
        }
        _baseObj.transform.position = Vector3.back * number * 2;
        transform.rotation = angle;
    }
}
