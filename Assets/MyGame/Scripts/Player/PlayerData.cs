using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;

    private PartsBuildParam _buildPreset = default;
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

    private ModelBuilder _modelBuilder = default;

    [SerializeField] 
    private List<PartsHeadData> _getsHeadParts = new List<PartsHeadData>();
    [SerializeField] 
    private List<PartsBodyData> _getsBodyParts = new List<PartsBodyData>();
    [SerializeField] 
    private List<PartsHandData> _getsHandParts = new List<PartsHandData>();
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
            instance._buildPreset = instance.PresetLoad();
            instance._modelBuilder = new ModelBuilder();
            PartsManager.Instance.LoadData();
            SceneManager.activeSceneChanged += ActiveSceneChanged;
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
    //Json�ɏ�������
    public void PresetSave()
    {
        
    }

    //Json����ǂݍ���
    public PartsBuildParam PresetLoad()
    {
        PartsBuildParam buildPreset = new PartsBuildParam();
        buildPreset.Head = 0;
        buildPreset.Body = 0;
        buildPreset.LHand = 0;
        buildPreset.RHand = 0;
        buildPreset.Leg = 0;
        buildPreset.Booster = 0;
        buildPreset.LWeapon = 0;
        buildPreset.RWeapon = 0;
        buildPreset.ColorId = 0;
        return buildPreset;
    }

    /// <summary>
    /// ���݂̃p�[�c�̃��O�𗬂�
    /// </summary>
    public void PartsLog()
    {
        Debug.Log("Head:" + BuildPreset.Head);
        Debug.Log("Body:" + BuildPreset.Body);
        Debug.Log("LHand:" + BuildPreset.LHand);
        Debug.Log("RHand:" + BuildPreset.RHand);
        Debug.Log("Leg:" + BuildPreset.Leg);
        Debug.Log("Booster:" + BuildPreset.Booster);
        Debug.Log("LWeapon:" + BuildPreset.LWeapon);
        Debug.Log("RWeapon:" + BuildPreset.RWeapon);
        Debug.Log("Color:" + BuildPreset.ColorId);

    }

    /// <summary>
    /// ���f���𐶐�����
    /// </summary>
    public void Build()
    {
        _modelBuilder.ViewModel(BuildPreset);
    }

    /// <summary>
    /// �p�[�c����肷��@Hand��Weapon��LHand�ALWeapon�̃^�O���g��
    /// </summary>
    /// <param name="type">�p�[�c�̎��</param>
    /// <param name="partsId">�擾����p�[�c��ID</param>
    public void PartsGet(PartsType type, int partsId)
    {
        switch(type)
        {
            case PartsType.Head:
                if (_getsHeadParts.Where(data => data.ID == partsId).FirstOrDefault() != null)
                {
                    Debug.Log("����ς�");
                    break;
                }
                else
                {
                    Debug.Log("����");
                    _getsHeadParts.Add(PartsManager.Instance.AllParamData.GetPartsHead(partsId));
                }
                break;
            case PartsType.Body:
                if (_getsBodyParts.Where(data => data.ID == partsId).FirstOrDefault() != null)
                {
                    Debug.Log("����ς�");
                    break;
                }
                else
                {
                    Debug.Log("����");
                    _getsBodyParts.Add(PartsManager.Instance.AllParamData.GetPartsBody(partsId));
                }
                break;
            case PartsType.LHand:
                if (_getsHandParts.Where(data => data.ID == partsId).FirstOrDefault() != null)
                {
                    Debug.Log("����ς�");
                    break;
                }
                else
                {
                    Debug.Log("����");
                    _getsHandParts.Add(PartsManager.Instance.AllParamData.GetPartsHand(partsId));
                }
                break;
            case PartsType.RHand:
                if (_getsHandParts.Where(data => data.ID == partsId).FirstOrDefault() != null)
                {
                    Debug.Log("����ς�");
                    break;
                }
                else
                {
                    Debug.Log("����");
                    _getsHandParts.Add(PartsManager.Instance.AllParamData.GetPartsHand(partsId));
                }
                break;
            case PartsType.BackPack:
                if (_getsBackPackParts.Where(data => data.ID == partsId).FirstOrDefault() != null)
                {
                    Debug.Log("����ς�");
                    break;
                }
                else
                {
                    Debug.Log("����");
                    _getsBackPackParts.Add(PartsManager.Instance.AllParamData.GetPartsBack(partsId));
                }
                break;
            case PartsType.Leg:
                if (_getsLegParts.Where(data => data.ID == partsId).FirstOrDefault() != null)
                {
                    Debug.Log("����ς�");
                    break;
                }
                else
                {
                    Debug.Log("����");
                    _getsLegParts.Add(PartsManager.Instance.AllParamData.GetPartsLeg(partsId));
                }
                break;
            case PartsType.LWeapon:
                if (_getsWeaponParts.Where(data => data.ID == partsId).FirstOrDefault() != null)
                {
                    Debug.Log("����ς�");
                    break;
                }
                else
                {
                    Debug.Log("����");
                    _getsWeaponParts.Add(PartsManager.Instance.AllParamData.GetPartsWeapon(partsId));
                }
                break;
            case PartsType.RWeapon:
                if (_getsWeaponParts.Where(data => data.ID == partsId).FirstOrDefault() != null)
                {
                    Debug.Log("����ς�");
                    break;
                }
                else
                {
                    Debug.Log("����");
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
    public PartsHandData[] GetObtainPartsHand()
    {
        return _getsHandParts.ToArray();
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
        }
    }
}
