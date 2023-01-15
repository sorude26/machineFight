using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDataView : MonoBehaviour
{
    [SerializeField]
    private Text _current = default;
    [SerializeField]
    private Text _currentBack = default;
    [SerializeField]
    private Text _magazine = default;
    [SerializeField]
    private Text _magazineBack = default;
    [SerializeField]
    private Text _total = default;
    [SerializeField]
    private Text _totalBack = default;
    [SerializeField]
    private Text _max = default;
    [SerializeField]
    private Text _maxBack = default;
    [SerializeField]
    private Image _countGauge = default;
    [SerializeField]
    private Image _totalGauge = default;
    [SerializeField]
    private bool _currentView = false;
    [SerializeField]
    private Color _outOfAmmoColor = default;
    [SerializeField]
    private GameObject _arrowMark = default;
    [SerializeField]
    private GameObject _bulletMark = default;
    [SerializeField]
    private GameObject _outOfAmmo = default;
    private Color _startColor = default;
    private bool _isStart = false;
    public void ShowWeaponData(WeaponBase weapon)
    {
        if (weapon == null)
        {
            gameObject.SetActive(false);
            return;
        }
        if (_isStart == false)
        {
            StartSet(weapon);
        }
        if (_currentView == true)
        {
            CurrentView(weapon);
        }
        else
        {
            AllView(weapon);
        }
    }
    private void StartSet(WeaponBase weapon)
    {
        if (weapon.Type != WeaponType.HandGun)
        {
            _arrowMark.SetActive(true);
            _bulletMark.SetActive(false);
        }
        _startColor = _current.color;
        _isStart = true;
    }
    /// <summary>
    /// ”pŽ~—\’è
    /// </summary>
    /// <param name="weapon"></param>
    private void AllView(WeaponBase weapon)
    {
        if (weapon.MaxAmmunitionCapacity <= 0)
        {
            _total.text = "‡";
            _totalGauge.fillAmount = 1;
        }
        else
        {
            _total.text = $"{weapon._currentAmmunition} / {weapon.MaxAmmunitionCapacity}";
            _totalGauge.fillAmount = (float)weapon._currentAmmunition / weapon.MaxAmmunitionCapacity;
        }
        if (weapon.MagazineCount <= 0)
        {
            _current.text = "‡";
            _countGauge.fillAmount = 1;
        }
        else
        {
            _current.text = $"{weapon._currentMagazine} / {weapon.MagazineCount}";
            _countGauge.fillAmount = (float)weapon._currentMagazine / weapon.MagazineCount;
        }
    }
    private void CurrentView(WeaponBase weapon)
    {
        if (weapon.MaxAmmunitionCapacity <= 0 || weapon.Type != WeaponType.HandGun)
        {
            _total.text = "";
            _totalBack.text = "";
            _max.text = "";
            _maxBack.text = "";
            _totalGauge.fillAmount = 1;
        }
        else
        {
            _total.text = weapon._currentAmmunition.ToString();
            _totalBack.text = string.Format("{0:0000}", weapon._currentAmmunition);
            _max.text = weapon.MaxAmmunitionCapacity.ToString();
            _maxBack.text = "/" + string.Format("{0:0000}", weapon.MaxAmmunitionCapacity);
            _totalGauge.fillAmount = (float)weapon._currentAmmunition / weapon.MaxAmmunitionCapacity;
            _total.color = _startColor;
            if (weapon._currentAmmunition == 0)
            {
                _total.color = _outOfAmmoColor;
            }
            _outOfAmmo.SetActive(weapon._currentAmmunition == 0);
        }
        if (weapon.MagazineCount <= 0 || weapon.Type != WeaponType.HandGun)
        {
            _current.text = "";
            _currentBack.text = "";
            _magazine.text = "";
            _magazineBack.text = "";
            _countGauge.fillAmount = 1;
        }
        else
        {
            _current.text = weapon._currentMagazine.ToString();
            _currentBack.text = string.Format("{0:000}", weapon._currentMagazine);
            _magazine.text = weapon.MagazineCount.ToString();
            _magazineBack.text = "/" + string.Format("{0:000}", weapon.MagazineCount);
            _countGauge.fillAmount = (float)weapon._currentMagazine / weapon.MagazineCount;
            _current.color = _startColor;
            if (weapon._currentMagazine == 0)
            {
                _current.text = "RELOAD";
                _currentBack.text = "";
                _current.color = _outOfAmmoColor;
            }
        }
    }
}
