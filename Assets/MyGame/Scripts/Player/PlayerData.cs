using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

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
    private PartsBuildParam buildPreset = default;
    public PartsBuildParam BuildPreset { 
        get 
        { 
            return buildPreset; 
        }
        set
        {
            buildPreset = value;
        }
    }

    //Json�ɏ�������
    public void PresetSave()
    {
        
    }

    //Json����ǂݍ���
    public void PresetLoad()
    {

    }

    
}
