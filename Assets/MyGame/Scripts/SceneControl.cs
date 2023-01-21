using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneControl
{
    private static bool sceneChange = false;
    private static LoadingViewController loadingView = default;
    public static Action OnSceneChange = default;
    public static void ChangeTargetScene(string sceneName)
    {
        if (sceneChange == true)
        {
            return;
        }
        if (loadingView == null)
        {
            loadingView = LoadingViewController.CreateLoadingView();
        }
        sceneChange = true;
        FadeController.StartFadeOutIn(OnSceneChange, loadingView.LoadScene(sceneName), () => { sceneChange = false; });
    }
}
