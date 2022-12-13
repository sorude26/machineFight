using MyGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }
    private int _breakBossCount;
    private int _breakTargetCount;
    [SerializeField]
    private int _clearBossCount = 1;
    [SerializeField]
    private int _bgmIDNum = 29;
    [SerializeField]
    private int _stageClearbgmIDNum = 41;
    [SerializeField]
    private int _gameOverbgmIDNum = 42;
    [SerializeField]
    private float _bgmVolume = 0.1f;
    private int _targetCount = 0;
    private int _totalCount = 0;
    private bool _gameOver = false;
    public event Action OnGameEnd = default;
    public int TotalDamage { get; set; }
    private void Awake()
    {
        Instance = this;
        PartsManager.Instance.LoadData();
    }
    private void Start()
    {
        PlayerInput.ChangeInputMode(InputMode.InGame);
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayBGM(_bgmIDNum, _bgmVolume);
        }
    }
    public void AddBossCount(int count = 1)
    {
        if (_gameOver == true)
        {
            return;
        }
        _breakBossCount += count;
        if (_breakBossCount >= _clearBossCount)
        {
            ViewClearPop();
        }
    }
    public void SetTargetCount(int count = 1)
    {
        _targetCount += count;
    }
    public void AddTargetCount(int count = 1)
    {
        if (_gameOver == true)
        {
            return;
        }
        _breakTargetCount += count;
    }
    public void AddCount()
    {
        _totalCount++;
    }
    private void MoveResult(PopUpData massage)
    {
        PlayerInput.Instance.InitializeInput();
        _gameOver = true;
        ResultData.TotalTargetCount = _breakTargetCount;
        ResultData.TotalBossCount = _breakBossCount;
        ResultData.TotalTargetNum = _targetCount;
        ResultData.TotalBossNum = _clearBossCount;
        ResultData.TotalDamage = TotalDamage;
        ResultData.TotalCount = _totalCount;
        PopUpMessage.CreatePopUp(massage, otherAction: () =>
        {
            Instance = null;
            SceneControl.ChangeTargetScene("Result");
        });
    }
    private void ViewClearPop()
    {
        if (_gameOver == true)
        {
            return;
        }
        OnGameEnd?.Invoke();
        ResultData.IsStageClear = true;
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayBGM(_stageClearbgmIDNum, _bgmVolume);
        }
        var massage = new PopUpData(center: "”C–±Š®—¹");
        MoveResult(massage);
    }
    public void ViewGameOver()
    {
        if (_gameOver == true)
        {
            return;
        }
        OnGameEnd?.Invoke();
        ResultData.IsStageClear = false;
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayBGM(_gameOverbgmIDNum, _bgmVolume);
        }
        var massage = new PopUpData(center: "”C–±Ž¸”s");
        MoveResult(massage);
    }
}
