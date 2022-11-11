using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomizeMenu : MonoBehaviour
{
    //[SerializeField] Text _partsName = default;
    [SerializeField] Button _partsButton = default;
    [SerializeField] GameObject _content = default;

    private void Awake()
    {
        PartsManager.Instance.LoadData();
    }

    private void Start()
    {
        Debug.Log(PartsManager.Instance.AllParamData.GetPartsHead(0));
        ButtonInstantiate(PartsCategory.Head, 0);
    }

    /// <summary>
    /// ボタンの生成
    /// </summary>
    /// <param name="category">パーツの種類</param>
    /// <param name="id">パーツのID(再起関数のため0を与える)</param>
    public void ButtonInstantiate(PartsCategory category, int id)
    {
        Button button;
        button = Instantiate(_partsButton);
        switch (category)
        {
            case PartsCategory.Head:
                PartsHeadData headParts = PartsManager.Instance.AllParamData.GetPartsHead(id);
                if (headParts == null)
                {
                    Destroy(button);
                    return;
                }
                button.GetComponentInChildren<Text>().text = headParts.Name;
                break;
            case PartsCategory.Body:
                PartsBodyData bodyParts = PartsManager.Instance.AllParamData.GetPartsBody(id);
                if (bodyParts == null)
                {
                    Destroy(button);
                    return;
                }
                button.GetComponentInChildren<Text>().text = bodyParts.Name;
                break;
            case PartsCategory.LHand:
                PartsHandData lhandParts = PartsManager.Instance.AllParamData.GetPartsHand(id);
                if (lhandParts == null)
                {
                    Destroy(button);
                    return;
                }
                button.GetComponentInChildren<Text>().text = lhandParts.Name;
                break;
            case PartsCategory.RHand:
                PartsHandData rhandParts = PartsManager.Instance.AllParamData.GetPartsHand(id);
                if (rhandParts == null)
                {
                    Destroy(button);
                    return;
                }
                button.GetComponentInChildren<Text>().text = rhandParts.Name;
                break;
            case PartsCategory.Leg:
                PartsLegData legParts = PartsManager.Instance.AllParamData.GetPartsLeg(id);
                if (legParts == null)
                {
                    Destroy(button);
                    return;
                }
                button.GetComponentInChildren<Text>().text = legParts.Name;
                break;
            case PartsCategory.Booster:
                PartsBackPackData boosterParts = PartsManager.Instance.AllParamData.GetPartsBack(id);
                if (boosterParts == null)
                {
                    Destroy(button);
                    return;
                }
                button.GetComponentInChildren<Text>().text = boosterParts.Name;
                break;
            case PartsCategory.LWeapon:
                WeaponBase lweaponParts = PartsManager.Instance.AllModelData.GetWeapon(id);
                if (lweaponParts == null)
                {
                    Destroy(button);
                    return;
                }
                button.GetComponentInChildren<Text>().text = lweaponParts.ID.ToString();
                break;
            case PartsCategory.RWeapon:
                WeaponBase rweaponParts = PartsManager.Instance.AllModelData.GetWeapon(id);
                if (rweaponParts == null)
                {
                    Destroy(button);
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
        ButtonInstantiate(category, id++);
    }
}
