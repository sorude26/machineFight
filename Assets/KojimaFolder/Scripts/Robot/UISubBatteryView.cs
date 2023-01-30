using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ブースターゲージのUI(VR用)
/// </summary>
public class UISubBatteryView : MonoBehaviour
{
    [SerializeField]
    StageUIController _stageUIController;
    [SerializeField]
    Image _fill;

    private void FixedUpdate()
    {
        if (_stageUIController == null) return;

        //雑だが、メインUIのゲージのfillAmountをそのまま適用する
        float fill = _stageUIController?.BoosterGauge.fillAmount ?? 0;
        _fill.fillAmount = fill;
    }
}
