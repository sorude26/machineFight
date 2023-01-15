using MyGame;
using MyGame.MachineFrame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageUIController : MonoBehaviour
{
    private const float NORMAL_LINE = 0.8f;
    private const float HALF_LINE = 0.5f;
    private const float LESS_LINE = 0.2f;
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
    private Image _energySubGauge = default;
    [SerializeField]
    private Text _energyText = default;
    [SerializeField]
    private Color _energyNormalColor = default;
    [SerializeField]
    private Color _energyHalfColor = default;
    [SerializeField]
    private Color _energyLessColor = default;
    [SerializeField]
    private Text _hpText = default;
    [SerializeField]
    private Text _hpBackText = default;
    [SerializeField]
    private Text _maxHpText = default;
    [SerializeField]
    private Image _hpGauge = default;
    [SerializeField]
    private Color _hpNormalColor = default;
    [SerializeField]
    private Color _hpHalfColor = default;
    [SerializeField]
    private Color _hpLessColor = default;
    [SerializeField]
    private Animator _uiAnime = default;
    private string _damage = "Damage";
    private Color _startHpColor = default;
    private Color _startEnergyColor = default;
    public void StartSet(WeaponBase left, WeaponBase right, WeaponBase back)
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
        _startEnergyColor = _energyGauge.color;
        _startHpColor = _hpGauge.color;
    }
    public void ShowHpData(float currentHp, float maxHp)
    {
        _hpBackText.text = string.Format("{0:0000}", currentHp);
        _hpText.text = currentHp.ToString();
        float level = currentHp / maxHp;
        _hpGauge.fillAmount = level;
        if (_maxHpText != null)
        {
            _maxHpText.text = "/" + string.Format("{0:0000}", maxHp);
        }
        if (level < LESS_LINE)
        {
            _hpGauge.color = _hpLessColor;
            if (currentHp == 0)
            {
                _hpText.text = "";
            }
        }
        else if (level < HALF_LINE)
        {
            _hpGauge.color = _hpHalfColor;
        }
        else if (level < NORMAL_LINE)
        {
            _hpGauge.color = _hpNormalColor;
        }
        else
        {
            _hpGauge.color = _startHpColor;
        }
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
        float level = currentEnergy / maxEnergy;
        _energyGauge.fillAmount = level;
        _energySubGauge.fillAmount = level;
        if (_energyText != null)
        {
            _energyText.text = string.Format("{0:000}", currentEnergy);
        }
        if (level < LESS_LINE)
        {
            _energyGauge.color = _energyLessColor;
        }
        else if (level < HALF_LINE)
        {
            _energyGauge.color = _energyHalfColor;
        }
        else if (level < NORMAL_LINE)
        {
            _energyGauge.color = _energyNormalColor;
        }
        else
        {
            _energyGauge.color = _startEnergyColor;
        }
    }
}
