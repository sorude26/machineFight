using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWeaponView : MonoBehaviour
{
    const string STANDBY_TEXT = "STAND BY";
    const string READY_TEXT = "READY";
    const string NOCONNECTION_TEXT = "NO CONNECTION";

    [SerializeField]
    Text[] _weaponCondition;
    [SerializeField]
    Text[] _weaponNum;
    [SerializeField]
    Text[] _weaponAllNum;

    bool[] _onOff = new bool[3];

    
}
