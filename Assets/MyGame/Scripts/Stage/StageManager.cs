using MyGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }
    public int BreakCount { get; private set; }
    public int CLEAR_COUNT = 10;
    private bool _clear = false;
    private bool _gameOver = false;
    private void Awake()
    {
        Instance = this;
        PartsManager.Instance.LoadData();
    }
    private void Start()
    {
        PlayerInput.ChangeInputMode(InputMode.InGame);
    }
    public void AddCount(int count = 1)
    {
        if (_clear == true)
        {
            return;
        }
        BreakCount += count;
        if (BreakCount >= CLEAR_COUNT)
        {
            _clear = true;
            ViewClearPop();
        }
    }
    private void ViewClearPop()
    {
        if (_gameOver == true)
        {
            return;
        }
        PlayerInput.Instance.InitializeInput();
        _gameOver = true;
        var massage = new PopUpData(center: "”C–±Š®—¹");
        PopUpMessage.CreatePopUp(massage, otherAction: () => { SceneControl.ChangeTargetScene("Result"); });
    }
    public void ViewGameOver()
    {
        if (_gameOver == true)
        {
            return;
        }
        PlayerInput.Instance.InitializeInput();
        _gameOver = true;
        var massage = new PopUpData(center: "”C–±Ž¸”s");
        PopUpMessage.CreatePopUp(massage, otherAction: () => { SceneControl.ChangeTargetScene("Result"); });
    }
}
