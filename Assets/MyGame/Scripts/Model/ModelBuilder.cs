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
        //baseObj.transform.position = this.gameObject.transform.position;
        //baseObj.transform.rotation = this.gameObject.transform.rotation;
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
        modelID.LWeapon = buildPattern.LWeapon;
        modelID.RWeapon = buildPattern.RWeapon;
        Build(modelID);
    }
    /// <summary>
    /// 組み立て処理
    /// </summary>
    /// <param name="_buildData"></param>
    private void Build(PartsBuildParam _buildData)
    {
        var leg = Instantiate(PartsManager.Instance.AllModelData.GetPartsLeg(_buildData.Leg));
        leg.transform.SetParent(_modelBase.transform);
        leg.transform.localPosition = Vector3.zero;
        leg.transform.localRotation = Quaternion.identity;
        var body = Instantiate(PartsManager.Instance.AllModelData.GetPartsBody(_buildData.Body));
        body.transform.SetParent(_modelBase.transform);
        body.transform.position = leg.BodyJoint.position;
        body.transform.rotation = leg.BodyJoint.rotation;
        body.BodyBase = leg.BodyJoint;
        var head = Instantiate(PartsManager.Instance.AllModelData.GetPartsHead(_buildData.Head));
        head.transform.SetParent(body.HeadJoint);
        head.transform.localPosition = Vector3.zero;
        head.transform.localRotation = Quaternion.identity;
        var larm = Instantiate(PartsManager.Instance.AllModelData.GetPartsHand(_buildData.LHand));
        var lweapon = Instantiate(PartsManager.Instance.AllModelData.GetWeapon(_buildData.LWeapon));
        larm.SetWeapon(lweapon);
        var rarm = Instantiate(PartsManager.Instance.AllModelData.GetPartsHand(_buildData.RHand));
        var rweapon = Instantiate(PartsManager.Instance.AllModelData.GetWeapon(_buildData.RWeapon));
        rarm.SetWeapon(rweapon);
        body.SetHands(larm, rarm);
        var backPack = Instantiate(PartsManager.Instance.AllModelData.GetBackPack(_buildData.Booster));
        body.SetBackPack(backPack);
        body.AddBooster(leg.LegBoost);
    }
}
