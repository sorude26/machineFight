using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
/// <summary>
/// 機体の全体パラメータ
/// </summary>
public struct TotalParam
{
    public static readonly int PARAM_NUM = Enum.GetValues(typeof(ParamPattern)).Length;
    public float ArmorHP;
    public float Energy;
    public float Generator;
    public float Recovery;
    public float AttackPower;
    public float Ammunition;
    public float RWeaponAim;
    public float LWeaponAim;
    public float LockOnRange;
    public float LockOnSpeed;
    public float MoveSpeed;
    public float JumpPower;
    public float FlySpeed;
    public float JetSpeed;
    public float EnergyConsumption;
    public TotalParam(PartsBodyData bodyData,PartsHeadData headData,PartsHandData handDataR, PartsHandData handDataL, PartsLegData legData,
        PartsBackPackData boosterData, PartsWeaponData rWeapon, PartsWeaponData lWeapon)
    {
        ArmorHP = bodyData.Hp;
        ArmorHP += handDataL.PartsHp;
        ArmorHP += handDataR.PartsHp;
        ArmorHP += legData.PartsHp;
        ArmorHP += headData.PartsHp;
        EnergyConsumption = boosterData.EnergyConsumption;
        EnergyConsumption += headData.EnergyConsumption;
        EnergyConsumption += handDataL.EnergyConsumption;
        EnergyConsumption += handDataR.EnergyConsumption;
        EnergyConsumption += legData.EnergyConsumption;
        Generator = bodyData.Generator / boosterData.UseGeneratorPower;
        Energy = bodyData.Energy + boosterData.AdditionEnergy;
        Recovery = bodyData.GeneratorRecoverySpeed;
        FlySpeed = boosterData.Param.BoostMoveSpeed;
        FlySpeed += handDataR.AdditionalBooster + handDataL.AdditionalBooster + legData.Param.BoostMoveSpeed;
        JetSpeed = boosterData.Param.JetPower;
        JetSpeed += handDataR.AdditionalBooster + handDataL.AdditionalBooster + legData.Param.BoostJetPower;
        AttackPower = boosterData.AttackPower;
        AttackPower += handDataL.AttackPower;
        AttackPower += handDataR.AttackPower;
        Ammunition = boosterData.Ammunition;
        if (handDataL.UseWeapon == false)
        {
            AttackPower += lWeapon.AttackPower;
            Ammunition += lWeapon.Param.MaxAmmunitionCapacity;
            EnergyConsumption += lWeapon.EnergyConsumption;
        }
        else
        {
            Ammunition += handDataL.WeaponParam.MaxAmmunitionCapacity;
        }
        if (handDataR.UseWeapon == false)
        {
            AttackPower += rWeapon.AttackPower;
            Ammunition += rWeapon.Param.MaxAmmunitionCapacity;
            EnergyConsumption += rWeapon.EnergyConsumption;
        }
        else
        {
            Ammunition += handDataR.WeaponParam.MaxAmmunitionCapacity;
        }
        LockOnRange = headData.LockOnRange;
        LockOnSpeed = headData.LockOnSpeed;
        MoveSpeed = legData.Param.WalkSpeed;
        JumpPower = legData.Param.JumpPower;
        RWeaponAim = handDataR.AimSpeed;
        LWeaponAim = handDataL.AimSpeed;
    }
    
    public enum ParamPattern
    {
        ArmorHP,
        Energy,
        Generator,
        Recovery,
        AttackPower,
        Ammunition,
        RWeaponAim,
        LWeaponAim,
        LockOnRange,
        LockOnSpeed,
        MoveSpeed,
        JumpPower,
        FlySpeed,
        JetSpeed,
        EnergyConsumption,
    }
    public float this[ParamPattern num]
    {
        get
        {
            switch (num)
            {
                case ParamPattern.ArmorHP:
                    return ArmorHP;
                case ParamPattern.Energy:
                    return Energy;
                case ParamPattern.Generator:
                    return Generator;
                case ParamPattern.AttackPower:
                    return AttackPower;
                case ParamPattern.Ammunition:
                    return Ammunition;
                case ParamPattern.Recovery:
                    return Recovery;
                case ParamPattern.RWeaponAim:
                    return RWeaponAim;
                case ParamPattern.LWeaponAim:
                    return LWeaponAim;
                case ParamPattern.LockOnRange:
                    return LockOnRange;
                case ParamPattern.LockOnSpeed:
                    return LockOnSpeed;
                case ParamPattern.MoveSpeed:
                    return MoveSpeed;
                case ParamPattern.JumpPower:
                    return JumpPower;
                case ParamPattern.FlySpeed:
                    return FlySpeed;
                case ParamPattern.JetSpeed:
                    return JetSpeed;
                case ParamPattern.EnergyConsumption:
                    return EnergyConsumption;
                default:
                    return 0;
            }
        }
    }
}
