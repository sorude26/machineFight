using MyGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    private string _targetScene = default;
    public bool IsSceneChangeMode = default;
    private IEnumerator Start()
    {
        PartsManager.Instance.LoadData();
        FadeController.StartFadeIn(() => { IsSceneChangeMode = true; });
        while (IsSceneChangeMode)
        {
            yield return null;
        }
        PlayerInput.SetEnterInput(InputMode.Menu, InputType.Submit, ChangeNextScene);
        PlayerInput.ChangeInputMode(InputMode.Menu);
    }
    private void ChangeNextScene()
    {
        PlayerInput.Instance.InitializeInput();
        SceneControl.ChangeTargetScene(_targetScene);
    }
}
