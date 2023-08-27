using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActiveEventer : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnActiveEvent;
    [SerializeField]
    private UnityEvent OnHaydEvent;

    private void OnEnable()
    {
        OnActiveEvent?.Invoke();
    }
    private void OnDisable()
    {
        OnHaydEvent?.Invoke();
    }
}
