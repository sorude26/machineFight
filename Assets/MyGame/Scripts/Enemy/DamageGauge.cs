using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageGauge : MonoBehaviour
{
    [SerializeField]
    private DamageChecker _checker = default;
    [SerializeField]
    private Image _gauge = default;
    private void Start()
    {
        _checker.OnDamageEvent.AddListener(ShowHp);
    }
    public void ShowHp()
    {
        _gauge.fillAmount = (float)_checker.CurrentHp / _checker.MaxHp;
    }
}
