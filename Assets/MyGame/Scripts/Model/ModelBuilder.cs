using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelBuilder : MonoBehaviour
{
    private GameObject _modelBase = default;
    /// <summary>
    /// データを受け取りModelを表示する
    /// </summary>
    /// <param name="buildData"></param>
    public void ViewModel(PartsBuildParam buildData)
    {
        if (_modelBase != null)
        {
            DeleteModelBase();
        }
        CreateModelBase();
        SetBuildPattern(buildData);
    }
    private void CreateModelBase()
    {
        var baseObj = new GameObject("ModelBase");
        baseObj.transform.position = new Vector3(6, -3, 0);
        baseObj.transform.rotation = Quaternion.Euler(0, 200, 0);
        _modelBase = baseObj;
    }
    private void DeleteModelBase()
    {
        Destroy(_modelBase);
        _modelBase = null;
    }
    /// <summary>
    /// ビルドのパターンからModelIDを取り出し組み立てを行う
    /// </summary>
    /// <param name="buildPattern"></param>
    private void SetBuildPattern(PartsBuildParam buildPattern)
    {
        var modelID = new PartsBuildParam();
        modelID.Head = PartsManager.Instance.AllParamData.GetPartsHead(buildPattern.Head).ModelID;
        modelID.Body = PartsManager.Instance.AllParamData.GetPartsBody(buildPattern.Body).ModelID;
        modelID.RHand = PartsManager.Instance.AllParamData.GetPartsHand(buildPattern.RHand).ModelID;
        modelID.LHand = PartsManager.Instance.AllParamData.GetPartsHand(buildPattern.LHand).ModelID;
        modelID.Leg = PartsManager.Instance.AllParamData.GetPartsLeg(buildPattern.Leg).ModelID;
        modelID.Booster = PartsManager.Instance.AllParamData.GetPartsBack(buildPattern.Booster).ModelID;
        modelID.LWeapon = PartsManager.Instance.AllParamData.GetPartsWeapon(buildPattern.LWeapon).ModelID;
        modelID.RWeapon = PartsManager.Instance.AllParamData.GetPartsWeapon(buildPattern.RWeapon).ModelID;
        Build(modelID);
    }
    /// <summary>
    /// 組み立て処理
    /// </summary>
    /// <param name="buildData"></param>
    private void Build(PartsBuildParam buildData)
    {
        var leg = Instantiate(PartsManager.Instance.AllModelData.GetPartsLeg(buildData.Leg));
        leg.transform.SetParent(_modelBase.transform);
        leg.transform.localPosition = Vector3.zero;
        leg.transform.localRotation = Quaternion.identity;
        var body = Instantiate(PartsManager.Instance.AllModelData.GetPartsBody(buildData.Body));
        body.transform.SetParent(leg.BodyJoint);
        body.transform.position = leg.BodyJoint.position;
        body.transform.rotation = leg.BodyJoint.rotation;
        body.BodyBase = leg.BodyJoint;
        var head = Instantiate(PartsManager.Instance.AllModelData.GetPartsHead(buildData.Head));
        head.transform.SetParent(body.HeadJoint);
        head.transform.localPosition = Vector3.zero;
        head.transform.localRotation = Quaternion.identity;
        var larm = Instantiate(PartsManager.Instance.AllModelData.GetPartsHand(buildData.LHand));
        var lweapon = Instantiate(PartsManager.Instance.AllModelData.GetWeapon(buildData.LWeapon));
        larm.SetWeapon(lweapon);
        var rarm = Instantiate(PartsManager.Instance.AllModelData.GetPartsHand(buildData.RHand));
        var rweapon = Instantiate(PartsManager.Instance.AllModelData.GetWeapon(buildData.RWeapon));
        rarm.SetWeapon(rweapon);
        body.SetHands(larm, rarm);
        var backPack = Instantiate(PartsManager.Instance.AllModelData.GetBackPack(buildData.Booster));
        body.SetBackPack(backPack);
        body.AddBooster(leg.LegBoost);
        IPartsModel[] parts = { head, body, larm, rarm, leg, backPack ,lweapon, rweapon};
        foreach (var p in parts)
        {
            p.ChangeColor(buildData.ColorId);
        }
    }
}
