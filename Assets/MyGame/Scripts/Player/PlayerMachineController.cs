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
    [SerializeField]
    private TargetMark _targetMark = default;
    private void Start()
    {
        PlayerInput.SetEnterInput(InputType.Jump, _machineController.ExecuteJump);
        PlayerInput.SetEnterInput(InputType.Fire2, ShotRight);
        PlayerInput.SetEnterInput(InputType.Fire1, ShotLeft);
        PlayerInput.SetEnterInput(InputType.Booster, JetBoost);
        PlayerInput.SetEnterInput(InputType.ChangeTarget, ChangeTarget);
        _machineController.Initialize();
    }
    private void FixedUpdate()
    {
        if (_machineController.IsInitalized == false) { return; }
        _playerCamera.FreeLock(PlayerInput.CameraDir);
        var dir = new Vector3(PlayerInput.MoveDir.x, 0, PlayerInput.MoveDir.y);
        var locktarget = LockOnController.Instance.GetTarget();
        _targetMark.Target = locktarget;
        _machineController.SetLockOn(locktarget);
        _machineController.ExecuteFixedUpdate(dir);
    }
    public void ShotLeft()
    {
        _machineController.ShotLeft();
    }
    public void ShotRight()
    {
        _machineController.ShotRight();
    }
    public void JetBoost()
    {
        _machineController.ExecuteJet(new Vector3(PlayerInput.MoveDir.x, 0, PlayerInput.MoveDir.y));
    }
    public void ChangeTarget()
    {
        LockOnController.Instance.ChangeTargetNum();
    }
}
