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
    /// �ꎞ��~�B�|�[�Y���(�ݒ�Ȃ�)�Ɏg�p
    /// </summary>
    public virtual void PauseSituation()
    {

    }
}
