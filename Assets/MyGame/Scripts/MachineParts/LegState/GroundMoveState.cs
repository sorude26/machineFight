using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public partial class LegStateContext
{
    private class GroundMoveState : ILegState
    {
        private enum LegAngle
        {
            Idle,
            Stop,
            Front,
            Back,
            Left,
            Right,
        }
        private readonly float decelerate = 0.8f;
        private LegAngle _currentAngle = default;
        private float _turnSpeed = default;
        private float _walkSpeed = default;
        /// <summary>
        /// 歩行移動を行う
        /// </summary>
        /// <param name="context"></param>
        private void LegMove(LegStateContext context)
        {
            context._moveController.MoveDecelerate(decelerate);//速度減衰を行う
            if (context._moveDir == Vector3.zero)//入力がない場合は待機アニメーションに変更
            {
                if (_currentAngle != LegAngle.Stop)
                {
                    _currentAngle = LegAngle.Stop;
                    context.ChangeAnimation(context.AnimeName.Idle);
                }
                return;
            }
            //入力回転目標
            Quaternion lockQ = Quaternion.Euler(0, (90f - Mathf.Abs(context._moveDir.z) * 45f) * context._moveDir.x, 0);
            //進行方向
            Vector3 moveDir = context.LegTrans.forward;
            if (context._moveDir.z < 0)//後退入力であれば後退アニメーションに変更
            {
                lockQ = Quaternion.Euler(0, (90f - Mathf.Abs(context._moveDir.z) * 45f) * -context._moveDir.x, 0);
                if (_currentAngle != LegAngle.Back)
                {
                    _currentAngle = LegAngle.Back;
                    context.ChangeAnimation(context.AnimeName.Back);
                }
                moveDir = -moveDir;
            }
            //脚部を旋回する
            context.LegTrans.localRotation = Quaternion.Lerp(context.LegTrans.localRotation, lockQ, _turnSpeed * Time.fixedDeltaTime);
            //後退時以外で旋回角度が一定以上の場合、旋回アニメーションに変更
            float local = Mathf.Abs(context.LegTrans.localRotation.y);
            float target = Mathf.Abs(lockQ.y);
            if (_currentAngle != LegAngle.Back && Mathf.Max(local, target) - Mathf.Min(local, target) > context.ActionParam.DotSub)
            {
                if (_currentAngle != LegAngle.Left && context.LegTrans.localRotation.y - lockQ.y > 0)//左旋回
                {
                    _currentAngle = LegAngle.Left;
                    context.ChangeAnimation(context.AnimeName.TurnLeft);
                }
                else if (_currentAngle != LegAngle.Right && context.LegTrans.localRotation.y - lockQ.y < 0)//右旋回
                {
                    _currentAngle = LegAngle.Right;
                    context.ChangeAnimation(context.AnimeName.TurnRight);
                }
                return;
            }
            else if (_currentAngle != LegAngle.Front && context._moveDir.z >= 0)//一定以内であれば正面移動アニメーションに変更
            {
                _currentAngle = LegAngle.Front;
                context.ChangeAnimation(context.AnimeName.Walk);
            }
            //移動処理
            context._moveController.VelocityMove(moveDir * _walkSpeed);
        }
        public void ExecuteEnter(LegStateContext context)
        {
            _currentAngle = LegAngle.Idle;
            _turnSpeed = context.ActionParam.WalkTurnSpeed;
            _walkSpeed = context.ActionParam.WalkSpeed;
        }

        public void ExecuteFixedUpdate(LegStateContext context)
        {            
            if (context._groundCheck == false)
            {
                context.ChangeState(context._fallState);
                return;
            }
            if (context._jumpInput == true)
            {
                context.ChangeState(context._jumpState);
                return;
            }
            LegMove(context);
        }
    }
}