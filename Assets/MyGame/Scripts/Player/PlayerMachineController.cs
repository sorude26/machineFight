using MyGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class PlayerMachineController : MonoBehaviour
{
    [SerializeField]
    private CameraController _playerCamera = default;
    [SerializeField]
    private MachinePartsController _machineController = default;
    [SerializeField]
    private TargetMark _targetMark = default;
    [SerializeField]
    private Text _hpText = default;
    [SerializeField]
    private Image _hpGauge = default;
    private void Start()
    {
        PlayerInput.SetEnterInput(InputMode.InGame,InputType.Jump, _machineController.ExecuteJump);
        PlayerInput.SetEnterInput(InputMode.InGame, InputType.Fire2, ShotRight);
        PlayerInput.SetEnterInput(InputMode.InGame, InputType.Fire1, ShotLeft);
        PlayerInput.SetEnterInput(InputMode.InGame, InputType.Fire3, _machineController.ExecuteBurst);
        PlayerInput.SetEnterInput(InputMode.InGame, InputType.Booster, JetBoost);
        PlayerInput.SetEnterInput(InputMode.InGame, InputType.ChangeTarget, ChangeTarget);
        _machineController.Initialize();
    }
    private void FixedUpdate()
    {
        if (_machineController.IsInitalized == false) { return; }
        _playerCamera.FreeLock(PlayerInput.CameraDir);
        var dir = new Vector3(PlayerInput.MoveDir.x, 0, PlayerInput.MoveDir.y);
        var locktarget = LockOnController.Instance.GetTarget();
        _targetMark.Target = locktarget;
        if (locktarget == null)
        {
            _machineController.SetLockOn(null);
        }
        else
        {
            _machineController.SetLockOn(locktarget.transform);
        }
        _machineController.ExecuteFixedUpdate(dir);
        _hpText.text = _machineController.DamageChecker.CurrentHp.ToString();
        _hpGauge.fillAmount = 1f - (float)_machineController.DamageChecker.CurrentHp / _machineController.DamageChecker.MaxHp;
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
