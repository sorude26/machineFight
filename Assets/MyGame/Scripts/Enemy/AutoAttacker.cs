using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttacker : MonoBehaviour
{

    [Tooltip("�J�����ʒu")]
    [SerializeField]
    private Transform _cameraTrans = default;
    [Tooltip("��Q���C���[")]
    [SerializeField]
    private LayerMask _wallLayer = default;
}
