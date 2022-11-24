using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PartsWeaponData : ScriptableObject
{
    public int ID;
    public string Name;
    public int ModelID;
    public WeaponParam Param;
}

[System.Serializable]
public struct WeaponParam
{
    [Header("ˆĞ—Í")]
    public int Power;
    [Header("‘¬“x")]
    public float Speed;
    [Header("Å‘å‘•’e”")]
    public int MaxAmmunitionCapacity;
    [Header("ƒ}ƒKƒWƒ“‘•’e”")]
    public int MagazineCount;
    [Header("˜AË”")]
    public int ShotCount;
    [Header("ŠgU”")]
    public int SubCount;
    [Header("ƒgƒŠƒK[ƒCƒ“ƒ^[ƒoƒ‹")]
    public float TriggerInterval;
    [Header("ËŒ‚ƒCƒ“ƒ^[ƒoƒ‹")]
    public float ShotInterval;
    [Header("ËŒ‚ŠgU—¦")]
    public float Diffusivity;
    [Header("”š•—ˆĞ—Í")]
    public int ExPower;
    [Header("”š•—‰ñ”")]
    public int ExCount;
    [Header("”š•—”ÍˆÍ")]
    public int ExRadius;
}
