using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// スイッチの状態によって色が変わるライト
/// </summary>
public class SwitchUiLight : MonoBehaviour
{
    [SerializeField]
    Switch _switch;
    Material _material;


    private void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
        if (_switch.IsOn)
        {
            TurnGreen();
        }
        else
        {
            TurnRed();
        }

        _switch.OnTurnOn += TurnGreen;
        _switch.OnTurnOff += TurnRed;
        _switch.OnInit += () =>
        {
            if (_switch.IsOn)
            {
                TurnGreen();
            }
            else
            {
                TurnRed();
            }
        };
    }

    private void TurnGreen()
    {
        _material.color = Color.green;
    }

    private void TurnRed()
    {
        _material.color = Color.red;
    }
}
