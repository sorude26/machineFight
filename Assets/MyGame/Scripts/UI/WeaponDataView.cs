using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDataView : MonoBehaviour
{
    [SerializeField]
    private Text _current = default;
    [SerializeField]
    private Text _total = default;
    [SerializeField]
    private Image _countGauge = default;
    [SerializeField]
    private Image _totalGauge = default;
    public void ShowWeaponData(WeaponBase weapon)
    {
        if (weapon == null)
        {
            gameObject.SetActive(false);
            return;
        }
        if (weapon.MaxAmmunitionCapacity <= 0)
        {
            _total.text = "‡";
        }
        else
        {
            _total.text = $"{weapon._currentAmmunition} / { weapon.MaxAmmunitionCapacity}";
            _totalGauge.fillAmount = (float)weapon._currentAmmunition / weapon.MaxAmmunitionCapacity;
        }
        if (weapon.MagazineCount <= 0)
        {
            _current.text = "‡";
        }
        else
        {
            _current.text = $"{weapon._currentMagazine} / {weapon.MagazineCount}";
            _countGauge.fillAmount = (float)weapon._currentMagazine / weapon.MagazineCount;
        }
    }
}
