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
    private int _power = 20;
    private WaitForSeconds _updateInterval = default;
    public StageNavigation Navigation { get; private set; }
    public Stack<IEnumerator> NavigationStack { get; private set; } = new Stack<IEnumerator>();
    private void Awake()
    {
        Instance = this;
        Navigation = _mapCreater.CreateMap();
        Navigation.Initialization();
        _updateInterval = new WaitForSeconds(_updateIntervalTime);
    }
    private void Start()
    {
        StartCoroutine(NavigationUpdate());
    }
    private IEnumerator NavigationUpdate()
    {
        while (true)
        {
            Navigation.MakeFootprints(_target, _power);
            yield return _updateInterval;
        }
    }
    //private IEnumerator NavigationUpdate()
    //{
    //    while (true)
    //    {
    //        while (NavigationStack.Count > 0)
    //        {
    //            yield return NavigationStack.Pop();
    //        }
    //        yield return null;
    //    }
    //}
}
