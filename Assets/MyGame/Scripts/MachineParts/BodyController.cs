using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    public Transform BodyJoint = null;
    private void FixedUpdate()
    {
        if (BodyJoint != null)
        {
            transform.position = BodyJoint.position;
        }
    }
}
