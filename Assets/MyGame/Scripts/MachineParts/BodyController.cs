using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class BodyController : MonoBehaviour
{
    private const float ATTACK_ANGLE = 0.98f;
    [SerializeField]
    private HandController _rHand = default;
    [SerializeField]
    private HandController _lHand = default;
    public Transform BodyBase = null;
    public Transform Lock = null;

    public Transform TestTarget = null;
    public bool Check = false;
    private bool _isShoot = false;
    private void FixedUpdate()
    {
        transform.position = BodyBase.position;
        transform.forward = Lock.forward;
        _lHand?.PartsMotion();
        _rHand?.PartsMotion();

        if (TestTarget != null)
        {
            Vector3 targetDir = TestTarget.position - transform.position;
            targetDir.y = 0.0f;
            if (ChackAngle(targetDir))
            {
                _lHand.SetLockOn(TestTarget.position);
                _rHand.SetLockOn(TestTarget.position);
                _isShoot = true;
            }
            else if (_isShoot == true)
            {
                _lHand.ResetAngle();
                _rHand.ResetAngle();
            }
        }
    }
    private bool ChackAngle(Vector3 targetDir)
    {
        var angle = Vector3.Dot(targetDir.normalized, transform.forward);
        if (Check)
        {
            Debug.Log(angle);
        }
        // return angle > ATTACK_ANGLE || angle < -ATTACK_ANGLE;
        return angle > ATTACK_ANGLE;
    }
    public void ShotLeft()
    {
        _lHand.StartShot();
    }
    public void ShotRight()
    {
        _rHand.StartShot();
    }
}
