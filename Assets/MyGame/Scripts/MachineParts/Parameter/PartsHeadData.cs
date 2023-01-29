using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parts/Head")]
public class PartsHeadData : ScriptableObject,IPartsData
{
    public int ID;
    public string Name;
    public int ModelID;
    public int PartsHp;
    public float LockOnRange;
    public float LockOnSpeed;
    public float EnergyConsumption;
    public int PartsID => ID;
    public string PartName => Name;
    public int Model => ModelID;
}