using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomizeMenu : MonoBehaviour
{
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
        ButtonInstantiate(_category, 0);
    }

    /// <summary>
    /// ボタンの生成
    /// </summary>
    /// <param name="category">パーツの種類</param>
    /// <param name="id">パーツのID(再起関数のため0を与える)</param>
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
                    Destroy(button);
                    ButtonSelectController.OnButtonFirstSelect(_content);
                    return;
                }
                button.GetComponentInChildren<Text>().text = headParts.Name;
                break;
            case PartsCategory.Body:
                PartsBodyData bodyParts = PartsManager.Instance.AllParamData.GetPartsBody(id);
                if (bodyParts == null)
                {
                    Destroy(button);
                    ButtonSelectController.OnButtonFirstSelect(_content);
                    return;
                }
                button.GetComponentInChildren<Text>().text = bodyParts.Name;
                break;
            case PartsCategory.LHand:
                PartsHandData lhandParts = PartsManager.Instance.AllParamData.GetPartsHand(id);
                if (lhandParts == null)
                {
                    Destroy(button);
                    ButtonSelectController.OnButtonFirstSelect(_content);
                    return;
                }
                button.GetComponentInChildren<Text>().text = lhandParts.Name;
                break;
            case PartsCategory.RHand:
                PartsHandData rhandParts = PartsManager.Instance.AllParamData.GetPartsHand(id);
                if (rhandParts == null)
                {
                    Destroy(button);
                    ButtonSelectController.OnButtonFirstSelect(_content);
                    return;
                }
                button.GetComponentInChildren<Text>().text = rhandParts.Name;
                break;
            case PartsCategory.Leg:
                PartsLegData legParts = PartsManager.Instance.AllParamData.GetPartsLeg(id);
                if (legParts == null)
                {
                    Destroy(button);
                    ButtonSelectController.OnButtonFirstSelect(_content);
                    return;
                }
                button.GetComponentInChildren<Text>().text = legParts.Name;
                break;
            case PartsCategory.Booster:
                PartsBackPackData boosterParts = PartsManager.Instance.AllParamData.GetPartsBack(id);
                if (boosterParts == null)
                {
                    Destroy(button);
                    ButtonSelectController.OnButtonFirstSelect(_content);
                    return;
                }
                button.GetComponentInChildren<Text>().text = boosterParts.Name;
                break;
            case PartsCategory.LWeapon:
                WeaponBase lweaponParts = PartsManager.Instance.AllModelData.GetWeapon(id);
                if (lweaponParts == null)
                {
                    Destroy(button);
                    ButtonSelectController.OnButtonFirstSelect(_content);
                    return;
                }
                button.GetComponentInChildren<Text>().text = lweaponParts.ID.ToString();
                break;
            case PartsCategory.RWeapon:
                WeaponBase rweaponParts = PartsManager.Instance.AllModelData.GetWeapon(id);
                if (rweaponParts == null)
                {
                    Destroy(button);
                    ButtonSelectController.OnButtonFirstSelect(_content);
                    return;
                }
                button.GetComponentInChildren<Text>().text = rweaponParts.ID.ToString();
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
    /// 変更したいパーツのリストに変える
    /// </summary>
    /// <param name="category">変更先のカテゴリー</param>
    private void CategoryChange(PartsCategory category)
    {
        ButtonInstantiate(_category, 0);
    }
}
