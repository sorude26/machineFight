using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PartsHeadData : ScriptableObject
{
    public int ID;
    public string Name;
    public int ModelID;
    public float LockOnRange;
    public float LockOnSpeed;
    public float EnergyConsumption;
}
