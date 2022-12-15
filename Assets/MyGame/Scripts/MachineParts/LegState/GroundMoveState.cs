using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public partial class LegStateContext
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
    private class GroundMoveState : ILegState
    {
        
        private readonly float decelerate = 0.8f;
        private LegAngle _currentAngle = default;
        private float _turnSpeed = default;
        private float _identiySpeed = 5f;
        private float _walkSpeed = default;
        private float _range = 0.95f;
        private float _rRange = 0.999f;
        private float _turnRange = 0.01f;

        /// <summary>
        /// 歩行移動を行う
        /// </summary>
        /// <param name="context"></param>
        private void LegMove(LegStateContext context)
        {
            context._moveController.MoveDecelerate(decelerate);//速度減衰を行う
            var range = Vector3.Dot(context.LegBaseTrans.forward, context.BodyTrans.forward);
            var rootR = context.BodyTrans.localRotation;
            if (Mathf.Abs(context._moveDir.z) <= _range && range <= _turnRange && range >= -_turnRange)
            {
                _turnRange = _rRange;
                context.LegBaseTrans.localRotation = Quaternion.Lerp(context.LegBaseTrans.localRotation, rootR, _turnSpeed * Time.fixedDeltaTime);
                context.LegTrans.localRotation = Quaternion.Lerp(context.LegTrans.localRotation, Quaternion.identity, _identiySpeed * _turnSpeed * Time.fixedDeltaTime);
                RotationLeg(context, context.BodyTrans.localRotation.y);
                return;
            }
            _turnRange = _range;
            if (context._moveDir == Vector3.zero)//入力がない場合は待機アニメーションに変更
            {
                if (_currentAngle != LegAngle.Stop)
                {
                    _currentAngle = LegAngle.Stop;
                    context.ChangeAnimation(context.AnimeName.Idle);
                }
                return;
            }
            context.LegBaseTrans.localRotation = Quaternion.Lerp(context.LegBaseTrans.localRotation, rootR, _turnSpeed * Time.fixedDeltaTime);
            //入力回転目標
            Quaternion lockQ;
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
            else
            {
                lockQ = Quaternion.Euler(0, (90f - Mathf.Abs(context._moveDir.z) * 45f) * context._moveDir.x, 0);
            }

            //脚部を旋回する
            context.LegTrans.localRotation = Quaternion.Lerp(context.LegTrans.localRotation, lockQ, _turnSpeed * Time.fixedDeltaTime);
            //後退時以外で旋回角度が一定以上の場合、旋回アニメーションに変更
            float local = Mathf.Abs(context.LegTrans.localRotation.y);
            float target = Mathf.Abs(lockQ.y);
            if (_currentAngle != LegAngle.Back && Mathf.Max(local, target) - Mathf.Min(local, target) > context.ActionParam.DotSub)
            {
                RotationLeg(context, lockQ.y);
                return;
            }
            else if (_currentAngle != LegAngle.Front && context._moveDir.z >= 0)//一定以内であれば正面移動アニメーションに変更
            {
                _currentAngle = LegAngle.Front;
                context.ChangeAnimation(context.AnimeName.Walk);
            }
            //移動処理
            context._moveController.GVelocityMove(moveDir * _walkSpeed);
        }

        /// <summary>
        /// 条件を満たす左右方向の旋回アニメーションに変更する
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dirDifference"></param>
        private void RotationLeg(LegStateContext context, float dirDifference)
        {
            if (_currentAngle != LegAngle.Left && context.LegTrans.localRotation.y - dirDifference > 0)//左旋回
            {
                _currentAngle = LegAngle.Left;
                context.ChangeAnimation(context.AnimeName.TurnLeft);
            }
            else if (_currentAngle != LegAngle.Right && context.LegTrans.localRotation.y - dirDifference < 0)//右旋回
            {
                _currentAngle = LegAngle.Right;
                context.ChangeAnimation(context.AnimeName.TurnRight);
            }
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
            if (context._stepInput == true)
            {
                context.ChangeState(context._stepState);
            }
            LegMove(context);
        }
    }
}