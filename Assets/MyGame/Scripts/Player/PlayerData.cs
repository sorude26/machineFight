using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UniRx.Triggers;
using UniRx;

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;

    public static PlayerData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerData();
                instance.PresetLoad();
            }
            return instance;
        }
    }
    private PartsBuildParam _buildPreset = default;
    public ReactiveProperty<PartsBuildParam> BuildPreset = new ReactiveProperty<PartsBuildParam>();

    private void Start()
    {
        BuildPreset.Value = _buildPreset;
        BuildPreset.Subscribe(_ => { 
            Debug.Log(BuildPreset.Value.Head);
            Debug.Log(BuildPreset.Value.Body);
            Debug.Log(BuildPreset.Value.LHand);
            Debug.Log(BuildPreset.Value.RHand);
            Debug.Log(BuildPreset.Value.Leg);
            Debug.Log(BuildPreset.Value.Booster);
            Debug.Log(BuildPreset.Value.LWeapon);
            Debug.Log(BuildPreset.Value.RWeapon);
        }).AddTo(this);
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
}
