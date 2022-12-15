using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager: MonoBehaviour
{
    [SerializeField]
    private float _floorRange = 210f;
    [SerializeField]
    private FloorController[] _allFloor = default;
}
