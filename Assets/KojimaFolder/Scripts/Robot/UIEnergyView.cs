using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// エネルギーゲージUI(VR用)
/// </summary>
public class UIEnergyView : MonoBehaviour
{
    const float CAUSION_AMOUNT = 0.25f;
    [SerializeField]
    StageUIController _stageUIController;
    [SerializeField]
    Image _fill;
    [SerializeField]
    GameObject _cautionText;

    private void FixedUpdate()
    {
        if (_stageUIController == null) return;

        //雑だが、メインUIのゲージのfillAmountをそのまま適用する
        float fill = _stageUIController?.EnergyGauge.fillAmount ?? 0;
        _fill.fillAmount = fill;

        //エネルギーが少ないときは警告表示
        if(fill < CAUSION_AMOUNT)
        {
            _cautionText.SetActive(true);
        }
        else
        {
            _cautionText.SetActive(false);
        }
    }
}
