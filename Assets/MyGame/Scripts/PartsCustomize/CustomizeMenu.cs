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

    PlayerData _playerData; 

    private void Awake()
    {
        PartsManager.Instance.LoadData();
    }

    private void Start()
    {
        _playerData = PlayerData.instance;
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
    /// ボタンの生成
    /// </summary>
    /// <param name="category">パーツの種類</param>
    public void ButtonInstantiate(PartsCategory category)
    {
        _partsName.text = category.ToString();
        switch (category)
        {
            case PartsCategory.Head:
                PartsHeadData[] headParts = _playerData.GetObtainPartsHead();
                _category = category;
                if (headParts == null)
                {   
                    return;
                }
                else
                {
                    foreach (var parts in headParts)
                    {
                        Button button = Instantiate(_partsButton);
                        button.gameObject.AddComponent<PartsButton>();
                        //button.gameObject.AddComponent<EventTrigger>();
                        PartsButton _ = button.GetComponent<PartsButton>();
                        _._partsCategory = category;
                        _._partsId = parts.ID;
                        button.onClick.AddListener(() => _.Customize());
                        button.transform.parent = _content.transform;
                        button.GetComponentInChildren<Text>().text = parts.Name;
                    }
                }
                
                break;
            case PartsCategory.Body:
                PartsBodyData[] bodyParts = _playerData.GetObtainPartsBody();
                if (bodyParts == null)
                {
                    return;
                }
                else
                {
                    foreach (var parts in bodyParts)
                    {
                        Button button = Instantiate(_partsButton);
                        button.gameObject.AddComponent<PartsButton>();
                        //button.gameObject.AddComponent<EventTrigger>();
                        PartsButton _ = button.GetComponent<PartsButton>();
                        _._partsCategory = category;
                        _._partsId = parts.ID;
                        button.onClick.AddListener(() => _.Customize());
                        button.transform.parent = _content.transform;
                        button.GetComponentInChildren<Text>().text = parts.Name;
                    }
                }
                break;
            case PartsCategory.LHand:
                PartsHandData[] lhandParts = _playerData.GetObtainPartsHand();
                if (lhandParts == null)
                {
                    return;
                }
                else
                {
                    foreach (var parts in lhandParts)
                    {
                        Button button = Instantiate(_partsButton);
                        button.gameObject.AddComponent<PartsButton>();
                        //button.gameObject.AddComponent<EventTrigger>();
                        PartsButton _ = button.GetComponent<PartsButton>();
                        _._partsCategory = category;
                        _._partsId = parts.ID;
                        button.onClick.AddListener(() => _.Customize());
                        button.transform.parent = _content.transform;
                        button.GetComponentInChildren<Text>().text = parts.Name;
                    }
                }
                break;
            case PartsCategory.RHand:
                PartsHandData[] rhandParts = _playerData.GetObtainPartsHand();
                if (rhandParts == null)
                {
                    return;
                }
                else
                {
                    foreach (var parts in rhandParts)
                    {
                        Button button = Instantiate(_partsButton);
                        button.gameObject.AddComponent<PartsButton>();
                        //button.gameObject.AddComponent<EventTrigger>();
                        PartsButton _ = button.GetComponent<PartsButton>();
                        _._partsCategory = category;
                        _._partsId = parts.ID;
                        button.onClick.AddListener(() => _.Customize());
                        button.transform.parent = _content.transform;
                        button.GetComponentInChildren<Text>().text = parts.Name;
                    }
                }
                break;
            case PartsCategory.Leg:
                PartsLegData[] legParts = _playerData.GetObtainPartsLeg();
                if (legParts == null)
                {
                    return;
                }
                else
                {
                    foreach (var parts in legParts)
                    {
                        Button button = Instantiate(_partsButton);
                        button.gameObject.AddComponent<PartsButton>();
                        //button.gameObject.AddComponent<EventTrigger>();
                        PartsButton _ = button.GetComponent<PartsButton>();
                        _._partsCategory = category;
                        _._partsId = parts.ID;
                        button.onClick.AddListener(() => _.Customize());
                        button.transform.parent = _content.transform;
                        button.GetComponentInChildren<Text>().text = parts.Name;
                    }
                }
                break;
            case PartsCategory.Booster:
                PartsBackPackData[] boosterParts = _playerData.GetObtainPartsBack();
                if (boosterParts == null)
                {
                    return;
                }
                else
                {
                    foreach (var parts in boosterParts)
                    {
                        Button button = Instantiate(_partsButton);
                        button.gameObject.AddComponent<PartsButton>();
                        //button.gameObject.AddComponent<EventTrigger>();
                        PartsButton _ = button.GetComponent<PartsButton>();
                        _._partsCategory = category;
                        _._partsId = parts.ID;
                        button.onClick.AddListener(() => _.Customize());
                        button.transform.parent = _content.transform;
                        button.GetComponentInChildren<Text>().text = parts.Name;
                    }
                }
                break;
            case PartsCategory.LWeapon:
                PartsWeaponData[] lweaponParts = _playerData.GetObtainPartsWeapon();
                if (lweaponParts == null)
                {
                    return;
                }
                else
                {
                    foreach (var parts in lweaponParts)
                    {
                        Button button = Instantiate(_partsButton);
                        button.gameObject.AddComponent<PartsButton>();
                        //button.gameObject.AddComponent<EventTrigger>();
                        PartsButton _ = button.GetComponent<PartsButton>();
                        _._partsCategory = category;
                        _._partsId = parts.ID;
                        button.onClick.AddListener(() => _.Customize());
                        button.transform.parent = _content.transform;
                        button.GetComponentInChildren<Text>().text = parts.Name;
                    }
                }
                break;
            case PartsCategory.RWeapon:
                PartsWeaponData[] rweaponParts = _playerData.GetObtainPartsWeapon();
                if (rweaponParts == null)
                {
                    return;
                }
                else
                {
                    foreach (var parts in rweaponParts)
                    {
                        Button button = Instantiate(_partsButton);
                        button.gameObject.AddComponent<PartsButton>();
                        //button.gameObject.AddComponent<EventTrigger>();
                        PartsButton _ = button.GetComponent<PartsButton>();
                        _._partsCategory = category;
                        _._partsId = parts.ID;
                        button.onClick.AddListener(() => _.Customize());
                        button.transform.parent = _content.transform;
                        button.GetComponentInChildren<Text>().text = parts.Name;
                    }
                }
                break;
        }
        //button.gameObject.AddComponent<PartsButton>();
        ////button.gameObject.AddComponent<EventTrigger>();
        //var _ = button.GetComponent<PartsButton>();
        //_._partsCategory = category;
        //_._partsId = id;
        //button.onClick.AddListener(() => _.Customize());
        //button.transform.parent = _content.transform;
        //ButtonInstantiate(category, id += 1);
    }

    /// <summary>
    /// 変更したいパーツのリストに変える
    /// </summary>
    /// <param name="category">変更先のカテゴリー</param>
    public IEnumerator CategoryChange(PartsCategory category)
    {
        ButtonSelectController.OnButtonNonSelect();
        ButtonDestory(_content);
        yield return null;
        ButtonInstantiate(_category);
        ButtonSelectController.OnButtonFirstSelect(_content);
    }

    /// <summary>
    /// ボタンをすべて削除する
    /// </summary>
    public void ButtonDestory(GameObject content)
    {
        foreach (Transform button in content.transform)
        {
            Destroy(button.gameObject);
        }
    }

    /// <summary>
    /// 次のカテゴリーに変更
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
    /// 前のカテゴリーに変更
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
    /// カスタマイズメニューを閉じる
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
