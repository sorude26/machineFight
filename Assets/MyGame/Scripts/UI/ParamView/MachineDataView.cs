using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineDataView : MonoBehaviour
{
    [Header("�e�p�����[�^�ɑΉ�����Q�[�W")]
    [SerializeField]
    private GaugeController[] _dataGauges = default;
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
            _dataGauges[i].SetGauge(param[(TotalParam.ParamPattern)i] / _maxParam[(TotalParam.ParamPattern)i]);
        }
    }
    /// <summary>
    /// �S�̃p�����[�^�̃Q�[�W���f���s��
    /// </summary>
    /// <param name="param"></param>
    public void ViewData(TotalParam param, TotalParam before)
    {
        if (_dataGauges == null || _dataGauges.Length < TotalParam.PARAM_NUM)
        {
            Debug.Log("�ݒ�Q�[�W���s��");
            return;
        }
        for (int i = 0; i < TotalParam.PARAM_NUM; i++)
        {
            var paramType = (TotalParam.ParamPattern)i;
            _dataGauges[i].SetGauge(param[paramType] / _maxParam[paramType], before[paramType] / _maxParam[paramType]);
        }
    }
}
