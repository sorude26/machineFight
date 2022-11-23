using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultSceneController : MonoBehaviour
{
    //[SerializeField]
    //private string _targetScene = default;
    [SerializeField]
    private Text _totalDamage = default;
    [SerializeField]
    private Text _countTotal = default;
    [SerializeField]
    private Text _countTarget = default;
    [SerializeField]
    private Text _countBoss = default;
    [SerializeField]
    private GameObject _clearMessage = default;
    [SerializeField]
    private GameObject _gameOverMessage = default;
    private void Start()
    {
        _gameOverMessage.SetActive(!ResultData.IsStageClear);
        _clearMessage.SetActive(ResultData.IsStageClear);
        _countTotal.text = $"{ResultData.TotalCount}";
        _totalDamage.text = $"{ResultData.TotalDamage}";
        _countTarget.text = $"{ResultData.TotalTargetCount} / {ResultData.TotalTargetNum}";
        _countBoss.text = $"{ResultData.TotalBossCount} / {ResultData.TotalBossNum}";
    }
}
