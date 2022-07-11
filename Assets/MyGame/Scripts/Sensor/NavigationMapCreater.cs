using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationMapCreater : MonoBehaviour
{
    [SerializeField]
    private float _pointSpanRange = 1f;
    [SerializeField]
    private Transform _startPoint = null;
    [SerializeField]
    private Transform _endPoint = null;
    [SerializeField]
    private LayerMask _navigationLayer = default;
    private List<Vector3> _naviMap = new List<Vector3>();
    public void CreateMap()
    {
        
    }
    private RaycastHit GetPoint(Vector3 start)
    {
        if (Physics.Raycast(start, Vector3.down, out RaycastHit hit))
        { 
            return hit;
        }
        return default(RaycastHit);
    }
}
