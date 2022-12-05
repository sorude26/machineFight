using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Parts/Hand")]
public class PartsHandData : ScriptableObject
{
    public int ID;
    public string Name;
    public int ModelID;
    public int PartsHp;
    public float AimSpeed;
    public float ReloadSpeed;
    public float EnergyConsumption;
    public float AdditionalBooster;
    public WeaponParam WeaponParam;
}
