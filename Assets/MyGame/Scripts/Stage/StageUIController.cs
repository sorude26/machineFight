using MyGame;
using MyGame.MachineFrame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageUIController : MonoBehaviour
{
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
    public void StartSet(WeaponBase left,WeaponBase right,WeaponBase back)
    {
        void ShowLeft()
        {
            _leftWeapon.ShowWeaponData(left);
        }
        void ShowRight()
        {
            _rightWeapon.ShowWeaponData(right);
        }
        void ShowBackPack()
        {
            _backPack.ShowWeaponData(back);
        }
        left.OnCount += ShowLeft;
        right.OnCount += ShowRight;
        ShowLeft();
        ShowRight();
        if (back != null)
        {
            back.OnCount += ShowBackPack;
            ShowBackPack();
        }
        else
        {
            _backPack.ShowWeaponData(null);
        }
    }
    public void ShowHpData(float currentHp, float maxHp)
    {
        _hpText.text = currentHp.ToString();
        _hpGauge.fillAmount = currentHp / maxHp;
    }
    public void DamagePlayer()
    {
        _uiAnime.Play(_damage);
    }
    public void BoosterUpdate(float currentBooster, float maxBooster)
    {
        _boosterGauge.fillAmount = currentBooster / maxBooster;
    }
    public void EnergyUpdate(float currentEnergy, float maxEnergy)
    {
        _energyGauge.fillAmount = currentEnergy / maxEnergy;
    }
}
