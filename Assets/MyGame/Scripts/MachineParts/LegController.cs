using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame;
public class LegController : MonoBehaviour, IPartsModel
{
    [SerializeField]
    private int _id = default;
    [SerializeField]
    private Animator _legAnimator = default;
    [SerializeField]
    private WallChecker _groundChecker = default;
    [SerializeField]
    private LegAnimation _animeName = default;
    [SerializeField]
    private LegActionParam _actionParam = default;
    [SerializeField]
    private Transform _bodyJoint = default;
    [SerializeField]
    private Transform _leftIk = default;
    [SerializeField]
    private Transform _rightIk = default;
    [SerializeField]
    private BoosterController _legBooster = default;
    [SerializeField]
    private PartsColorChanger _partsColorChanger = default;
    [SerializeField]
    private ParticleSystem _floatEffect = default;
    [SerializeField]
    private float _effectRange = default;
    [SerializeField]
    private float _legAttackInterval = 1f;
    [SerializeField]
    private LayerMask _effectLayer = default;
    public Transform LegBase = default;
    public Transform LockTrans = default;
    public Transform BaseTrans = default;
    private MoveController _moveController = default;
    private LegStateContext _stateContext = default;
    private bool _isJump = false;
    private bool _isStep = false;
    private float _attackIntervalTimer = 0;
    public bool IsFall { get => _stateContext.IsFall; }
    public bool IsFloat { get => _stateContext.IsFloat; }
    public int ID { get => _id; }
    public Transform BodyJoint { get => _bodyJoint; }
    public BoosterController LegBoost { get => _legBooster; }
    public void Initialize(MoveController moveController)
    {
        _moveController = moveController;
        _stateContext = new LegStateContext(_legAnimator, _moveController);
        _stateContext.ActionParam = _actionParam;
        _stateContext.AnimeName = _animeName;
        _stateContext.LegTrans = LegBase;
        _stateContext.BodyTrans = LockTrans;
        _stateContext.LegBaseTrans = BaseTrans;
        _stateContext.InitializeState();
    }
    public void ExecuteFixedUpdate(Vector3 dir)
    {
        _stateContext.ExecuteFixedUpdate(dir, _isJump, _isStep, _groundChecker.IsWalled());
        if (_stateContext.IsFloat == true || _stateContext.IsFall == true)
        {
            if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, _effectRange, _effectLayer))
            {
                _floatEffect.transform.position = hit.point;
                _floatEffect.Play();
            }
        }
        if (_attackIntervalTimer > 0)
        {
            _attackIntervalTimer -= Time.fixedDeltaTime;
        }
        _isJump = false;
        _isStep = false;
    }
    public void SetLegParam(LegActionParam param)
    {
        _actionParam = param;
    }
    public void SetIkTargetBase(Transform trans)
    {
        _leftIk.SetParent(trans);
        _rightIk.SetParent(trans);
    }
    public void ChangeColor(int id)
    {
        if (_partsColorChanger != null)
        {
            _partsColorChanger.ChangeColor(id);
        }
    }
    public void Jump()
    {
        _isJump = true;
    }
    public void Step()
    {
        _isStep = true;
    }
    public void Attack()
    {
        if (_attackIntervalTimer > 0)
        {
            return;
        }
        _attackIntervalTimer = _legAttackInterval;
        _stateContext.Attack();
    }
    public void PowerDown()
    {
        _stateContext.ChangeToDown();
    }
    public void StartUpLeg()
    {
        LegBase.localRotation = Quaternion.identity;
        LockTrans.localRotation = Quaternion.identity;
        BaseTrans.localRotation = Quaternion.identity;
        _stateContext.ChangeIdle();
    }
    public void ChangeFloat()
    {
        _stateContext.ChangeFloatMode();
    }
}
