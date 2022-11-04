using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsManager
{
    private static PartsManager instance;
    public static PartsManager Instance 
    { 
        get 
        {
            if (instance == null)
            {
                instance = new PartsManager();
            }
            return instance;
        }
    }
    private PartsModelData _modelData = default;
    private PartsParamData _paramData = default;
    private bool _isLoaded = false;
    public PartsModelData AllModelData { get { return _modelData; } }
    public PartsParamData AllParamData { get { return _paramData; } }
    public void LoadData()
    {
        if (_isLoaded == true) { return; }
        var modelData = Resources.Load<PartsModelData>("ScriptableObjects/PartsModelData");
        var paramData = Resources.Load<PartsParamData>("ScriptableObjects/PartsParamData");
        if (modelData != null && paramData != null)
        {
            instance._modelData = modelData;
            instance._paramData = paramData;
            _isLoaded = true;
        }
    }
}
