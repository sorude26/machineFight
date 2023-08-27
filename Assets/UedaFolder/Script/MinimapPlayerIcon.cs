using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapPlayerIcon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.rotation = new Quaternion(0f,0f,-Camera.main.transform.rotation.y, Camera.main.transform.rotation.w) ;
    }
}
