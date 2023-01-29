using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Parts/Hand")]
public class PartsHandData : ScriptableObject,IPartsData
{
    public int ID;
    public string Name;
    public int ModelID;
    public int PartsHp;
    public float AimSpeed;
    public float ReloadSpeed;
    public float EnergyConsumption;
    public float AdditionalBooster;
    public bool UseWeapon;
    public float AttackPower;
    public WeaponParam WeaponParam;
    public int PartsID => ID;
    public string PartName => Name;
    public int Model => ModelID;
}
