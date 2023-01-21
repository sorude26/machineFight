using System;
using MyGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpMessage : MonoBehaviour
{
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
    private Button _submitButton = null;
    [SerializeField]
    private Button _cancelButton = null;
    [SerializeField]
    private Button _otherButton = null;
    private Action _submitDel = default;
    private Action _cancelDel = default;
    private Action _otherDel = default;
    public static void CreatePopUp(PopUpData data, Action submitAction = null, Action cancelAction = null, Action otherAction = null)
    {
        var obj = Instantiate(Resources.Load<PopUpMessage>("Prefabs/PopUpCanvas"));
        obj.Initialized(data, submitAction, cancelAction, otherAction);
    }
    public void Initialized(PopUpData data, Action submitAction, Action cancelAction, Action otherAction)
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
        if (otherAction != null)
        {
            _otherButton.onClick.AddListener(() =>
            {
                PlaySE(_submitSEID);
                otherAction?.Invoke();
            });
            _otherDel = otherAction;
            _otherButton.gameObject.SetActive(true);
        }
        else
        {
            _otherButton.gameObject.SetActive(false);
        }
        PlayerInput.SetEnterInput(InputMode.Menu, InputType.Submit, Submit);
        PlayerInput.SetEnterInput(InputMode.Menu, InputType.Cancel, Cancel);
        PlayerInput.SetEnterInput(InputMode.Menu, InputType.Submit, Other);
        PlayerInput.SetEnterInput(InputMode.Menu, InputType.Cancel, Other);
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
    private void Other()
    {
        _otherDel?.Invoke();
        ClosePopUp();
    }
    public void ClosePopUp()
    {
        Destroy(this.gameObject);
    }
    private void OnDisable()
    {
        PlayerInput.LiftEnterInput(InputMode.Menu, InputType.Submit, Submit);
        PlayerInput.LiftEnterInput(InputMode.Menu, InputType.Cancel, Cancel);
        PlayerInput.LiftEnterInput(InputMode.Menu, InputType.Submit, Other);
        PlayerInput.LiftEnterInput(InputMode.Menu, InputType.Cancel, Other);
    }
}
public struct PopUpData
{
    public string TopText;
    public string CenterText;
    public string MiddleText;
    public string Submit;
    public string Cancel;
    public PopUpData(string top = "", string center = "", string middle = "", string sub = "OK", string cancel = "Cancel")
    {
        TopText = top;
        CenterText = center;
        MiddleText = middle;
        Submit = sub;
        Cancel = cancel;
    }
}
