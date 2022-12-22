using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageNamePanel : MonoBehaviour
{
    [SerializeField]
    private GameObject _baseObj = default;
    [SerializeField]
    private Text _stageName = default;
    [SerializeField]
    private Image _stageImage = default;
    [SerializeField]
    private Text _stageGuide = default;
    public void SetPanel(string name, Quaternion angle, int number)
    {
        _stageName.text = name;
        _baseObj.transform.position = Vector3.back * number * 2;
        transform.rotation = angle;
    }
    public void SetPanel(StageGuideData data,Quaternion angle,int number)
    {
        _stageName.text = data.StageName;
        _stageImage.sprite = data.StageImage;
        _stageGuide.text = data.Mission;
        _baseObj.transform.position = Vector3.back * number * 2;
        transform.rotation = angle;
    }
    public void SelectPanel()
    {

    }
}
