using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorUnscaledTimeChanger : MonoBehaviour
{
    void Start()
    {
        GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;
    }


}
