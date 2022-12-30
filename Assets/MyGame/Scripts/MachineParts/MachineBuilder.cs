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
    public PartsBuildParam BuildData { get => _buildParam; }
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
        modelID.ColorId = buildPattern.ColorId;
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
        IPartsModel[] parts = { head, body, larm, rarm, leg, backPack, lweapon, rweapon };
        foreach (var par in parts)
        {
            par.ChangeColor(_buildData.ColorId);
        }
        Body = body;
        Leg = leg;
        SetParam();
    }
    private void SetParam()
    {
        var boosterData = PartsManager.Instance.AllParamData.GetPartsBack(_buildParam.Booster);
        var bodyData = PartsManager.Instance.AllParamData.GetPartsBody(_buildParam.Body);
        var headData = PartsManager.Instance.AllParamData.GetPartsHead(_buildParam.Head);
        var handDataL = PartsManager.Instance.AllParamData.GetPartsHand(_buildParam.LHand);
        var handDataR = PartsManager.Instance.AllParamData.GetPartsHand(_buildParam.RHand);
        var legData = PartsManager.Instance.AllParamData.GetPartsLeg(_buildParam.Leg);
        var lWeapon = PartsManager.Instance.AllParamData.GetPartsWeapon(_buildParam.LWeapon);
        var rWeapon = PartsManager.Instance.AllParamData.GetPartsWeapon(_buildParam.RWeapon);
        TotalParam param = new TotalParam(bodyData, headData, handDataR, handDataL, legData, boosterData, rWeapon, lWeapon);
        var bodyParam = boosterData.Param;
        bodyParam.UpPower += legData.Param.BoostUpPower;
        BoosterConsumption = boosterData.UseGeneratorPower;
        EnergyConsumption = param.EnergyConsumption;
        MaxBooster = bodyData.Generator;
        MaxEnergy = param.Energy;
        BoosterRecoverySpeed = bodyData.GeneratorRecoverySpeed;
        bodyParam.BoostMoveSpeed = param.FlySpeed;
        bodyParam.JetPower = param.JetSpeed;
        bodyParam.Hp = (int)param.ArmorHP;
        Body.SetParam(bodyParam, handDataL, handDataR);
        Leg.SetLegParam(legData.Param);
    }
}
