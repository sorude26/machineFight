﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetMark : MonoBehaviour
{
    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private RectTransform _rect = default;
    [SerializeField]
    private Text _rangeText = default;
    [SerializeField]
    private Text _hpText = default;
    [SerializeField]
    private Image _hpGauge = default;
    [SerializeField]
    private GameObject _targetMessage = default;
    [SerializeField]
    private GameObject _bossMessage = default;
    public LockOnTarget Target;
    private bool _isActive = false;

    private void FixedUpdate()
    {
        if (Target == null && _isActive == true)
        {
            _isActive = false;
            _rect.gameObject.SetActive(_isActive);
            _targetMessage.SetActive(false);
            _bossMessage.SetActive(false);
            return;
        }       
        else if (Target == null)
        {
            return;
        }
        else if (_isActive == false)
        {
            _isActive = true;
            _rect.gameObject.SetActive(_isActive); 
        }
        _rangeText.text = Vector3.Distance(Camera.main.transform.position,Target.transform.position).ToString("F2");
        _hpText.text = Target.DamageChecker.CurrentHp.ToString();
        _hpGauge.fillAmount = (float)Target.DamageChecker.CurrentHp / Target.DamageChecker.MaxHp;
        var ajust = RectTransformUtility.WorldToScreenPoint(MainCameraLocator.MainCamera, Target.transform.position);
        ajust -= new Vector2(MainCameraLocator.MainCamera.pixelWidth / 2, MainCameraLocator.MainCamera.pixelHeight / 2);
        _rect.anchoredPosition = ajust;
        _targetMessage.SetActive(Target.DamageChecker.AddTarget);
        _bossMessage.SetActive(Target.DamageChecker.BossTarget);
    }
}
