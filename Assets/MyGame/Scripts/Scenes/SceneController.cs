using MyGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    private string _targetScene = default;
    public float _waitTime = 2;
    public bool IsSceneChangeMode = default;
    private IEnumerator Start()
    {
        PartsManager.Instance.LoadData();
        yield return new WaitForSeconds(_waitTime);
        PlayerInput.SetEnterInput(InputMode.Menu, InputType.Submit, ChangeNextScene);
        PlayerInput.ChangeInputMode(InputMode.Menu);
    }
    private void ChangeNextScene()
    {
        PlayerInput.Instance.InitializeInput();
        SceneControl.ChangeTargetScene(_targetScene);
    }
}
