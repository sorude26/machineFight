using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class PlayerVrCockpit : MonoBehaviour
{
    private static PlayerVrCockpit _instance;
    public static PlayerVrCockpit Instance => _instance;

    [SerializeField]
    FlightStick _flightStick;

    private void Awake()
    {
        _instance = this;
        var layer = this.gameObject.layer;
        SetLayerToChildlen(layer, this.transform);
    }

    private void SetLayerToChildlen(int layer, Transform t)
    {
        foreach (Transform item in t)
        {
            item.gameObject.layer = layer;
            //�ċA�����ł��ׂĂ̎q�I�u�W�F�N�g�ɓK�p
            SetLayerToChildlen(layer, item);
        }
    }
}
