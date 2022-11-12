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
    //JsonÇ…èëÇ´çûÇ›
    public void PresetSave()
    {
        
    }

    //JsonÇ©ÇÁì«Ç›çûÇ›
    public void PresetLoad()
    {

    }

    
}
