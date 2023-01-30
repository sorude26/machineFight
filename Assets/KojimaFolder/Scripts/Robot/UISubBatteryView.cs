using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �u�[�X�^�[�Q�[�W��UI(VR�p)
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

        //�G�����A���C��UI�̃Q�[�W��fillAmount�����̂܂ܓK�p����
        float fill = _stageUIController?.BoosterGauge.fillAmount ?? 0;
        _fill.fillAmount = fill;
    }
}
