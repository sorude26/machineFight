using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineDataView : MonoBehaviour
{
    [Header("各パラメータに対応するゲージ")]
    [SerializeField]
    private GaugeController[] _dataGauges = default;
    [Header("各パラメータの基準最大値")]
    [SerializeField]
    private TotalParam _maxParam = default;
    /// <summary>
    /// 全体パラメータのゲージ反映を行う
    /// </summary>
    /// <param name="param"></param>
    public void ViewData(TotalParam param)
    {
        if (_dataGauges == null || _dataGauges.Length < TotalParam.PARAM_NUM)
        {
            Debug.Log("設定ゲージ数不足");
            return;
        }
        for (int i = 0; i < TotalParam.PARAM_NUM; i++)
        {
            _dataGauges[i].SetGauge(param[(TotalParam.ParamPattern)i] / _maxParam[(TotalParam.ParamPattern)i]);
        }
    }
    /// <summary>
    /// 全体パラメータのゲージ反映を行う
    /// </summary>
    /// <param name="param"></param>
    public void ViewData(TotalParam param, TotalParam before)
    {
        if (_dataGauges == null || _dataGauges.Length < TotalParam.PARAM_NUM)
        {
            Debug.Log("設定ゲージ数不足");
            return;
        }
        for (int i = 0; i < TotalParam.PARAM_NUM; i++)
        {
            var paramType = (TotalParam.ParamPattern)i;
            _dataGauges[i].SetGauge(param[paramType] / _maxParam[paramType], before[paramType] / _maxParam[paramType]);
        }
    }
}
