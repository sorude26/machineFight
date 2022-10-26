using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageChecker : MonoBehaviour, IDamageApplicable
{
    [SerializeField]
    private int _maxHp = 100;
    [SerializeField]
    private int _severelyDamage = 50;
    [SerializeField]
    private UnityEvent _onDeadEvent = default;
    [SerializeField]
    private UnityEvent _onSeverelyDamagedEvent = default;
    [SerializeField]
    private UnityEvent _onDamageEvent = default;
    [SerializeField]
    private GameObject _deadEffect = default;
    [SerializeField]
    private ShakeParam _deadShakeParam = default;
    [SerializeField]
    private bool _addCount = true;
    private int _hp = 1;
    private bool _isSeverelyDamaged = false;
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
        if (_isSeverelyDamaged == false &&_hp <= _maxHp - _severelyDamage) 
        {
            _onSeverelyDamagedEvent?.Invoke();
            _isSeverelyDamaged = true;
        }
        if (_hp <= 0)
        {
            _hp = 0;
            _onDeadEvent?.Invoke();
            if (_addCount == true)
            {
                StageManager.Instance.AddCount();
            }
        }
    }
    public void PlayEffect()
    {
        var effect = ObjectPoolManager.Instance.LimitUse(_deadEffect);
        effect.transform.position = transform.position;
        effect.transform.rotation = transform.rotation;
        effect.SetActive(true);
        StageShakeController.PlayShake(_deadShakeParam.Pos + transform.position,_deadShakeParam.Power,_deadShakeParam.Time);
    }
}
