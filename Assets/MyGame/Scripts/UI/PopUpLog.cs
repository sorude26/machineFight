using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpLog : MonoBehaviour
{
    [SerializeField]
    private Text _logText = default;
    private float viewTimer = 0;
    public static void CreatePopUp(string log,float viewTime = 1f)
    {
        var obj = Instantiate(Resources.Load<PopUpLog>("Prefabs/PopUpLog"));
        obj.Initialized(log,viewTime);
    }
    public void Initialized(string log,float viewTime)
    {
        viewTimer = viewTime;
        _logText.text = log;
        StartCoroutine(ViewLog());
    }
    private IEnumerator ViewLog()
    {
        while (viewTimer > 0)
        {
            viewTimer -= Time.deltaTime;
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
