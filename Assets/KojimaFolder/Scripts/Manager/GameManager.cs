using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public enum SituationType
    {
        Title,
        Stage,
        Result,
        Setting,
        Length,
    }

    [SerializeField]
    TitleSituation _titleSituation;
    [SerializeField]
    StageSituation _stageSituation;
    [SerializeField]
    ResultSituation _resultSituation;
    [SerializeField]
    SettingSituation _settingSituation;
    [SerializeField]
    Switch[] _switchPrefabs;

    static GameManager _instance;
    Situation _currentSituation;
    SituationType _currentSituationType;
    Situation[] _situations;

    public static GameManager Instance => _instance;
    public Switch[] SwitchPrefabs => _switchPrefabs;
    public void ChangeSituation(SituationType situation)
    {
        ChangeSituatiionImple(situation);
    }




    private void Awake()
    {
        _instance = this;
        _situations = new Situation[(int)SituationType.Length];
        _situations[0] = _titleSituation;
        _situations[1] = _stageSituation;
        _situations[2] = _resultSituation;
        _situations[3] = _settingSituation;
        foreach(var item in _situations)
        {
            item.gameObject.SetActive(false);
        }
    }
    private void Start()
    {
        StartSituation(SituationType.Title);
    }
    private void Update()
    {
        _currentSituation?.SituationUpdate();
    }
    private void ChangeSituatiionImple(SituationType situation)
    {
        if (_currentSituationType == situation) return;
        //現在のシチュエーションを終了し、新しいシチュエーションを開始する
        _currentSituation.EndSituation();
        StartSituation(situation);
    }
    private void StartSituation(SituationType situation)
    {
        //新しいシチュエーションを設定し、開始
        _currentSituation = _situations[(int)situation];
        _currentSituation.StartSituation();
        _currentSituationType = situation;
    }
}
