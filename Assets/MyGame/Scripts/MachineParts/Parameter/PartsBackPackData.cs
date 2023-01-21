using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Parts/BackPack")]
public class PartsBackPackData : ScriptableObject , IPartsData
{
    public int ID;
    public string Name;
    public int ModelID;
    public BodyParam Param;
    public float EnergyConsumption;
    public float AdditionEnergy;
    public float UseGeneratorPower;
    public int AttackPower;
    public int Ammunition;

    public int PartsID => ID;
    public string PartName => Name;
    public int Model => ModelID;
}
