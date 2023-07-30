using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲージの表示操作
/// </summary>
public class GaugeController : MonoBehaviour
{
    [SerializeField]
    private Image _frontGauge = default;
    [SerializeField]
    private Image _backGauge = default;
    [SerializeField]
    private Color _upperColor = Color.green;
    [SerializeField]
    private Color _downColor = Color.red;
    [SerializeField]
    private float _alphaScale = 0.5f;
    [SerializeField]
    private float _speed = 4f;
    private Color _alphaValue = Color.white;

    private void LateUpdate()
    {
        _alphaValue.a = 1 - Mathf.Abs(Mathf.Sin(Time.time * _speed)) * _alphaScale;
        _backGauge.color = _alphaValue;
    }

    /// <summary>
    /// 現在値の表示
    /// </summary>
    /// <param name="value"></param>
    public void SetGauge(float value)
    {
        _frontGauge.fillAmount = value;
        _backGauge.fillAmount = 0;
    }

    /// <summary>
    /// 初期値との差分表示
    /// </summary>
    /// <param name="value"></param>
    /// <param name="before"></param>
    public void SetGauge(float value, float before)
    {
        if (value > before)
        {
            _frontGauge.fillAmount = before;
            _backGauge.fillAmount = value;
            _alphaValue = _upperColor;
            return;
        }
        _frontGauge.fillAmount = value;
        _backGauge.fillAmount = before;
        _alphaValue = _downColor;
    }
}
