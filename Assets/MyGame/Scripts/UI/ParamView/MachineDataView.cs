using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineDataView : MonoBehaviour
{
    [Header("�e�p�����[�^�ɑΉ�����Q�[�W")]
    [SerializeField]
    private Image[] _dataGauges = default;
    [Header("�e�p�����[�^�̊�ő�l")]
    [SerializeField]
    private TotalParam _maxParam = default;
    /// <summary>
    /// �S�̃p�����[�^�̃Q�[�W���f���s��
    /// </summary>
    /// <param name="param"></param>
    public void ViewData(TotalParam param)
    {
        if (_dataGauges == null || _dataGauges.Length < TotalParam.PARAM_NUM)
        {
            Debug.Log("�ݒ�Q�[�W���s��");
            return;
        }
        for (int i = 0; i < TotalParam.PARAM_NUM; i++)
        {
            _dataGauges[i].fillAmount = param[(TotalParam.ParamPattern)i] / _maxParam[(TotalParam.ParamPattern)i];
        }
    }
}
