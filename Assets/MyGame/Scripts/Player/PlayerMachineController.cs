using MyGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMachineController : MonoBehaviour
{
    [SerializeField]
    private CameraController _playerCamera = default;
    [SerializeField]
    private MachinePartsController _machineController = default;
    [SerializeField]
    private Transform _lockTrans = default;
    private void Start()
    {
        PlayerInput.SetEnterInput(InputType.Jump, _machineController.ExecuteJump);
    }
    private void FixedUpdate()
    {
        _playerCamera.FreeLock(PlayerInput.CameraDir);
        var dir = new Vector3(PlayerInput.MoveDir.x, 0, PlayerInput.MoveDir.y);
        _machineController.ExecuteFixedUpdate(dir);
    }
}
