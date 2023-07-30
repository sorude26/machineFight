using MyGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelBuilder : MonoBehaviour
{
    [SerializeField]
    private MachineDataView _dataView = default; 
    [SerializeField]
    private Transform _viewPoint = default;
    [SerializeField]
    private float _maxZ = 3f;
    [SerializeField]
    private float _minZ = -1f;
    [SerializeField]
    private float _rotationSpeed = 0.1f;
    [SerializeField]
    private float _moveSpeed = 0.1f;
    [SerializeField]
    private float _inputThreshold = 0.3f;
    private GameObject _modelBase = default;
    private Quaternion _currentR = default;
    private PartsBuildParam _currentBuildData;
    private TotalParam _currentParam;
    private void FixedUpdate()
    {
        if (_modelBase != null)
        {
            if (Mathf.Abs(PlayerInput.CameraDir.x) > _inputThreshold)
            {
                _modelBase.transform.rotation *= Quaternion.Euler(0, _rotationSpeed * PlayerInput.CameraDir.x, 0);
            }
            if (Mathf.Abs(PlayerInput.CameraDir.y) > _inputThreshold)
            {
                if (PlayerInput.CameraDir.y < 0 && transform.position.z > _minZ)
                {
                    transform.position += Vector3.back * _moveSpeed;
                }
                else if(PlayerInput.CameraDir.y > 0 && transform.position.z < _maxZ)
                {
                    transform.position += Vector3.forward * _moveSpeed;
                }
            }
        }
    }
    /// <summary>
    /// データを受け取りModelを表示する
    /// </summary>
    /// <param name="buildData"></param>
    public void ViewModel(PartsBuildParam buildData)
    {
        transform.position = Vector3.zero;
        if (_modelBase != null)
        {
            DeleteModelBase();
            CreateModelBase();
            _modelBase.transform.rotation = _currentR;
        }
        else
        {
            CreateModelBase();
        }
        _currentBuildData = buildData;
        SetBuildPattern(buildData);
    }
    public void ViewChange(PartsBuildParam buildData)
    {
        transform.position = Vector3.zero;
        if (_modelBase != null)
        {
            DeleteModelBase();
            CreateModelBase();
            _modelBase.transform.rotation = _currentR;
        }
        else
        {
            CreateModelBase();
        }
        SetBuildPattern(buildData,false);
    }
    private void CreateModelBase()
    {
        var baseObj = new GameObject("ModelBase");
        baseObj.transform.position = _viewPoint.position;
        baseObj.transform.rotation = _viewPoint.rotation;
        baseObj.transform.SetParent(transform, false);
        _modelBase = baseObj;
    }
    private void DeleteModelBase()
    {
        _currentR = _modelBase.transform.rotation;
        Destroy(_modelBase);
        _modelBase = null;
    }
    /// <summary>
    /// ビルドのパターンからModelIDを取り出し組み立てを行う
    /// </summary>
    /// <param name="buildPattern"></param>
    private void SetBuildPattern(PartsBuildParam buildPattern,bool set = true)
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
        modelID.ColorId = buildPattern.ColorId;
        Build(modelID);
        if (_dataView != null)
        {
            var boosterData = PartsManager.Instance.AllParamData.GetPartsBack(buildPattern.Booster);
            var bodyData = PartsManager.Instance.AllParamData.GetPartsBody(buildPattern.Body);
            var headData = PartsManager.Instance.AllParamData.GetPartsHead(buildPattern.Head);
            var handDataL = PartsManager.Instance.AllParamData.GetPartsHand(buildPattern.LHand);
            var handDataR = PartsManager.Instance.AllParamData.GetPartsHand(buildPattern.RHand);
            var legData = PartsManager.Instance.AllParamData.GetPartsLeg(buildPattern.Leg);
            var lWeapon = PartsManager.Instance.AllParamData.GetPartsWeapon(buildPattern.LWeapon);
            var rWeapon = PartsManager.Instance.AllParamData.GetPartsWeapon(buildPattern.RWeapon);
            TotalParam param = new TotalParam(bodyData, headData, handDataR, handDataL, legData, boosterData, rWeapon, lWeapon);
            if (set == true)
            {
                _currentParam = param;
                _dataView.ViewData(param);
                return;
            }
            _dataView.ViewData(param,_currentParam);
        }
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
