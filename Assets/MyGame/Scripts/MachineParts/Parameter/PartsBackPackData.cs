using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PartsBackPackData : ScriptableObject
{
    public int ID;
    public string Name;
    public int ModelID;
    public BodyParam Param;
    public float EnergyConsumption;
    public float UseGeneratorPower;
}
