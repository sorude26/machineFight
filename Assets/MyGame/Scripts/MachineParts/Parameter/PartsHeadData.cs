using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parts/Head")]
public class PartsHeadData : ScriptableObject
{
    public int ID;
    public string Name;
    public int ModelID;
    public int PartsHp;
    public float LockOnRange;
    public float LockOnSpeed;
    public float EnergyConsumption;
}
