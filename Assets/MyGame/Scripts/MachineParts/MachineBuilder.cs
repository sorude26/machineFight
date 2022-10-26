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
    public void Build()
    {
        var leg = Instantiate(PartsManager.Instance.AllData.GetPartsLeg(_buildData.Leg));
        leg.transform.SetParent(transform);
        leg.transform.localPosition = transform.localPosition;
        leg.transform.localRotation = transform.localRotation;
        leg.SetIkTargetBase(_machineBase);
        leg.LockTrans = _lockTrans;
        var body = Instantiate(PartsManager.Instance.AllData.GetPartsBody(_buildData.Body));
        body.transform.SetParent(transform);
        body.transform.position = leg.BodyJoint.position;
        body.transform.rotation = leg.BodyJoint.rotation;
        body.BodyBase = leg.BodyJoint;
        body.Lock = _lockTrans;
        var head = Instantiate(PartsManager.Instance.AllData.GetPartsHead(_buildData.Head));
        head.transform.SetParent(body.HeadJoint);
        head.transform.localPosition = Vector3.zero;
        head.transform.localRotation = Quaternion.identity;
        _lockTrans.position = head.transform.position;
        var larm = Instantiate(PartsManager.Instance.AllData.GetPartsHand(_buildData.LHand));
        var lweapon = Instantiate(PartsManager.Instance.AllData.GetWeapon(_buildData.LWeapon));
        larm.SetWeapon(lweapon);
        larm.SetLockAim(_aimTrans);
        var rarm = Instantiate(PartsManager.Instance.AllData.GetPartsHand(_buildData.RHand));
        var rweapon = Instantiate(PartsManager.Instance.AllData.GetWeapon(_buildData.RWeapon));
        rarm.SetWeapon(rweapon);
        rarm.SetLockAim(_aimTrans);
        body.SetHands(larm, rarm);
        var booster = Instantiate(PartsManager.Instance.AllData.GetBooster(_buildData.Booster));
        body.SetBooster(booster);
        body.AddBooster(leg.LegBoost);
        Body = body;
        Leg = leg;
    }
}
