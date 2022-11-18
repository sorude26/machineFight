using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AutoActionController : MonoBehaviour
{
    [SerializeField]
    private float _maxInterval = 3f;
    [SerializeField]
    private float _intervalDiffusivity = 0f;
    [SerializeField]
    private UnityEvent _onActionEvent = default;
    [SerializeField]
    private AutoAttacker _autoAttacker = default;
    private float _actionTimer = 0f;
    private void FixedUpdate()
    {
        if (_actionTimer > 0)
        {
            _actionTimer -= Time.fixedDeltaTime;
        }
        else if( _autoAttacker == null || _autoAttacker.IsAttackMode == true)
        {
            ExecuteAction();
        }
    }
    private void ExecuteAction()
    {
        _onActionEvent?.Invoke();
        _actionTimer = Random.Range(_maxInterval - _intervalDiffusivity, _maxInterval);
    }
}
