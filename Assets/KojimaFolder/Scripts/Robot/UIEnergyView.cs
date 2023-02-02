using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �G�l���M�[�Q�[�WUI(VR�p)
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

        //�G�����A���C��UI�̃Q�[�W��fillAmount�����̂܂ܓK�p����
        float fill = _stageUIController?.EnergyGauge.fillAmount ?? 0;
        _fill.fillAmount = fill;

        //�G�l���M�[�����Ȃ��Ƃ��͌x���\��
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
