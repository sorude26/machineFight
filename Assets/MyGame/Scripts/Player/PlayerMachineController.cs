using MyGame;
using MyGame.MachineFrame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMachineController : MonoBehaviour
{
    [SerializeField]
    private float _useEnergySpeed = 5f;
    [SerializeField]
    private float _useFloatEnergy = 5f;
    [SerializeField]
    private CameraController _playerCamera = default;
    [SerializeField]
    private MachinePartsController _machineController = default;
    [SerializeField]
    private TargetMark _targetMark = default;
    [SerializeField]
    private PartsBuildParam _buildParam = default;
    [SerializeField]
    private StageUIController _stageUI = default;
    [SerializeField]
    private Transform _headTrans = default;
    [SerializeField]
    private FollowCamera _camera = default;
    [SerializeField]
    private bool _startWaitMode = false;
    [SerializeField]
    private Rigidbody _machineRB = default;
    private float _maxBooster = default;
    private float _currentBooster = default;
    private float _boosterRecoverySpeed = default;
    private float _maxEnergy = default;
    private float _currentEnergy = default;
    private float _energyConsumption = default;
    private float _boosterConsumption = default;
    public Transform HeadTrans { get => _headTrans; }
    public MachinePartsController MachineController => _machineController;
    private IEnumerator Start()
    {
        if (PlayerData.instance != null)
        {
            _buildParam = PlayerData.instance.BuildPreset;
        }
        _machineController.Initialize(_buildParam);
        _machineController.DamageChecker.OnDamageEvent.AddListener(DamagePlayer);
        _machineController.DamageChecker.OnRecoveryEvent.AddListener(ShowHpData);
        if (_startWaitMode == true)
        {
            _machineRB.isKinematic = true;
        }
        else
        {
            SetInput();
        }
        yield return null;
        _stageUI.StartSet(_machineController.BodyController.LeftHand.WeaponBase,
            _machineController.BodyController.RightHand.WeaponBase, _machineController.BodyController.BackPack.BackPackWeapon);
        ShowHpData();
        LockOnController.Instance.LockOnRange = PartsManager.Instance.AllParamData.GetPartsHead(_buildParam.Head).LockOnRange;
        LockOnController.Instance.LockOnSpeed = PartsManager.Instance.AllParamData.GetPartsHead(_buildParam.Head).LockOnSpeed;
        SetParam();
        _machineController.BodyController.UseBooster += UseBooster;
        _machineController.DamageChecker.ChangeAnTarget();
        StageManager.Instance.OnGameEnd += SetTotalDamage;
        _headTrans.SetParent(_machineController.BodyController.HeadJoint.transform);
        if (_startWaitMode == true)
        {
            _machineController.BodyController.StartJetBoosters();
        }
    }
    private void FixedUpdate()
    {
        if (_machineController.IsInitalized == false || _startWaitMode == true) { return; }
        BoosterUpdate();
        EnergyUpdate();
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
    }
    private void SetInput()
    {
        PlayerInput.SetEnterInput(InputMode.InGame, InputType.Jump, Jump);
        PlayerInput.SetEnterInput(InputMode.InGame, InputType.Fire2, ShotRight);
        PlayerInput.SetEnterInput(InputMode.InGame, InputType.Fire1, ShotLeft);
        PlayerInput.SetEnterInput(InputMode.InGame, InputType.Fire3, _machineController.ExecuteBurst);
        PlayerInput.SetEnterInput(InputMode.InGame, InputType.Fire4, _machineController.AttackLeg);
        PlayerInput.SetEnterInput(InputMode.InGame, InputType.Booster, JetBoost);
        PlayerInput.SetEnterInput(InputMode.InGame, InputType.ChangeTarget, ChangeTarget);
        PlayerInput.SetEnterInput(InputMode.InGame, InputType.ChangeMode, _camera.ChangeMode);
    }
    private void LiftInput()
    {
        PlayerInput.LiftEnterInput(InputMode.InGame, InputType.Jump, Jump);
        PlayerInput.LiftEnterInput(InputMode.InGame, InputType.Fire2, ShotRight);
        PlayerInput.LiftEnterInput(InputMode.InGame, InputType.Fire1, ShotLeft);
        PlayerInput.LiftEnterInput(InputMode.InGame, InputType.Fire3, _machineController.ExecuteBurst);
        PlayerInput.LiftEnterInput(InputMode.InGame, InputType.Fire4, _machineController.AttackLeg);
        PlayerInput.LiftEnterInput(InputMode.InGame, InputType.Booster, JetBoost);
        PlayerInput.LiftEnterInput(InputMode.InGame, InputType.ChangeTarget, ChangeTarget);
        PlayerInput.LiftEnterInput(InputMode.InGame, InputType.ChangeMode, _camera.ChangeMode);
    }
    private void SetParam()
    {
        _boosterConsumption = _machineController.Builder.BoosterConsumption;
        _boosterRecoverySpeed = _machineController.Builder.BoosterRecoverySpeed;
        _energyConsumption = _machineController.Builder.EnergyConsumption;
        _maxBooster = _machineController.Builder.MaxBooster;
        _maxEnergy = _machineController.Builder.MaxEnergy;
        _currentBooster = 0;
        _currentEnergy = _maxEnergy;
    }
    private void BoosterUpdate()
    {
        if (_currentBooster < _maxBooster)
        {
            _currentBooster += _boosterRecoverySpeed * Time.fixedDeltaTime;
            if (_currentBooster > _maxBooster)
            {
                _currentBooster = _maxBooster;
            }
            _stageUI.BoosterUpdate(_currentBooster, _maxBooster);
        }
    }
    private void EnergyUpdate()
    {
        if (_currentEnergy > 0)
        {
            float useEnergy = _energyConsumption * _useEnergySpeed * Time.fixedDeltaTime;
            if (_machineController.IsFloat)
            {
                useEnergy += useEnergy * _useFloatEnergy * _machineController.BodyController.FloatSpeed;
            }
            _currentEnergy -= useEnergy;
            if (_currentEnergy <= 0)
            {
                LiftInput();
                _machineController.PlayDeadEvent();
            }
            _stageUI.EnergyUpdate(_currentEnergy, _maxEnergy);
        }
    }
    private void ShowHpData()
    {
        _stageUI.ShowHpData(_machineController.DamageChecker.CurrentHp, _machineController.DamageChecker.MaxHp);
    }
    private void DamagePlayer()
    {
        _stageUI.DamagePlayer();
        ShowHpData();
    }
    private void SetTotalDamage()
    {
        StageManager.Instance.TotalDamage = _machineController.DamageChecker.TotalDamage;
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
        if (_currentBooster < _boosterConsumption)
        {
            return;
        }
        _machineController.ExecuteJet(new Vector3(PlayerInput.MoveDir.x, 0, PlayerInput.MoveDir.y));
    }
    public void Jump()
    {
        if (_currentBooster < _boosterConsumption)
        {
            return;
        }
        _machineController.ExecuteJump();
    }
    public void UseBooster()
    {
        _currentBooster -= _boosterConsumption;
    }
    public void ChangeTarget()
    {
        LockOnController.Instance.ChangeTargetNum();
    }
    public void RefillAmmunition(float percent = 0.05f)
    {
        _machineController.BodyController.LeftHand.WeaponBase.RefillAmmunition(percent);
        _machineController.BodyController.RightHand.WeaponBase.RefillAmmunition(percent);
        if (_machineController.BodyController.BackPack.BackPackWeapon != null)
        {
            _machineController.BodyController.BackPack.BackPackWeapon.RefillAmmunition(percent);
        }
    }
    public void RecoveryHp(int point)
    {
        _machineController.DamageChecker.RecoveryHp(point);
    }
    public void RecoveryEnergy(float point)
    {
        if (_currentEnergy <= 0)
        {
            return;
        }
        _currentEnergy += point;
        if (_currentEnergy > _maxEnergy)
        {
            _currentEnergy = _maxEnergy;
        }
    }
    public void EndWaitMode()
    {
        SetInput();
        _startWaitMode = false;
        _machineRB.isKinematic = false;
    }
}
