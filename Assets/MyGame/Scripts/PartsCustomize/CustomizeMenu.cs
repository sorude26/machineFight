using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeMenu : MonoBehaviour
{
    [SerializeField] Text _partsName = default;
    [SerializeField] Button _part = default;

    public void ButtonInstance(PlayerData.PartsCategory category)
    {
        int id = 0;
        while(PartsManager.Instance.AllParamData.GetPartsHead(id) != null)
        {

        }
    }
    
}
