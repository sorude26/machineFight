using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LoadingViewController : MonoBehaviour
{
    private static LoadingViewController instance = default;
    private const float MAX_PROGRESS = 0.9f;
    [SerializeField]
    private Image _loadGauge = default;
    public static LoadingViewController CreateLoadingView()
    {
        if (instance == null)
        {
            var obj = Instantiate(Resources.Load<LoadingViewController>("Prefabs/LoadingCanvas"));
            DontDestroyOnLoad(obj);
            obj.gameObject.SetActive(false);
            instance = obj;
        }
        return instance;
    }
    public IEnumerator LoadScene(string targetScene)
    {
        gameObject.SetActive(true);
        AsyncOperation async = SceneManager.LoadSceneAsync(targetScene);
        while (!async.isDone)
        {
            _loadGauge.fillAmount = async.progress / MAX_PROGRESS;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
