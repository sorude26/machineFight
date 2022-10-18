using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageChecker : MonoBehaviour, IDamageApplicable
{
    [SerializeField]
    private int _maxHp = 100;
    [SerializeField]
    private UnityEvent _onDeadEvent = default;
    [SerializeField]
    private UnityEvent _onDamageEvent = default;
    [SerializeField]
    private GameObject _deadEffect = default;
    private int _hp = 1;
    public int CurrentHp { get { return _hp; } }
    private void Start()
    {
        _hp = _maxHp;
    }
    public void AddlyDamage(int damage)
    {
        if (_hp <= 0) { return; }
        _hp -= damage;
        _onDamageEvent?.Invoke();
        if (_hp <= 0)
        {
            _hp = 0;
            _onDeadEvent?.Invoke();
        }
    }
    public void PlayEffect()
    {
        var effect = ObjectPoolManager.Instance.LimitUse(_deadEffect);
        effect.transform.position = transform.position;
        effect.transform.rotation = transform.rotation;
        effect.SetActive(true);
    }
}
