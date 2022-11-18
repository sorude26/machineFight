using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneChangeButton : MonoBehaviour
{
    [SerializeField] string _nextSceneName = default;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(ChangeScene);   
    }

    private void ChangeScene()
    {
        SceneControl.ChangeTargetScene(_nextSceneName);
    }
}
