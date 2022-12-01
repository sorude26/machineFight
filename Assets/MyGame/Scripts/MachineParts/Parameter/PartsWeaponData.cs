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
    [Header("威力")]
    public int Power;
    [Header("速度")]
    public float Speed;
    [Header("最大装弾数")]
    public int MaxAmmunitionCapacity;
    [Header("マガジン装弾数")]
    public int MagazineCount;
    [Header("連射数")]
    public int ShotCount;
    [Header("拡散数")]
    public int SubCount;
    [Header("トリガーインターバル")]
    public float TriggerInterval;
    [Header("射撃インターバル")]
    public float ShotInterval;
    [Header("射撃拡散率")]
    public float Diffusivity;
    [Header("爆風威力")]
    public int ExPower;
    [Header("爆風回数")]
    public int ExCount;
    [Header("爆風範囲")]
    public int ExRadius;
    [Header("追尾旋回速度（誘導弾のみ）")]
    public float HomingSpeed;
    [Header("追尾開始時間（誘導弾のみ）")]
    public float HomingStartTime;
    [Header("追尾終了時間（誘導弾のみ）")]
    public float HomingEndTime;
}
