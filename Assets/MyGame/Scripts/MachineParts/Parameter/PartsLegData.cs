using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(menuName = "Parts/Leg")]
public class PartsLegData : ScriptableObject
{
    public int ID;
    public string Name;
    public int ModelID;
    public int PartsHp;
    public float EnergyConsumption;
    public LegActionParam Param;
}
