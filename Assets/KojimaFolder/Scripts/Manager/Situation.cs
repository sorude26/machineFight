using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Situation : MonoBehaviour
{
    public virtual void StartSituation()
    {
        this.gameObject.SetActive(true);
    }

    public virtual void SituationUpdate()
    {

    }

    public virtual void EndSituation()
    {

    }

    /// <summary>
    /// 一時停止。ポーズ画面(設定など)に使用
    /// </summary>
    public virtual void PauseSituation()
    {

    }
}
