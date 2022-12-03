using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineBuilder : MonoBehaviour
{
    [SerializeField]
    private PartsBuildParam _buildData = default;
    [SerializeField]
    private Transform _machineBase = default;
    [SerializeField]
    private Transform _lockTrans = default;
    [SerializeField]
    private Transform _aimTrans = default;
    [SerializeField]
    private BodyController _anBuildBody = default;
    [SerializeField]
    private LegController _anBuildLeg = default;
    [SerializeField]
    private BackPackController _anBuildBackPack = default;
    private PartsBuildParam _buildParam = default;
    public BodyController Body { get; private set; }
    public LegController Leg { get; private set; }
    public PartsBuildParam BuildData { get => _buildData; }
    public float MaxBooster { get; private set; }
    public float BoosterRecoverySpeed { get; private set; }
    public float MaxEnergy { get; private set; }
    public float EnergyConsumption { get; private set; }
    public float BoosterConsumption { get; private set; }
    public void Build(PartsBuildParam buildPattern)
    {
        if (_anBuildBackPack != null && _anBuildBody != null && _anBuildLeg != null)
        {
            AnBuildSet();
            return;
        }
        var modelID = new PartsBuildParam();
        modelID.Head = PartsManager.Instance.AllParamData.GetPartsHead(buildPattern.Head).ModelID;
        modelID.Body = PartsManager.Instance.AllParamData.GetPartsBody(buildPattern.Body).ModelID;
        modelID.RHand = PartsManager.Instance.AllParamData.GetPartsHand(buildPattern.RHand).ModelID;
        modelID.LHand = PartsManager.Instance.AllParamData.GetPartsHand(buildPattern.LHand).ModelID;
        modelID.Leg = PartsManager.Instance.AllParamData.GetPartsLeg(buildPattern.Leg).ModelID;
        modelID.Booster = PartsManager.Instance.AllParamData.GetPartsBack(buildPattern.Booster).ModelID;
        modelID.LWeapon = PartsManager.Instance.AllParamData.GetPartsWeapon(buildPattern.LWeapon).ModelID;
        modelID.RWeapon = PartsManager.Instance.AllParamData.GetPartsWeapon(buildPattern.RWeapon).ModelID;
        _buildData = modelID;
        _buildParam = buildPattern;
        Build();
    }
    public void AnBuildSet()
    {
        _anBuildBody.SetBackPack(_anBuildBackPack);
        _anBuildBody.AddBooster(_anBuildLeg.LegBoost);
        Body = _anBuildBody;
        Leg = _anBuildLeg;
    }
    public void Build()
    {
        var leg = Instantiate(PartsManager.Instance.AllModelData.GetPartsLeg(_buildData.Leg));
        leg.transform.SetParent(transform);
        leg.transform.localPosition = transform.localPosition;
        leg.transform.localRotation = transform.localRotation;
        leg.SetIkTargetBase(_machineBase);
        leg.LockTrans = _lockTrans;
        var body = Instantiate(PartsManager.Instance.AllModelData.GetPartsBody(_buildData.Body));
        body.transform.SetParent(transform);
        body.transform.position = leg.BodyJoint.position;
        body.transform.rotation = leg.BodyJoint.rotation;
        body.BodyBase = leg.BodyJoint;
        body.Lock = _lockTrans;
        var head = Instantiate(PartsManager.Instance.AllModelData.GetPartsHead(_buildData.Head));
        head.transform.SetParent(body.HeadJoint);
        head.transform.localPosition = Vector3.zero;
        head.transform.localRotation = Quaternion.identity;
        _lockTrans.position = head.transform.position;
        var larm = Instantiate(PartsManager.Instance.AllModelData.GetPartsHand(_buildData.LHand));
        var lweapon = Instantiate(PartsManager.Instance.AllModelData.GetWeapon(_buildData.LWeapon));
        lweapon.SetParam(PartsManager.Instance.AllParamData.GetPartsWeapon(_buildParam.LWeapon).Param);
        larm.SetWeapon(lweapon);
        larm.SetLockAim(_aimTrans);
        var rarm = Instantiate(PartsManager.Instance.AllModelData.GetPartsHand(_buildData.RHand));
        var rweapon = Instantiate(PartsManager.Instance.AllModelData.GetWeapon(_buildData.RWeapon));
        rweapon.SetParam(PartsManager.Instance.AllParamData.GetPartsWeapon(_buildParam.RWeapon).Param);
        rarm.SetWeapon(rweapon);
        rarm.SetLockAim(_aimTrans);
        body.SetHands(larm, rarm);
        var backPack = Instantiate(PartsManager.Instance.AllModelData.GetBackPack(_buildData.Booster));
        backPack.CameraLock = _aimTrans;
        body.SetBackPack(backPack);
        body.AddBooster(leg.LegBoost);
        Body = body;
        Leg = leg;
        SetParam();
    }
    private void SetParam()
    {
        var boosterData = PartsManager.Instance.AllParamData.GetPartsBack(_buildData.Booster);
        var bodyData = PartsManager.Instance.AllParamData.GetPartsBody(_buildData.Body);
        var headData = PartsManager.Instance.AllParamData.GetPartsHead(_buildData.Head);
        var handDataL = PartsManager.Instance.AllParamData.GetPartsHand(_buildData.LHand);
        var handDataR = PartsManager.Instance.AllParamData.GetPartsHand(_buildData.RHand);
        var legData = PartsManager.Instance.AllParamData.GetPartsLeg(_buildData.Leg);
        var bodyParam = boosterData.Param;
        int hp = bodyData.Hp;
        hp += handDataL.PartsHp;
        hp += handDataR.PartsHp;
        hp += legData.PartsHp;
        hp += headData.PartsHp;
        BoosterConsumption = boosterData.UseGeneratorPower;
        EnergyConsumption += boosterData.EnergyConsumption;
        EnergyConsumption += headData.EnergyConsumption;
        EnergyConsumption += handDataL.EnergyConsumption;
        EnergyConsumption += handDataR.EnergyConsumption;
        EnergyConsumption += legData.EnergyConsumption;
        MaxBooster = bodyData.Generator;
        MaxEnergy = bodyData.Energy;
        BoosterRecoverySpeed = bodyData.GeneratorRecoverySpeed;
        bodyParam.BoostMoveSpeed += handDataR.AdditionalBooster + handDataL.AdditionalBooster;
        bodyParam.JetPower += handDataR.AdditionalBooster + handDataL.AdditionalBooster;
        bodyParam.Hp = hp;
        Body.SetParam(bodyParam,handDataL,handDataR);
        Leg.SetLegParam(legData.Param);
    }
}
