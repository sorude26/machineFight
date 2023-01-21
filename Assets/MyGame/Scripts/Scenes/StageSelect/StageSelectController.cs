using MyGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class StageSelectController : MonoBehaviour
{
    private const float INPUT_SENSITIVITY = 0.8f;
    private const float SIDE_RANGE = 2f;
    private const float FORWARD_RANGE = 5f;
    [SerializeField]
    private StageNamePanel _namePanelPrefab = default;
    [SerializeField]
    private float _waitTime = 1f;
    [SerializeField]
    private float _changetime = 1f;
    [SerializeField]
    private Transform _base = default;
    [SerializeField]
    private int _changeSEID = 27;
    [SerializeField]
    private float _seVolume = 0.2f;
    [SerializeField]
    private StageGuideData[] AllStages = default;
    private string _returnScene = "Home";
    private int _stageMaxNumber = default;
    private int _stageNumber = 0;
    private bool _inputStop = false;
    private bool _buttonOn = false;
    private float Angle => 360f / _stageMaxNumber;
    private IEnumerator Start()
    {
        _stageMaxNumber = AllStages.Length;
        for (int i = 0; i < _stageMaxNumber; i++)
        {
            var p = Instantiate(_namePanelPrefab, _base);
            p.SetPanel(AllStages[i], Quaternion.Euler(0, -Angle * i, 0), _stageMaxNumber);
        }
        transform.position = Vector3.forward * _stageMaxNumber * SIDE_RANGE - Vector3.forward * FORWARD_RANGE + Vector3.up * SIDE_RANGE;
        _buttonOn = true;
        _stageNumber = StageData.StageID;        
        ChangeStage(_stageNumber);
        yield return new WaitForSeconds(_waitTime);
        _buttonOn = false;
        PlayerInput.Instance.InitializeInput();
        PlayerInput.SetEnterInput(InputMode.Menu, InputType.Submit, SelectStage);
        PlayerInput.SetEnterInput(InputMode.Menu, InputType.Cancel, ReturnScene);
        PlayerInput.ChangeInputMode(InputMode.Menu);
    }
    private void Update()
    {
        if (_buttonOn == true)
        {
            return;
        }
        MoveCursor(PlayerInput.MoveDir.x, PlayerInput.MoveDir.y);
    }
    private void MoveCursor(float h, float v)
    {
        if (_inputStop == true)
        {
            return;
        }
        if (h > INPUT_SENSITIVITY || h < -INPUT_SENSITIVITY)
        {
            if (h > 0)
            {
                UIControl(1, 0);
            }
            else
            {
                UIControl(-1, 0);
            }
        }
        else if (v > INPUT_SENSITIVITY || v < -INPUT_SENSITIVITY)
        {
            if (v > 0)
            {
                UIControl(0, 1);
            }
            else
            {
                UIControl(0, -1);
            }
        }
    }
    private void UIControl(int target, int value)
    {
        if (target < 0)
        {
            _stageNumber++;
            if (_stageNumber >= _stageMaxNumber)
            {
                _stageNumber = 0;
            }
            value = 0;
            InputStop();
        }
        else if (target > 0)
        {
            _stageNumber--;
            if (_stageNumber < 0)
            {
                _stageNumber = _stageMaxNumber - 1;
            }
            value = 0;
            InputStop();
        }
        TargetControl(_stageNumber, value);
    }
    private void TargetControl(int target, int value)
    {
        if (value == 0)
        {
            ChangeStage(target);
        }
        else
        {
            ChangeSelectTarget(value);
        }
    }
    private void SelectStage()
    {
        if (_inputStop == true || _buttonOn == true)
        {
            return;
        }
        _buttonOn = true;
        var target = _stageMaxNumber - _stageNumber;
        if (target == _stageMaxNumber)
        {
            target = 0;
        }
        var message = new PopUpData(middle: $"{AllStages[target].StageName}へ出撃",sub: "〇：OK",cancel: "×:Cancel");
        StageData.StageID = _stageNumber;
        StageData.StageLevel = AllStages[target].Level;
        StageData.StageName = AllStages[target].StageName;
        PopUpMessage.CreatePopUp(message,
            submitAction: () => SceneChange(AllStages[target].TargetSceneName),
            cancelAction: () => { _buttonOn = false; });
    }
    private void SceneChange(string target)
    {
        PlayerInput.Instance.InitializeInput();
        SceneControl.ChangeTargetScene(target);
    }
    private void ReturnScene()
    {
        if (_inputStop == true || _buttonOn == true)
        {
            return;
        }
        _buttonOn = true;
        StageData.StageID = _stageNumber;
        PlayerInput.Instance.InitializeInput();
        SceneControl.ChangeTargetScene(_returnScene);
    }
    private void ChangeStage(int target)
    {
        transform.rotation = Quaternion.Euler(0, -Angle * target, 0);
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySE(_changeSEID,_seVolume);
        }
    }
    private void ChangeSelectTarget(int value)
    {

    }
    private void InputStop()
    {
        if (_inputStop)
        {
            return;
        }
        _inputStop = true;
        StartCoroutine(InputWait());
    }
    private IEnumerator InputWait()
    {
        float changetime = _changetime;
        while (changetime > 0)
        {
            changetime -= Time.deltaTime;
            yield return null;
        }
        _inputStop = false;
    }
}
