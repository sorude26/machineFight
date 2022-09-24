using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class LegStateContext
{
    private ILegState _currentState = default;
    private Animator _legAnimator = default;

    private Vector3 _moveDir = default;
    private bool _jumpInput = false;
    private bool _groundCheck = false;
    private MyGame.MoveController _moveController = default;

    private GroundMoveState _groundState = new GroundMoveState();
    private JumpState _jumpState = new JumpState();

    public LegAnimation _animeName = default;
    public LegActionParam _actionParam = default;
    public Transform LegTrans = default;
    public Transform BodyTrans = default;
    public LegStateContext(Animator animator, MyGame.MoveController moveController)
    {
        _legAnimator = animator;
        _moveController = moveController;
        _currentState = _groundState;
        _currentState.ExecuteEnter(this);
    }

    public void ExecuteUpdate(Vector3 moveDir, bool jumpInput, bool groundCheck)
    {
        _moveDir = moveDir;
        _jumpInput = jumpInput;
        _groundCheck = groundCheck;
        _currentState.ExecuteUpdate(this);
    }
    public void ChangeState(ILegState legState)
    {
        _currentState.ExecuteExit(this);
        _currentState = legState;
        _currentState.ExecuteEnter(this);
    }
    private void ChangeAnimetion(string target)
    {
        _legAnimator.CrossFadeInFixedTime(target, _actionParam.AnimeChangeSpeed);
    }
}
