using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using MyGame;
using UniRx.Triggers;
using UniRx;
using UnityEngine.InputSystem.UI;

public class CustomizeMenu : MonoBehaviour
{
    private static readonly int VR_COLOR = 20;
    [SerializeField] GameObject _partsSelectPanel = default;
    [SerializeField] Text _partsName = default;
    [SerializeField] Button _partsButton = default;
    [SerializeField] RectTransform _content = default;
    [SerializeField] PartsCategory _category = default;
    private int _selectedIndex = default;
    //[SerializeField] GameObject _currentButton = default;
    ReactiveProperty<GameObject> _currentButtonProperty = new ReactiveProperty<GameObject>();
    private GameObject _preButton = null;


    PlayerData _playerData; 

    private void Awake()
    {
        PartsManager.Instance.LoadData();
    }

    private void Start()
    {
        _playerData = PlayerData.instance;
        //ButtonInstantiate(_category, 0);
        this.gameObject.SetActive(false);
        _currentButtonProperty.Value = (GameObject)ButtonSelectController.OnGetCurrentButton();
        _currentButtonProperty.Skip(1).Subscribe(_ => Scroll()).AddTo(this);
        //ButtonSelectController.OnButtonFirstSelect(_content);
    }

    private void OnEnable()
    {
        if (_content.transform.childCount > 0)
        {
            ButtonSelectController.OnButtonFirstSelect(_content.gameObject);
        }
        PlayerInput.SetEnterInput(InputMode.Menu, InputType.Fire2, CategoryChangeNext);
        PlayerInput.SetEnterInput(InputMode.Menu, InputType.Fire1, CategoryChangePre);
        PlayerInput.SetEnterInput(InputMode.Menu, InputType.Cancel, EndCustomize);
        PlayerInput.ChangeInputMode(InputMode.Menu);
        _selectedIndex = 0;
        _content.transform.localPosition = new Vector3(0, 0, 0);
    }

    private void OnDisable()
    {
        PlayerInput.LiftEnterInput(InputMode.Menu, InputType.Fire2, CategoryChangeNext);
        PlayerInput.LiftEnterInput(InputMode.Menu, InputType.Fire1, CategoryChangePre);
        PlayerInput.LiftEnterInput(InputMode.Menu, InputType.Cancel, EndCustomize);
    }

    private void Update()
    {
        _currentButtonProperty.Value = (GameObject)ButtonSelectController.OnGetCurrentButton();
    }
    
    private void SetPartsButton(IPartsData[] partsDatas,PartsCategory category)
    {
        if (partsDatas == null || partsDatas.Length == 0)
        {
            return;
        }
        else
        {
            foreach (var parts in partsDatas)
            {
                Button button = Instantiate(_partsButton);
                Vector2 buttonScale = button.transform.localScale;                
                PartsButton _ = button.gameObject.AddComponent<PartsButton>();
                _._partsCategory = category;
                _._partsId = parts.PartsID;
                button.onClick.AddListener(() => _.Customize());
                button.transform.SetParent(_content);
                button.GetComponentInChildren<Text>().text = parts.PartName;
                button.transform.localScale = buttonScale;
                button.transform.localPosition = Vector3.zero;
            }
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
                _category = category;
                SetPartsButton(_playerData.GetObtainPartsHead(), category);
                break;
            case PartsCategory.Body:
                SetPartsButton(_playerData.GetObtainPartsBody(), category);
                break;
            case PartsCategory.LHand:
                SetPartsButton(_playerData.GetObtainPartsLHand(), category);
                break;
            case PartsCategory.RHand:
                SetPartsButton(_playerData.GetObtainPartsRHand(), category);
                break;
            case PartsCategory.Leg:
                SetPartsButton(_playerData.GetObtainPartsLeg(), category);
                break;
            case PartsCategory.Booster:
                SetPartsButton(_playerData.GetObtainPartsBack(), category);
                break;
            case PartsCategory.LWeapon:
                SetPartsButton(_playerData.GetObtainPartsWeapon(), category);
                break;
            case PartsCategory.RWeapon:
                SetPartsButton(_playerData.GetObtainPartsWeapon(), category);
                break;
            case PartsCategory.Color:
                int id = 0;
                while(PartsManager.Instance.AllModelData.GetColor(id) != null)
                {
                    if (id == VR_COLOR)
                    {
                        id++;
                        continue;
                    }
                    var color = PartsManager.Instance.AllModelData.GetColor(id);
                    Button button = Instantiate(_partsButton);
                    Vector2 buttonScale = button.transform.localScale;
                    PartsButton _ = button.gameObject.AddComponent<PartsButton>();
                    _._partsCategory = category;
                    _._partsId = color.ID;
                    button.onClick.AddListener(() => _.Customize());
                    button.transform.SetParent(_content);
                    button.GetComponentInChildren<Text>().text = color.ColorSetName;
                    id++;
                    button.transform.localScale = buttonScale;
                    button.transform.localPosition = Vector3.zero;
                }
                break;
        }
    }

    /// <summary>
    /// 変更したいパーツのリストに変える
    /// </summary>
    /// <param name="category">変更先のカテゴリー</param>
    public IEnumerator CategoryChange(PartsCategory category)
    {
        ButtonSelectController.OnButtonNonSelect();
        ButtonDestory(_content.gameObject);
        _preButton = null;
        yield return null;
        ButtonInstantiate(_category);
        ButtonSelectController.OnButtonFirstSelect(_content.gameObject);
        _selectedIndex = 0;
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
        if (gameObject.activeInHierarchy == false)
        {
            return;
        }
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
        if (gameObject.activeInHierarchy == false)
        {
            return;
        }
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

    public void Scroll()
    {
        if (_currentButtonProperty.Value != null)
        {   
            if (_preButton == null)
            {
                _preButton = _currentButtonProperty.Value;
                return;
            }
            if (_currentButtonProperty.Value.transform.localPosition.y > _preButton.transform.localPosition.y)
            {
                _selectedIndex -= 1;
                if (_selectedIndex < 0)
                {
                    _content.transform.localPosition = new Vector3(_content.transform.localPosition.x, _content.transform.localPosition.y - 110f, _content.transform.localPosition.z);
                    _selectedIndex = 0;
                }
            }
            else if (_currentButtonProperty.Value.transform.localPosition.y < _preButton.transform.localPosition.y)
            {
                _selectedIndex += 1;
                if (_selectedIndex > 3)
                {
                    _content.transform.localPosition = new Vector3(_content.transform.localPosition.x, _content.transform.localPosition.y + 110f, _content.transform.localPosition.z);
                    _selectedIndex = 3;
                }
            }
        }
        _preButton = _currentButtonProperty.Value;
        if (_preButton == null)
        {
            return;
        }
        if (_preButton.TryGetComponent<PartsButton>(out var current))
        {
            current.OnSelectButton();
        }
    }
}
