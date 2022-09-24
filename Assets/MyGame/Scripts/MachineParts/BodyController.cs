using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 0.5f;
    public Transform BodyJoint = null;
    public Transform LockRoute = null;
    public Transform LegBase = null;
    public Transform LegTrans = null;
    private Quaternion _bodyRotaion;
    private void FixedUpdate()
    {
        _bodyRotaion = Quaternion.Euler(0, MyGame.PlayerInput.CameraDir.x * 90f, 0);
        transform.position = BodyJoint.position;
        LockRoute.localRotation = Quaternion.Lerp(LockRoute.localRotation, _bodyRotaion, _rotationSpeed * Time.fixedDeltaTime);
        //transform.forward = Vector3.Lerp(transform.forward, BodyJoint.forward, _rotationSpeed * Time.fixedDeltaTime);
        var r = LegTrans.rotation;
        LegBase.localRotation = LockRoute.localRotation;
        LegTrans.rotation = r;
    }
}
