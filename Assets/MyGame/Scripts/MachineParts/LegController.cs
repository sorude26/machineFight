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
    public Transform LegBase = default;
    public Transform LockTrans = default;
    public Transform BaseTrans = default;
    private MoveController _moveController = default;
    private LegStateContext _stateContext = default;
    private bool _isJump = false;
    private bool _isStep = false;
    public bool IsFall { get; private set; }
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
        _stateContext.ExecuteFixedUpdate(dir, _isJump,_isStep, _groundChecker.IsWalled());
        _isJump = false;
        _isStep = false;
        IsFall = _stateContext.IsFall;
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
    public void Jump()
    {
        _isJump = true;
    }
    public void Step()
    {
        _isStep = true;
    }
    public void PowerDown()
    {
        _stateContext.ChangeToDown();
    }
}
