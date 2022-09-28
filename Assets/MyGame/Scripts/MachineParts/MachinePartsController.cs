using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    }
}