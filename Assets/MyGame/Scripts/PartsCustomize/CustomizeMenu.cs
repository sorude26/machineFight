using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using MyGame;

public class CustomizeMenu : MonoBehaviour
{
    [SerializeField] GameObject _partsSelectPanel = default;
    [SerializeField] Text _partsName = default;
    [SerializeField] Button _partsButton = default;
    [SerializeField] GameObject _content = default;
    [SerializeField] PartsCategory _category = default;

    private void Awake()
    {
        PartsManager.Instance.LoadData();
    }

    private void Start()
    {
        PlayerInput.SetEnterInput(InputMode.Menu, InputType.Fire2, CategoryChangeNext);
        PlayerInput.SetEnterInput(InputMode.Menu, InputType.Fire1, CategoryChangePre);
        PlayerInput.SetEnterInput(InputMode.Menu, InputType.Cancel, EndCustomize);
        PlayerInput.ChangeInputMode(InputMode.Menu);
        //ButtonInstantiate(_category, 0);
        this.gameObject.SetActive(false);
        //ButtonSelectController.OnButtonFirstSelect(_content);
    }

    private void OnEnable()
    {
        if (_content.transform.childCount > 0)
        {
            ButtonSelectController.OnButtonFirstSelect(_content);
        }
    }
    /// <summary>
    /// �{�^���̐���
    /// </summary>
    /// <param name="category">�p�[�c�̎��</param>
    /// <param name="id">�p�[�c��ID(�ċN�֐��̂���0��^����)</param>
    public void ButtonInstantiate(PartsCategory category, int id)
    {
        _partsName.text = category.ToString();
        Button button;
        button = Instantiate(_partsButton);
        switch (category)
        {
            case PartsCategory.Head:
                PartsHeadData headParts = PartsManager.Instance.AllParamData.GetPartsHead(id);
                if (headParts == null)
                {
                    Destroy(button.gameObject);
                    _category = category;
                    return;
                }
                button.GetComponentInChildren<Text>().text = headParts.Name;
                break;
            case PartsCategory.Body:
                PartsBodyData bodyParts = PartsManager.Instance.AllParamData.GetPartsBody(id);
                if (bodyParts == null)
                {
                    Destroy(button.gameObject);
                    _category = category;
                    return;
                }
                button.GetComponentInChildren<Text>().text = bodyParts.Name;
                break;
            case PartsCategory.LHand:
                PartsHandData lhandParts = PartsManager.Instance.AllParamData.GetPartsHand(id);
                if (lhandParts == null)
                {
                    Destroy(button.gameObject);
                    _category = category;
                    return;
                }
                button.GetComponentInChildren<Text>().text = lhandParts.Name;
                break;
            case PartsCategory.RHand:
                PartsHandData rhandParts = PartsManager.Instance.AllParamData.GetPartsHand(id);
                if (rhandParts == null)
                {
                    Destroy(button.gameObject);
                    _category = category;
                    return;
                }
                button.GetComponentInChildren<Text>().text = rhandParts.Name;
                break;
            case PartsCategory.Leg:
                PartsLegData legParts = PartsManager.Instance.AllParamData.GetPartsLeg(id);
                if (legParts == null)
                {
                    Destroy(button.gameObject);
                    _category = category;
                    return;
                }
                button.GetComponentInChildren<Text>().text = legParts.Name;
                break;
            case PartsCategory.Booster:
                PartsBackPackData boosterParts = PartsManager.Instance.AllParamData.GetPartsBack(id);
                if (boosterParts == null)
                {
                    Destroy(button.gameObject);
                    _category = category;
                    return;
                }
                button.GetComponentInChildren<Text>().text = boosterParts.Name;
                break;
            case PartsCategory.LWeapon:
                PartsWeaponData lweaponParts = PartsManager.Instance.AllParamData.GetPartsWeapon(id);
                if (lweaponParts == null)
                {
                    Destroy(button.gameObject);
                    _category = category;
                    return;
                }
                button.GetComponentInChildren<Text>().text = lweaponParts.Name;
                break;
            case PartsCategory.RWeapon:
                PartsWeaponData rweaponParts = PartsManager.Instance.AllParamData.GetPartsWeapon(id);
                if (rweaponParts == null)
                {
                    Destroy(button.gameObject);
                    _category = category;
                    return;
                }
                button.GetComponentInChildren<Text>().text = rweaponParts.Name;
                break;
        }
        button.gameObject.AddComponent<PartsButton>();
        //button.gameObject.AddComponent<EventTrigger>();
        var _ = button.GetComponent<PartsButton>();
        _._partsCategory = category;
        _._partsId = id;
        button.onClick.AddListener(() => _.Customize());
        button.transform.parent = _content.transform;
        ButtonInstantiate(category, id += 1);
    }

    /// <summary>
    /// �ύX�������p�[�c�̃��X�g�ɕς���
    /// </summary>
    /// <param name="category">�ύX��̃J�e�S���[</param>
    public IEnumerator CategoryChange(PartsCategory category)
    {
        ButtonSelectController.OnButtonNonSelect();
        ButtonDestory(_content);
        yield return null;
        ButtonInstantiate(_category, 0);
        ButtonSelectController.OnButtonFirstSelect(_content);
    }

    /// <summary>
    /// �{�^�������ׂč폜����
    /// </summary>
    public void ButtonDestory(GameObject content)
    {
        foreach (Transform button in content.transform)
        {
            Destroy(button.gameObject);
        }
    }

    /// <summary>
    /// ���̃J�e�S���[�ɕύX
    /// </summary>
    public void CategoryChangeNext()
    {
        int nextcategoryNum = (int)_category + 1;
        if (nextcategoryNum >= Enum.GetValues(typeof(PartsCategory)).Length)
        {
            nextcategoryNum = 0;
        }
        if (Enum.IsDefined((typeof(PartsCategory)), nextcategoryNum))
        {
            _category = (PartsCategory)nextcategoryNum;
            StartCoroutine(CategoryChange(_category));
        }
    }

    /// <summary>
    /// �O�̃J�e�S���[�ɕύX
    /// </summary>
    public void CategoryChangePre()
    {
        int nextcategoryNum = (int)_category - 1;
        if (nextcategoryNum < 0 )
        {
            nextcategoryNum = Enum.GetValues(typeof(PartsCategory)).Length - 1;
        }
        if (Enum.IsDefined((typeof(PartsCategory)), nextcategoryNum))
        {
            _category = (PartsCategory)nextcategoryNum;
            StartCoroutine(CategoryChange(_category));
        }
    }

    /// <summary>
    /// �J�X�^�}�C�Y���j���[�����
    /// </summary>
    public void EndCustomize()
    {
        if (this.gameObject.activeSelf == true)
        {
            ButtonSelectController.OnButtonNonSelect();
            _partsSelectPanel.SetActive(true);
            ButtonSelectController.OnButtonFirstSelect(_partsSelectPanel);
            this.gameObject.SetActive(false);
        }
    }
}
