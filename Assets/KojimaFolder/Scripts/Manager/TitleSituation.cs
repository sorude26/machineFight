using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSituation : Situation
{
    [SerializeField]
    Switch _startSwitch;

    private void Awake()
    {
        //スイッチオンでゲーム開始
        _startSwitch.OnTurnOn += () => GameManager.Instance.ChangeSituation(GameManager.SituationType.Stage);
    }
    public override void StartSituation()
    {
        base.StartSituation();
        foreach (Transform item in this.transform)
        {
            item.gameObject.SetActive(true);
        }
        _startSwitch.Init();
    }
    public override void EndSituation()
    {
        foreach(Transform item in this.transform)
        {
            item.gameObject.SetActive(false);
        }
    }
}
