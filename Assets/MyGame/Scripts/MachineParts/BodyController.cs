using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    public Transform BodyJoint = null;
    public Transform LockRoute = null;
    private void FixedUpdate()
    {
        transform.position = BodyJoint.position;
        //transform.localRotation = LockRoute.localRotation;
    }
}
