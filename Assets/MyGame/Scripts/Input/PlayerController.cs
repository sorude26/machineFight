using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame 
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private MachineFrame.MachineController _machineController = default;
        private void Start()
        {
            PlayerInput.SetEnterInput(InputType.Jump, _machineController.InputJump);
        }
        private void FixedUpdate()
        {
            _machineController.MoveDir = PlayerInput.MoveDir;
        }
    }
}