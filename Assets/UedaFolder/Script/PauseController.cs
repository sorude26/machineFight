using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame;
using static UnityEngine.GraphicsBuffer;

public class PauseController : MonoBehaviour
{
    private string _returnScene = "StageSelect";


    void Start()
    {
        PlayerInput.SetEnterInput(InputMode.InGame, InputType.Cancel, ReturnStageSelectScene);      
    }

    private void ReturnStageSelectScene()
    {
        PlayerInput.ChangeInputMode(InputMode.Menu);
        Time.timeScale = 0f;
        var message = new PopUpData(middle: $"Pause’†", sub: "Z:‹AŠÒ‚·‚é", cancel: "~:‘±‚¯‚é");
        PausePopUpMessage.CreatePopUp(message, "¢:‘€ìà–¾",
        submitAction: () => {
            Time.timeScale = 1f;
            SceneControl.ChangeTargetScene(_returnScene);
        },
        cancelAction: () => {
            Time.timeScale = 1f;
            PlayerInput.ChangeInputMode(InputMode.InGame);
            ; });
    }

}
