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
    public BodyController Body { get; private set; }
    public LegController Leg { get; private set; }
    public void Build(PartsBuildParam buildPattern)
    {
        var modelID = new PartsBuildParam();
        modelID.Head = PartsManager.Instance.AllParamData.GetPartsHead(buildPattern.Head).ModelID;
        modelID.Body = PartsManager.Instance.AllParamData.GetPartsBody(buildPattern.Body).ModelID;
        modelID.RHand = PartsManager.Instance.AllParamData.GetPartsHand(buildPattern.RHand).ModelID;
        modelID.LHand = PartsManager.Instance.AllParamData.GetPartsHand(buildPattern.LHand).ModelID;
        modelID.Leg = PartsManager.Instance.AllParamData.GetPartsLeg(buildPattern.Leg).ModelID;
        modelID.Booster = PartsManager.Instance.AllParamData.GetPartsBack(buildPattern.Booster).ModelID;
        modelID.LWeapon = buildPattern.LWeapon;
        modelID.RWeapon = buildPattern.RWeapon;
        _buildData = modelID;
        Build();
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
        larm.SetWeapon(lweapon);
        larm.SetLockAim(_aimTrans);
        var rarm = Instantiate(PartsManager.Instance.AllModelData.GetPartsHand(_buildData.RHand));
        var rweapon = Instantiate(PartsManager.Instance.AllModelData.GetWeapon(_buildData.RWeapon));
        rarm.SetWeapon(rweapon);
        rarm.SetLockAim(_aimTrans);
        body.SetHands(larm, rarm);
        var backPack = Instantiate(PartsManager.Instance.AllModelData.GetBackPack(_buildData.Booster));
        backPack.CameraLock = _aimTrans;
        body.SetBackPack(backPack);
        body.AddBooster(leg.LegBoost);
        Body = body;
        Leg = leg;
        var bodyParam = PartsManager.Instance.AllParamData.GetPartsBack(_buildData.Booster).Param;
        bodyParam.Hp = PartsManager.Instance.AllParamData.GetPartsBody(_buildData.Body).Hp;
        Body.SetParam(bodyParam);
        Leg.SetLegParam(PartsManager.Instance.AllParamData.GetPartsLeg(_buildData.Leg).Param);
    }
}
