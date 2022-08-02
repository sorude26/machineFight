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
            PlayerInput.SetEnterInput(InputType.ChangeMode, _machineController.InputChangeMode);
            PlayerInput.SetEnterInput(InputType.Fire1, _machineController.InputAttack1);
        }
        private void FixedUpdate()
        {
            _machineController.MoveDir = PlayerInput.MoveDir;
        }
    }
}