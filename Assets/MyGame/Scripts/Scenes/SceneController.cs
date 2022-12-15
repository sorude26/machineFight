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
    [SerializeField]
    private int _changeBGMID = 28;
    [SerializeField]
    private float _BGMVolume = 0.1f;
    [SerializeField]
    private bool _changeBGM = false; 
    private IEnumerator Start()
    {
        PartsManager.Instance.LoadData();
        yield return new WaitForSeconds(_waitTime);
        PlayerInput.SetEnterInput(InputMode.Menu, InputType.Submit, ChangeNextScene);
        PlayerInput.ChangeInputMode(InputMode.Menu);
    }
    private void ChangeNextScene()
    {
        if (_changeBGM == true)
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlayBGM(_changeBGMID,_BGMVolume);
            }
        }
        PlayerInput.Instance.InitializeInput();
        SceneControl.ChangeTargetScene(_targetScene);
    }
}
