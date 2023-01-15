using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneControl
{
    private static bool sceneChange = false;
    public static Action OnSceneChange = default;
    public static void ChangeTargetScene(string sceneName)
    {
        if (sceneChange == true)
        {
            return;
        }
        sceneChange = true;
        FadeController.StartFadeOutIn(() => {
            OnSceneChange?.Invoke();
            SceneManager.LoadScene(sceneName);
        }, () => {
            sceneChange = false;
        });
    }
}
