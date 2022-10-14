using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace MyGame
{
    public class MachinePartsController : MonoBehaviour
    {
        [SerializeField]
        private BodyController _bodyController = default;
        [SerializeField]
        private LegController _legController = default;
        public void ExecuteFixedUpdate(Vector3 dir)
        {
            _legController.ExecuteFixedUpdate(dir);
        }
        public void ExecuteJump()
        {
            _legController.Jump();
        }
        public void ShotLeft()
        {
            _bodyController.ShotLeft();
        }
        public void ShotRight()
        {
            _bodyController.ShotRight();
        }
    }
}