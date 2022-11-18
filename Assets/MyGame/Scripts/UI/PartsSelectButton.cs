using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class PartsSelectButton : MonoBehaviour
{
    [SerializeField] PartsCategory _category = default;
    [SerializeField] GameObject _customizePanel = default;
    [SerializeField] GameObject _content = default;
    bool isStart = true;
    // Start is called before the first frame update
    void Start()
    {
        PartsManager.Instance.LoadData();
        switch (_category)
        {
            case PartsCategory.Head:
                this.gameObject.GetComponentInChildren<Text>().text = PartsManager.Instance.AllParamData.GetPartsHead(PlayerData.instance.BuildPreset.Head).name;
                break;
            case PartsCategory.Body:
                this.gameObject.GetComponentInChildren<Text>().text = PartsManager.Instance.AllParamData.GetPartsBody(PlayerData.instance.BuildPreset.Body).name;
                break;
            case PartsCategory.LHand:
                this.gameObject.GetComponentInChildren<Text>().text = PartsManager.Instance.AllParamData.GetPartsHand(PlayerData.instance.BuildPreset.LHand).name;
                break;
            case PartsCategory.RHand:
                this.gameObject.GetComponentInChildren<Text>().text = PartsManager.Instance.AllParamData.GetPartsHand(PlayerData.instance.BuildPreset.RHand).name;
                break;
            case PartsCategory.Leg:
                this.gameObject.GetComponentInChildren<Text>().text = PartsManager.Instance.AllParamData.GetPartsLeg(PlayerData.instance.BuildPreset.Leg).name;
                break;
            case PartsCategory.Booster:
                this.gameObject.GetComponentInChildren<Text>().text = PartsManager.Instance.AllParamData.GetPartsBack(PlayerData.instance.BuildPreset.Booster).name;
                break;
            case PartsCategory.LWeapon:
                this.gameObject.GetComponentInChildren<Text>().text = PartsManager.Instance.AllModelData.GetWeapon(PlayerData.instance.BuildPreset.LWeapon).name;
                break;
            case PartsCategory.RWeapon:
                this.gameObject.GetComponentInChildren<Text>().text = PartsManager.Instance.AllModelData.GetWeapon(PlayerData.instance.BuildPreset.RWeapon).name;
                break;
        }
        this.gameObject.GetComponent<Button>().onClick.AddListener(PartsCustomizePanelOpen);
    }

    private void OnEnable()
    {
        if (isStart == false)
        {
            switch (_category)
            {
                case PartsCategory.Head:
                    this.gameObject.GetComponentInChildren<Text>().text = PartsManager.Instance.AllParamData.GetPartsHead(PlayerData.instance.BuildPreset.Head).name;
                    break;
                case PartsCategory.Body:
                    this.gameObject.GetComponentInChildren<Text>().text = PartsManager.Instance.AllParamData.GetPartsBody(PlayerData.instance.BuildPreset.Body).name;
                    break;
                case PartsCategory.LHand:
                    this.gameObject.GetComponentInChildren<Text>().text = PartsManager.Instance.AllParamData.GetPartsHand(PlayerData.instance.BuildPreset.LHand).name;
                    break;
                case PartsCategory.RHand:
                    this.gameObject.GetComponentInChildren<Text>().text = PartsManager.Instance.AllParamData.GetPartsHand(PlayerData.instance.BuildPreset.RHand).name;
                    break;
                case PartsCategory.Leg:
                    this.gameObject.GetComponentInChildren<Text>().text = PartsManager.Instance.AllParamData.GetPartsLeg(PlayerData.instance.BuildPreset.Leg).name;
                    break;
                case PartsCategory.Booster:
                    this.gameObject.GetComponentInChildren<Text>().text = PartsManager.Instance.AllParamData.GetPartsBack(PlayerData.instance.BuildPreset.Booster).name;
                    break;
                case PartsCategory.LWeapon:
                    this.gameObject.GetComponentInChildren<Text>().text = PartsManager.Instance.AllModelData.GetWeapon(PlayerData.instance.BuildPreset.LWeapon).name;
                    break;
                case PartsCategory.RWeapon:
                    this.gameObject.GetComponentInChildren<Text>().text = PartsManager.Instance.AllModelData.GetWeapon(PlayerData.instance.BuildPreset.RWeapon).name;
                    break;
            }
        }
        isStart = false;
    }


    private void PartsCustomizePanelOpen()
    {
        StartCoroutine(PartsPanelInstance());
    }

    IEnumerator PartsPanelInstance()
    {
        ButtonSelectController.OnButtonNonSelect();
        var panel = _customizePanel.gameObject.GetComponent<CustomizeMenu>();
        if (_customizePanel.gameObject.transform.childCount > 0)
        {
            panel.ButtonDestory(_content);
        }
        yield return null;
        panel.ButtonInstantiate(_category, 0);
        _customizePanel.gameObject.SetActive(true);
        this.gameObject.transform.parent.gameObject.SetActive(false);
    }
}

