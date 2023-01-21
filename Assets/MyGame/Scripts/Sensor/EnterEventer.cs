using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnterEventer : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _enterEvent = default;
    [SerializeField]
    private string _eventTag = "Player";
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == _eventTag)
        {
            _enterEvent?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
