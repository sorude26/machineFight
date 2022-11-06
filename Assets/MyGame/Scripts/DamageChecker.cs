using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.VersionControl.Message;

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
    private GameObject _deadEffect = default;
    [SerializeField]
    private ShakeParam _deadShakeParam = default;
    [SerializeField]
    private bool _addCount = true;
    private int _hp = 1;
    private bool _isSeverelyDamaged = false;
    public int MaxHp { get => _maxHp; }
    public int CurrentHp { get => _hp; }
    public UnityEvent OnDamageEvent;
    public UnityEvent OnRecoveryEvent;
    private void Start()
    {
        _hp = _maxHp;
    }
    public void SetHp(int hp,float severely = 0.8f)
    {
        _maxHp = hp;
        _hp = hp;
        _severelyDamage = (int)(_maxHp * severely);
    }
    public void RecoveryHp(int point)
    {
        if (_hp <= 0) { return; }
        _hp += point;
        if (_hp > _maxHp)
        {
            _hp = _maxHp;
        }
        OnRecoveryEvent?.Invoke();
    }
    public void AddlyDamage(int damage)
    {
        if (_hp <= 0) { return; }
        _hp -= damage;
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
        OnDamageEvent?.Invoke();
    }
    public void PlayEffect()
    {
        var effect = ObjectPoolManager.Instance.LimitUse(_deadEffect);
        if (effect != null)
        {
            effect.transform.position = transform.position;
            effect.transform.rotation = transform.rotation;
            effect.SetActive(true);
        }
        StageShakeController.PlayShake(_deadShakeParam.Pos + transform.position,_deadShakeParam.Power,_deadShakeParam.Time);
    }
}
