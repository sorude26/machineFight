using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    public static NavigationManager Instance { get; private set; }
    [SerializeField]
    private NavigationMapCreater _mapCreater = default;
    [SerializeField]
    private float _updateIntervalTime = 1f;
    [SerializeField]
    private Transform _target = default;
    [SerializeField]
    private int _range = 100;
    [SerializeField]
    private LayerMask _obstacleLayer = default;
    [SerializeField]
    private int _maxRayCount = 500;
    private float _rayRange = 5f;
    private WaitForSeconds _updateInterval = default;
    private NavigationMap _navMap = default;
    public Transform Target { get => _target; }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        StartNavigation();
    }
    private void StartNavigation()
    {
        _updateInterval = new WaitForSeconds(_updateIntervalTime);
        _navMap = _mapCreater.CreateMap();
        _navMap.Initialization();
        StartCoroutine(NavigationUpdate());
    }
    private IEnumerator NavigationUpdate()
    {
        while (true)
        {
            yield return PointUpDate();
            _navMap.MakeFootprints(_target, _range);
            yield return _updateInterval;
        }
    }
    private IEnumerator PointUpDate()
    {
        if (_navMap is null)
        {
            yield break;
        }
        int count = _maxRayCount;
        foreach (var navMap in _navMap.NaviMap)
        {
            navMap.IsNoEntry = Physics.Raycast(navMap.Pos, Vector3.up, _rayRange, _obstacleLayer);
            count--;
            if (count < 0)
            {
                count = _maxRayCount;
                yield return null;
            }
        }
    }
    public Vector3 GetMoveDir(Transform user,int power = 0)
    {
        if (_navMap is null)
        {
            return Vector3.zero;
        }
        return _navMap.GetMoveDir(user,power);
    }
}
