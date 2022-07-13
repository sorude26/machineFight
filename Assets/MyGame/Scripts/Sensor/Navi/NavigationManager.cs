using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    public static NavigationManager Instance { get; private set; }
    [SerializeField]
    private NavigationMapCreater _mapCreater = default;
    public StageNavigation Navigation { get; private set; }
    public Stack<IEnumerator> NavigationStack { get; private set; } = new Stack<IEnumerator>();
    private void Awake()
    {
        Instance = this;
        Navigation = _mapCreater.CreateMap();
        Navigation.Initialization();
    }
    private IEnumerator Start()
    {
        while (true)
        {
            while (NavigationStack.Count > 0)
            {
                yield return NavigationStack.Pop();
            }
            yield return null;
        }
    }
}
