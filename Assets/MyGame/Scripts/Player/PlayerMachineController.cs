using MyGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMachineController : MonoBehaviour
{
    [SerializeField]
    private CameraController _playerCamera = default;
    [SerializeField]
    private MachinePartsController _machineController = default;
    [SerializeField]
    private TargetMark _targetMark = default;
    [SerializeField]
    private PartsBuildParam _buildParam = default;
    [SerializeField]
    private WeaponDataView _leftWeapon = default;
    [SerializeField]
    private WeaponDataView _rightWeapon = default;
    [SerializeField]
    private WeaponDataView _backPack = default;
    [SerializeField]
    private Image _boosterGauge = default;
    [SerializeField]
    private Image _energyGauge = default;
    [SerializeField]
    private Text _hpText = default;
    [SerializeField]
    private Image _hpGauge = default;
    [SerializeField]
    private Animator _uiAnime = default;
    private string _damage = "Damage";
    private float _maxBooster = default;
    private float _currentBooster = default;
    private float _boosterRecoverySpeed = default;
    private float _maxEnergy = default;
    private float _currentEnergy = default;
    private float _energyConsumption = default;
    private float _boosterConsumption = default;
    private IEnumerator Start()
    {
        SetInput();
        _machineController.Initialize(_buildParam);
        _machineController.DamageChecker.OnDamageEvent.AddListener(DamagePlayer);
        _machineController.DamageChecker.OnRecoveryEvent.AddListener(ShowHpData);
        void ShowLeft()
        {
            _leftWeapon.ShowWeaponData(_machineController.BodyController.LeftHand.WeaponBase);
        }
        void ShowRight()
        {
            _rightWeapon.ShowWeaponData(_machineController.BodyController.RightHand.WeaponBase);
        }
        void ShowBackPack()
        {
            _backPack.ShowWeaponData(_machineController.BodyController.BackPack.BackPackWeapon);
        }
        yield return null;        
        _machineController.BodyController.LeftHand.WeaponBase.OnCount += ShowLeft;
        _machineController.BodyController.RightHand.WeaponBase.OnCount += ShowRight;
        ShowHpData();
        ShowLeft();
        ShowRight();
        if (_machineController.BodyController.BackPack.BackPackWeapon != null)
        {
            _machineController.BodyController.BackPack.BackPackWeapon.OnCount += ShowBackPack;
            ShowBackPack();
        }
        else
        {
            _backPack.ShowWeaponData(null);
        }
        LockOnController.Instance.LockOnRange = PartsManager.Instance.AllParamData.GetPartsHead(_buildParam.Head).LockOnRange;
        LockOnController.Instance.LockOnSpeed = PartsManager.Instance.AllParamData.GetPartsHead(_buildParam.Head).LockOnSpeed;
        SetParam();
        _machineController.BodyController.UseBooster += UseBooster;
    }
    private void FixedUpdate()
    {
        if (_machineController.IsInitalized == false) { return; }
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
        PlayerInput.SetEnterInput(InputMode.InGame, InputType.Booster, JetBoost);
        PlayerInput.SetEnterInput(InputMode.InGame, InputType.ChangeTarget, ChangeTarget);
    }
    private void LiftInput()
    {
        PlayerInput.LiftEnterInput(InputMode.InGame, InputType.Jump, Jump);
        PlayerInput.LiftEnterInput(InputMode.InGame, InputType.Fire2, ShotRight);
        PlayerInput.LiftEnterInput(InputMode.InGame, InputType.Fire1, ShotLeft);
        PlayerInput.LiftEnterInput(InputMode.InGame, InputType.Fire3, _machineController.ExecuteBurst);
        PlayerInput.LiftEnterInput(InputMode.InGame, InputType.Booster, JetBoost);
        PlayerInput.LiftEnterInput(InputMode.InGame, InputType.ChangeTarget, ChangeTarget);
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
            _boosterGauge.fillAmount = _currentBooster / _maxBooster;
        }
    }
    private void EnergyUpdate()
    {
        if (_currentEnergy > 0)
        {
            _currentEnergy -= _energyConsumption * Time.fixedDeltaTime;
            _energyGauge.fillAmount = _currentEnergy / _maxEnergy;
            if (_currentEnergy <= 0)
            {
                LiftInput();
            }
        }
    }
    private void ShowHpData()
    {
        _hpText.text = _machineController.DamageChecker.CurrentHp.ToString();
        _hpGauge.fillAmount = (float)_machineController.DamageChecker.CurrentHp / _machineController.DamageChecker.MaxHp;
    }
    private void DamagePlayer()
    {
        _uiAnime.Play(_damage);
        ShowHpData();
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
}
