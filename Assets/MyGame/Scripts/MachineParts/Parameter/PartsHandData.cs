using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PartsHandData : ScriptableObject
{
    public int ID;
    public string Name;
    public int ModelID;
    public float AimSpeed;
    public float ReloadSpeed;
    public float EnergyConsumption;
}
