using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parts/Body")]
public class PartsBodyData : ScriptableObject,IPartsData
{
    public int ID;
    public string Name;
    public int ModelID;
    public int Hp;
    public float Energy;
    public float Generator;
    public float GeneratorRecoverySpeed;
    public int PartsID => ID;
    public string PartName => Name;
    public int Model => ModelID;
}
