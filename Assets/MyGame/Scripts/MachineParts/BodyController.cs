using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    public Transform BodyBase = null;
    public Transform Lock = null;
    private void FixedUpdate()
    {
        transform.position = BodyBase.position;
        transform.forward = Lock.forward;
    }
}
