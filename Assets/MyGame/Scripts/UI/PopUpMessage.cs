using System;
using MyGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpMessage : MonoBehaviour
{
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
            _cancelButton.onClick.AddListener(() => { cancelAction?.Invoke(); });
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
            _submitButton.onClick.AddListener(() => { submitAction?.Invoke(); });
            _submitDel = submitAction;
            _submitButton.gameObject.SetActive(true);
        }
        else
        {
            _submitButton.gameObject.SetActive(false);
        }
        if (otherAction != null)
        {
            _otherButton.onClick.AddListener(() => { otherAction?.Invoke(); });
            _otherDel = otherAction;
            _otherButton.gameObject.SetActive(true);
        }
        else
        {
            _otherButton.gameObject.SetActive(false);
        }
        PlayerInput.SetEnterInput(InputMode.Menu, InputType.Submit, Submit);
        PlayerInput.SetEnterInput(InputMode.Menu, InputType.Cancel, Cancel);
        PlayerInput.ChangeInputMode(InputMode.Menu);
    }
    private void Submit()
    {
        _submitDel?.Invoke();
        ClosePopUp();
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
        PlayerInput.LiftEnterInput(InputMode.Menu, InputType.Submit, Submit);
        PlayerInput.LiftEnterInput(InputMode.Menu, InputType.Cancel, Cancel);
        Destroy(this.gameObject);
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
