using MyGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }
    public static bool InStage { get; private set; }
    private int _breakBossCount = 0;
    private int _breakTargetCount = 0;
    private int _clearBossCount = 0;
    [SerializeField]
    private int _defaultBossCount = 0;
    [SerializeField]
    private int _bgmIDNum = 29;
    [SerializeField]
    private int _stageClearbgmIDNum = 41;
    [SerializeField]
    private int _gameOverbgmIDNum = 42;
    [SerializeField]
    private float _bgmVolume = 0.1f;
    [SerializeField]
    private int _changeBgmIDNum = 0;
    [SerializeField]
    private float _changeBgmSpeed = 1f;
    [SerializeField]
    private int _alarmSEID = 47;
    [SerializeField]
    private float _alarmVolume = 0.5f;
    [SerializeField]
    private float _moveTime = 10f;
    [SerializeField]
    private GameObject[] _enemySet = default;
    private int _targetCount = 0;
    private int _totalCount = 0;
    private bool _gameOver = false;
    public event Action OnGameEnd = default;
    public int TotalDamage { get; set; }
    private void Awake()
    {
        Instance = this;
        PartsManager.Instance.LoadData();
        _clearBossCount = _defaultBossCount;
        InStage = false;
    }
    private IEnumerator Start()
    {
        if (_enemySet != null && StageData.StageLevel < _enemySet.Length)
        {
            _enemySet[StageData.StageLevel].SetActive(true);
        }
        PlayerInput.ChangeInputMode(InputMode.InGame);
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayBGM(_bgmIDNum, _bgmVolume);
        }
        yield return null;
        InStage = true;
    }
    public void ChangeBGM()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayBGMWithCrossFade(_changeBgmIDNum, _changeBgmSpeed, _bgmVolume);
        }
    }
    public void PlayAlarm()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySE(_alarmSEID, _alarmVolume);
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
    public void SetBossCount(int count = 1)
    {
        if (_defaultBossCount > 0)
        {
            return;
        }
        _clearBossCount += count;
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
        InStage = false;
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
        StartCoroutine(MoveResultImpl());
    }
    private IEnumerator MoveResultImpl()
    {
        yield return new WaitForSeconds(_moveTime);
        Instance = null;
        SceneControl.ChangeTargetScene("Result");
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
        var massage = new PopUpData(center: "îCñ±äÆóπ");
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
        var massage = new PopUpData(center: "îCñ±é∏îs");
        MoveResult(massage);
    }
    public void ViewOutStage()
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
        var massage = new PopUpData(center: "êÌàÊó£íE");
        MoveResult(massage);
    }
}
