using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 脚部用ステートマシーン
/// </summary>
public partial class LegStateContext
{
    #region PrivateField
    private ILegState _currentState = default;
    private Animator _legAnimator = default;
    private Vector3 _moveDir = default;
    private bool _jumpInput = false;
    private bool _stepInput = false;
    private bool _groundCheck = false;
    private MyGame.MoveController _moveController = default;

    //------------------------STATE------------------------------
    private GroundMoveState _groundState = new GroundMoveState();
    private JumpState _jumpState = new JumpState();
    private FallState _fallState = new FallState();
    private LandingState _landingState = new LandingState();
    private DownState _downState = new DownState();
    private GroundStepState _stepState = new GroundStepState();
    //-----------------------------------------------------------
    #endregion
    public LegAnimation AnimeName = default;
    public LegActionParam ActionParam = default;
    public Transform LegTrans = default;
    public Transform LegBaseTrans = default;
    public Transform BodyTrans = default;
    public bool IsFall = false;
    public LegStateContext(Animator animator, MyGame.MoveController moveController)
    {
        _legAnimator = animator;
        _moveController = moveController;
    }
    public void InitializeState()
    {
        _currentState = _groundState;
        _currentState.ExecuteEnter(this);
    }
    /// <summary>
    /// 移動方向、ジャンプ入力、接地判定を受け取りUpdateを行う
    /// </summary>
    /// <param name="moveDir"></param>
    /// <param name="jumpInput"></param>
    /// <param name="groundCheck"></param>
    public void ExecuteFixedUpdate(Vector3 moveDir, bool jumpInput, bool stepInput, bool groundCheck)
    {
        _moveDir = moveDir;
        _jumpInput = jumpInput;
        _groundCheck = groundCheck;
        _stepInput = stepInput;
        _currentState.ExecuteFixedUpdate(this);
    }
    public void ChangeToDown()
    {
        ChangeState(_downState);
    }
    private void ChangeState(ILegState legState)
    {
        _currentState = legState;
        _currentState.ExecuteEnter(this);
    }
    private void ChangeAnimation(string target)
    {
        _legAnimator.CrossFadeInFixedTime(target, ActionParam.AnimeChangeSpeed);
    }
}
