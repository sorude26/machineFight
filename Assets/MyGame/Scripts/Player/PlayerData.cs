using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;
    
    private PartsBuildParam _buildPreset = default;
    private SaveDataReader saveDataReader = new SaveDataReader();
    public PartsBuildParam BuildPreset
    {
        get
        {
            return _buildPreset;
        }
        set
        {
            _buildPreset = value;
        }
    }
    //private ModelBuilder _modelBuilder = default;

    [SerializeField] 
    private List<PartsHeadData> _getsHeadParts = new List<PartsHeadData>();
    [SerializeField] 
    private List<PartsBodyData> _getsBodyParts = new List<PartsBodyData>();
    [SerializeField] 
    private List<PartsHandData> _getsLHandParts = new List<PartsHandData>();
    [SerializeField]
    private List<PartsHandData> _getsRHandParts = new List<PartsHandData>();
    [SerializeField] 
    private List<PartsLegData> _getsLegParts = new List<PartsLegData>();
    [SerializeField] 
    private List<PartsBackPackData> _getsBackPackParts = new List<PartsBackPackData>();
    [SerializeField] 
    private List<PartsWeaponData> _getsWeaponParts = new List<PartsWeaponData>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            instance.PresetLoad();
            PartsManager.Instance.LoadData();
            SceneManager.activeSceneChanged += ActiveSceneChanged;
            saveDataReader.ReadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        instance.Build();
    }
    //Jsonに書き込み
    public void PresetSave()
    {
        //Json.JsonSave(this);
        Json.SavePreset(_buildPreset);
        saveDataReader.SetData();
    }

    //Jsonから読み込み
    public void PresetLoad()
    {
        bool _isDataGet = Json.JsonLoad();
        if (_isDataGet == false)
        {
            PartsBuildParam buildData = new PartsBuildParam();
            buildData.Head = 0;
            buildData.Body = 0;
            buildData.LHand = 0;
            buildData.RHand = 0;
            buildData.Booster = 0;
            buildData.Leg = 0;
            buildData.LWeapon = 0;
            buildData.RWeapon = 0;
            buildData.ColorId = 0;
            PlayerData.instance._buildPreset = buildData;
            Debug.Log("初回");
        }
        else
        {
            Debug.Log("再起動時");
        }
    }

    /// <summary>
    /// 現在のパーツのログを流す
    /// </summary>
    public void PartsLog()
    {
        //Debug.Log("Head:" + BuildPreset.Head);
        //Debug.Log("Body:" + BuildPreset.Body);
        //Debug.Log("LHand:" + BuildPreset.LHand);
        //Debug.Log("RHand:" + BuildPreset.RHand);
        //Debug.Log("Leg:" + BuildPreset.Leg);
        //Debug.Log("Booster:" + BuildPreset.Booster);
        //Debug.Log("LWeapon:" + BuildPreset.LWeapon);
        //Debug.Log("RWeapon:" + BuildPreset.RWeapon);
        //Debug.Log("Color:" + BuildPreset.ColorId);
    }

    /// <summary>
    /// モデルを生成する
    /// </summary>
    public void Build()
    {
        if (HomeUIController.Instance == null)
        {
            return;
        }
        HomeUIController.Instance.BuildModel();
        //_modelBuilder.ViewModel(BuildPreset);
    }

    /// <summary>
    /// パーツを入手する　HandとWeaponはLHand、LWeaponのタグを使う
    /// </summary>
    /// <param name="type">パーツの種類</param>
    /// <param name="partsId">取得するパーツのID</param>
    public void PartsGet(PartsType type, int partsId)
    {
        switch(type)
        {
            case PartsType.Head:
                if (_getsHeadParts.Where(data => data.ID == partsId).FirstOrDefault() != null)
                {
                    //Debug.Log("入手済み");
                    break;
                }
                else
                {
                    //Debug.Log("入手");
                    _getsHeadParts.Add(PartsManager.Instance.AllParamData.GetPartsHead(partsId));
                }
                break;
            case PartsType.Body:
                if (_getsBodyParts.Where(data => data.ID == partsId).FirstOrDefault() != null)
                {
                    //Debug.Log("入手済み");
                    break;
                }
                else
                {
                    //Debug.Log("入手");
                    _getsBodyParts.Add(PartsManager.Instance.AllParamData.GetPartsBody(partsId));
                }
                break;
            case PartsType.LHand:
                if (_getsLHandParts.Where(data => data.ID == partsId).FirstOrDefault() != null)
                {
                    //Debug.Log("入手済み");
                    break;
                }
                else
                {
                    //Debug.Log("入手");
                    _getsLHandParts.Add(PartsManager.Instance.AllParamData.GetPartsHand(partsId));
                }
                break;
            case PartsType.RHand:
                if (_getsRHandParts.Where(data => data.ID == partsId).FirstOrDefault() != null)
                {
                    //Debug.Log("入手済み");
                    break;
                }
                else
                {
                    //Debug.Log("入手");
                    _getsRHandParts.Add(PartsManager.Instance.AllParamData.GetPartsHand(partsId));
                }
                break;
            case PartsType.BackPack:
                if (_getsBackPackParts.Where(data => data.ID == partsId).FirstOrDefault() != null)
                {
                    //Debug.Log("入手済み");
                    break;
                }
                else
                {
                    //Debug.Log("入手");
                    _getsBackPackParts.Add(PartsManager.Instance.AllParamData.GetPartsBack(partsId));
                }
                break;
            case PartsType.Leg:
                if (_getsLegParts.Where(data => data.ID == partsId).FirstOrDefault() != null)
                {
                    //Debug.Log("入手済み");
                    break;
                }
                else
                {
                    //Debug.Log("入手");
                    _getsLegParts.Add(PartsManager.Instance.AllParamData.GetPartsLeg(partsId));
                }
                break;
            case PartsType.LWeapon:
                if (_getsWeaponParts.Where(data => data.ID == partsId).FirstOrDefault() != null)
                {
                    //Debug.Log("入手済み");
                    break;
                }
                else
                {
                    //Debug.Log("入手");
                    _getsWeaponParts.Add(PartsManager.Instance.AllParamData.GetPartsWeapon(partsId));
                }
                break;
            case PartsType.RWeapon:
                if (_getsWeaponParts.Where(data => data.ID == partsId).FirstOrDefault() != null)
                {
                    //Debug.Log("入手済み");
                    break;
                }
                else
                {
                    //Debug.Log("入手");
                    _getsWeaponParts.Add(PartsManager.Instance.AllParamData.GetPartsWeapon(partsId));
                }
                break;
        }
    }

    public PartsBodyData[] GetObtainPartsBody()
    {
        return _getsBodyParts.ToArray();
    }
    public PartsHeadData[] GetObtainPartsHead()
    {
        return _getsHeadParts.ToArray();
    }
    public PartsHandData[] GetObtainPartsLHand()
    {
        return _getsLHandParts.ToArray();
    }
    public PartsHandData[] GetObtainPartsRHand()
    {
        return _getsRHandParts.ToArray();
    }
    public PartsLegData[] GetObtainPartsLeg()
    {
        return _getsLegParts.ToArray();
    }
    public PartsBackPackData[] GetObtainPartsBack()
    {
        return _getsBackPackParts.ToArray();
    }
    public PartsWeaponData[] GetObtainPartsWeapon()
    {
        return _getsWeaponParts.ToArray();
    }

    public void ActiveSceneChanged(Scene prescene, Scene thisscene)
    {
        if (thisscene.name == "Home")
        {
            instance.Build();
            PresetSave();
        }
    }

    public IPartsData[] this[PartsType type]
    {
        get
        {
            switch (type)
            {
                case PartsType.Head:
                    return GetObtainPartsHead();
                case PartsType.Body:
                    return GetObtainPartsBody();
                case PartsType.LHand:
                    return GetObtainPartsLHand();
                case PartsType.RHand:
                    return GetObtainPartsRHand();
                case PartsType.Leg:
                    return GetObtainPartsLeg();
                case PartsType.BackPack:
                    return GetObtainPartsBack();
                case PartsType.LWeapon:
                    return GetObtainPartsWeapon();
                case PartsType.RWeapon:
                    return GetObtainPartsWeapon();
                default:
                    break;
            }
            return null;
        }
    }
}