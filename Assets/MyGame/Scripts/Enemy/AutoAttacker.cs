using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttacker : MonoBehaviour
{

    [Tooltip("カメラ位置")]
    [SerializeField]
    private Transform _cameraTrans = default;
    [Tooltip("障害レイヤー")]
    [SerializeField]
    private LayerMask _wallLayer = default;
}
