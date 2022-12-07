using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultSituation : Situation
{
    [SerializeField]
    Switch _toTitleSwitch;
    [SerializeField]
    UnityEngine.UI.Text _timeText;
    private void Awake()
    {
        _toTitleSwitch.OnTurnOn += () => GameManager.Instance.ChangeSituation(GameManager.SituationType.Title);
    }

    public override void StartSituation()
    {
        base.StartSituation();
        foreach (Transform item in this.transform)
        {
            item.gameObject.SetActive(true);
        }
        _toTitleSwitch.Init();
        _timeText.text = $"TIME : {ScoreManager.GetTimeScore()}";
    }
    public override void EndSituation()
    {
        foreach (Transform item in this.transform)
        {
            item.gameObject.SetActive(false);
        }
    }
}
