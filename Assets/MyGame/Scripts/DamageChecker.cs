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
    private DamageRate[] _damageRateData = default;
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
    [SerializeField]
    private bool _isMarkTarget = false;
    [SerializeField]
    private bool _count = true;
    [SerializeField]
    private int _deadSEID = 8;
    [SerializeField]
    private float _seVolume = 1f;
    private int _hp = 1;
    private bool _isSeverelyDamaged = false;
    public int MaxHp { get => _maxHp; }
    public int CurrentHp { get => _hp; }
    public bool AddTarget { get => _addCount || _isMarkTarget; }
    public bool BossTarget { get => _addCount; }
    public int TotalDamage { get; private set; }
    public UnityEvent OnDamageEvent;
    public UnityEvent OnRecoveryEvent;
    private void Start()
    {
        StartSet();
    }
    public void StartSet()
    {
        _hp = _maxHp;
        if (StageManager.Instance != null)
        {
            if (_isMarkTarget == true)
            {
                StageManager.Instance.SetTargetCount();
            }
            if (_addCount == true)
            {
                StageManager.Instance.SetBossCount();
            }
        }
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
    public void AddlyDamage(int damage ,DamageType damageType)
    {
        if (_damageRateData == null)
        {
            AddlyDamage(damage);
            return;
        }
        for (int i = 0; i < _damageRateData.Length; i++)
        {
            if (damageType == _damageRateData[i].Type)
            {
                damage = (int)(damage * _damageRateData[i].ChangeRate);
            }
        }
        AddlyDamage(damage);
    }
    public void AddlyDamage(int damage)
    {
        if (_hp <= 0) { return; }
        _hp -= damage;
        TotalDamage += damage;
        if (_isSeverelyDamaged == false &&_hp <= _maxHp - _severelyDamage) 
        {
            _onSeverelyDamagedEvent?.Invoke();
            _isSeverelyDamaged = true;
        }
        if (_hp <= 0)
        {
            _hp = 0;
            _onDeadEvent?.Invoke();
            if (_addCount == true && StageManager.Instance != null)
            {
                StageManager.Instance.AddBossCount();
            }
            if (_isMarkTarget == true && StageManager.Instance != null)
            {
                StageManager.Instance.AddTargetCount();
            }
            if (_count == true && StageManager.Instance != null)
            {
                StageManager.Instance.AddCount();
            }
        }
        OnDamageEvent?.Invoke();
    }
    public void ChangeAnTarget()
    {
        _addCount = false;
        _isMarkTarget = false;
        _count = false;
        StageManager.Instance.SetTargetCount(-1);
    }
    public void ChangeTarget()
    {
        _isMarkTarget = true;
    }
    public void ChangeBoss()
    {
        _addCount = true;
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
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySE(_deadSEID,transform.position, _seVolume);
        }
    }
}
