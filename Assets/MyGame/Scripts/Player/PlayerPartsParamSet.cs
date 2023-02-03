using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerPartsParamSet : MonoBehaviour
{
    [SerializeField]
    MachineDataView _machineDataView;

    public void ParamSet(TotalParam totalParam)
    {
        if (GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(this.gameObject) > 0)
        {
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(this.gameObject);
        }
        if (_machineDataView == null)
        {
            _machineDataView = GameObject.Find("/DataCanvas").GetComponent<MachineDataView>();
        }
        _machineDataView.ViewData(totalParam);
    }
}
