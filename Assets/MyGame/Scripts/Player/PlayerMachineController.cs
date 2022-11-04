using MyGame;
using MyGame.MachineFrame;
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
    private Text _hpText = default;
    [SerializeField]
    private Image _hpGauge = default;
    [SerializeField]
    private Animator _uiAnime = default;
    private string _damage = "Damage";
    private IEnumerator Start()
    {
        SetInput();
        _machineController.Initialize(_buildParam);
        _machineController.DamageChecker.OnDamageEvent.AddListener(DamagePlayer);
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
        LockOnController.Instance.LockOnRange = PartsManager.Instance.AllParamData.GetPartsHead(_buildParam.Head).LockOnRange;
        LockOnController.Instance.LockOnSpeed = PartsManager.Instance.AllParamData.GetPartsHead(_buildParam.Head).LockOnSpeed;
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
    }
    private void SetInput()
    {
        PlayerInput.SetEnterInput(InputMode.InGame, InputType.Jump, _machineController.ExecuteJump);
        PlayerInput.SetEnterInput(InputMode.InGame, InputType.Fire2, ShotRight);
        PlayerInput.SetEnterInput(InputMode.InGame, InputType.Fire1, ShotLeft);
        PlayerInput.SetEnterInput(InputMode.InGame, InputType.Fire3, _machineController.ExecuteBurst);
        PlayerInput.SetEnterInput(InputMode.InGame, InputType.Booster, JetBoost);
        PlayerInput.SetEnterInput(InputMode.InGame, InputType.ChangeTarget, ChangeTarget);
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
        _machineController.ExecuteJet(new Vector3(PlayerInput.MoveDir.x, 0, PlayerInput.MoveDir.y));
    }
    public void ChangeTarget()
    {
        LockOnController.Instance.ChangeTargetNum();
    }
}
