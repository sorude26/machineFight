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
    private bool _isLoaded = false;
    public PartsModelData AllData { get { return _modelData; } }
    public void LoadData()
    {
        if (_isLoaded == true) { return; }
        var modelData = Resources.Load<PartsModelData>("ScriptableObjects/PartsModelData");
        if (modelData != null)
        {
            instance._modelData = modelData;
            _isLoaded = true;
        }
    }
}
