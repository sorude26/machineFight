using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu]
public class PartsLegData : ScriptableObject
{
    public int ID;
    public string Name;
    public int ModelID;
    public float EnergyConsumption;
    public LegActionParam Param;
}
