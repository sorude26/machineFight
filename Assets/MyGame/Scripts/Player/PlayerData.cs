using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;

    private PartsBuildParam _buildPreset = default;
    public PartsBuildParam BuildPreset
    {
        get
        {
            return _buildPreset;
        }
        set
        {
            _buildPreset = value;
        }
    }

    private ModelBuilder _modelBuilder = default;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            instance._buildPreset = instance.PresetLoad();
            instance._modelBuilder = new ModelBuilder();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        instance.Build();
    }
    //Jsonに書き込み
    public void PresetSave()
    {
        
    }

    //Jsonから読み込み
    public PartsBuildParam PresetLoad()
    {
        PartsBuildParam buildPreset = new PartsBuildParam();
        buildPreset.Head = 0;
        buildPreset.Body = 0;
        buildPreset.LHand = 0;
        buildPreset.RHand = 0;
        buildPreset.Leg = 0;
        buildPreset.Booster = 0;
        buildPreset.LWeapon = 0;
        buildPreset.RWeapon = 0;
        return buildPreset;
    }

    /// <summary>
    /// 現在のパーツのログを流す
    /// </summary>
    public void PartsLog()
    {
        Debug.Log("Head:" + BuildPreset.Head);
        Debug.Log("Body:" + BuildPreset.Body);
        Debug.Log("LHand:" + BuildPreset.LHand);
        Debug.Log("RHand:" + BuildPreset.RHand);
        Debug.Log("Leg:" + BuildPreset.Leg);
        Debug.Log("Booster:" + BuildPreset.Booster);
        Debug.Log("LWeapon:" + BuildPreset.LWeapon);
        Debug.Log("RWeapon:" + BuildPreset.RWeapon);
    }

    public void Build()
    {
        _modelBuilder.ViewModel(BuildPreset);
    }
}
