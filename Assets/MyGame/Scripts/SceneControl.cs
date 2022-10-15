using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneControl 
{
    private static bool sceneChange = false;
    public static void ChangeTargetScene(string sceneName)
    {
        if (!sceneChange)
        {
            sceneChange = true;
            FadeController.StartFadeOutIn(() =>
            {
                SceneManager.LoadScene(sceneName);
                sceneChange = false;
            });
        }
    }
}
