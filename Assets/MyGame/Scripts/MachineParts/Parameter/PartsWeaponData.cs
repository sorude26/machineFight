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
    [Header("�З�")]
    public int Power;
    [Header("���x")]
    public float Speed;
    [Header("�ő呕�e��")]
    public int MaxAmmunitionCapacity;
    [Header("�}�K�W�����e��")]
    public int MagazineCount;
    [Header("�A�ː�")]
    public int ShotCount;
    [Header("�g�U��")]
    public int SubCount;
    [Header("�g���K�[�C���^�[�o��")]
    public float TriggerInterval;
    [Header("�ˌ��C���^�[�o��")]
    public float ShotInterval;
    [Header("�ˌ��g�U��")]
    public float Diffusivity;
    [Header("�����З�")]
    public int ExPower;
    [Header("������")]
    public int ExCount;
    [Header("�����͈�")]
    public int ExRadius;
    [Header("�ǔ����񑬓x�i�U���e�̂݁j")]
    public float HomingSpeed;
    [Header("�ǔ��J�n���ԁi�U���e�̂݁j")]
    public float HomingStartTime;
    [Header("�ǔ��I�����ԁi�U���e�̂݁j")]
    public float HomingEndTime;
}
