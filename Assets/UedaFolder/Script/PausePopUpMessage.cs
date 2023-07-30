using System;
using MyGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePopUpMessage : MonoBehaviour
{
    [SerializeField]
    private GameObject _operationImage;
    [SerializeField]
    private int _submitSEID = 25;
    [SerializeField]
    private int _cancelSEID = 25;
    [SerializeField]
    private float _seVolume = 0.2f;
    [SerializeField]
    private Text _topText = default;
    [SerializeField]
    private Text _centerText = default;
    [SerializeField]
    private Text _middleText = default;
    [SerializeField]
    private Text _submitText = default;
    [SerializeField]
    private Text _cancelText = default;
    [SerializeField]
    private Text _operationText = default;
    [SerializeField]
    private Button _submitButton = null;
    [SerializeField]
    private Button _cancelButton = null;
    [SerializeField]
    private Button _operationButton = null;
    private Action _submitDel = default;
    private Action _cancelDel = default;
    private Action _operationDel = default;
    public static void CreatePopUp(PopUpData data,string operationText, Action submitAction = null, Action cancelAction = null, Action operationAction = null)
    {
        var obj = Instantiate(Resources.Load<PausePopUpMessage>("Prefabs/PausePopUpCanvas"));
        obj.Initialized(data, operationText,submitAction, cancelAction, operationAction);
    }
    public void Initialized(PopUpData data, string operationText, Action submitAction, Action cancelAction, Action operationAction)
    {
        _topText.text = data.TopText;
        _centerText.text = data.CenterText;
        _middleText.text = data.MiddleText;
        if (cancelAction != null)
        {
            _cancelText.text = data.Cancel;
            _cancelButton.onClick.AddListener(() =>
            {
                PlaySE(_cancelSEID);
                cancelAction?.Invoke();
            });
            _cancelDel = cancelAction;
            _cancelButton.gameObject.SetActive(true);
        }
        else
        {
            _cancelButton.gameObject.SetActive(false);
        }
        if (submitAction != null)
        {
            _submitText.text = data.Submit;
            _submitButton.onClick.AddListener(() =>
            {
                PlaySE(_submitSEID);
                submitAction?.Invoke();
            });
            _submitDel = submitAction;
            _submitButton.gameObject.SetActive(true);
        }
        else
        {
            _submitButton.gameObject.SetActive(false);
        }
         _operationButton.gameObject.SetActive(true);
        PlayerInput.SetEnterInput(InputMode.Menu, InputType.Submit, Submit);
        PlayerInput.SetEnterInput(InputMode.Menu, InputType.Cancel, Cancel);
        PlayerInput.SetEnterInput(InputMode.Menu, InputType.Fire3, Operation);
        
        PlayerInput.ChangeInputMode(InputMode.Menu);
    }
    private void Submit()
    {
        _submitDel?.Invoke();
        ClosePopUp();
    }
    private void PlaySE(int seID)
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySE(seID, _seVolume);
        }
    }
    private void Cancel()
    {
        _cancelDel?.Invoke();
        ClosePopUp();
    }
    private void Operation()
    {
        PlayerInput.LiftEnterInput(InputMode.Menu, InputType.Submit, Submit);
        PlayerInput.LiftEnterInput(InputMode.Menu, InputType.Cancel, Cancel);
        PlayerInput.LiftEnterInput(InputMode.Menu, InputType.Fire3, Operation);
        //UI‚ðSetActive‚·‚é
        _operationImage.gameObject.SetActive(true);
        PlayerInput.SetEnterInput(InputMode.Menu, InputType.Cancel, CnacelOperation);
    }
    private void CnacelOperation()
    {
        //UI‚ðSetActive(false)‚·‚é
        _operationImage.gameObject.SetActive(false);
        PlayerInput.LiftEnterInput(InputMode.Menu, InputType.Cancel, CnacelOperation);
        PlayerInput.SetEnterInput(InputMode.Menu, InputType.Submit, Submit);
        PlayerInput.SetEnterInput(InputMode.Menu, InputType.Cancel, Cancel);
        PlayerInput.SetEnterInput(InputMode.Menu, InputType.Fire3, Operation);
        
    }
    public void ClosePopUp()
    {
        Destroy(this.gameObject);
    }
    private void OnDisable()
    {
        PlayerInput.LiftEnterInput(InputMode.Menu, InputType.Submit, Submit);
        PlayerInput.LiftEnterInput(InputMode.Menu, InputType.Cancel, Cancel);
        PlayerInput.LiftEnterInput(InputMode.Menu, InputType.Fire3, Operation);
    }
}

