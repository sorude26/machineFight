using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ˌ��֌W�̗\���v�Z���s��
/// </summary>
public class ShotPrediction
{
    private const int PREDICTION_FLAME_COUNT = 3;
    /// <summary>
    /// ���`�\���ɂ��\���ڕW���W��Ԃ�
    /// </summary>
    /// <param name="startPos">�J�n�_</param>
    /// <param name="targetPos">�ڕW�_</param>
    /// <param name="beforePos">�P�t���[���O�̍��W</param>
    /// <param name="shotSpeed">�e��</param>
    /// <returns></returns>
    public static Vector3 Linear(Vector3 startPos, Vector3 targetPos, Vector3 beforePos, float shotSpeed)
    {
        var shotFixedSpeed = shotSpeed * Time.fixedDeltaTime;
        //���݂̑��x
        Vector3 currentMoveSpeed = targetPos - beforePos;
        //�ڕW�Ƃ̋���
        Vector3 targetDis = targetPos - startPos;
        //�ړ������Q��
        float moveRange = Vector3.SqrMagnitude(currentMoveSpeed) - shotFixedSpeed * shotFixedSpeed;
        //�ڕW�̑��x�Ƌ����̍�
        float targetAngleSub = Vector3.Dot(targetDis, currentMoveSpeed);
        //�ڕW�����Q��
        float targetRange = Vector3.SqrMagnitude(targetDis);        
        if (moveRange == 0)
        {
            if (targetAngleSub == 0) { return targetPos; }
            return targetPos + currentMoveSpeed * (-targetRange / targetAngleSub / 2);
        }
        //�e�̖����t���[���̌v�Z
        float squareRoot = Mathf.Sqrt(Mathf.Abs(targetAngleSub * targetAngleSub - moveRange * targetRange));
        float shotHitFrame = 0;
        float shotHitFrameA = (-targetAngleSub - squareRoot) / moveRange;
        float shotHitFrameB = (-targetAngleSub + squareRoot) / moveRange;
        if (shotHitFrameA > 0 || shotHitFrameB > 0)
        {
            shotHitFrame = shotHitFrameA > shotHitFrameB ? shotHitFrameA : shotHitFrameB;
        }
        return targetPos + currentMoveSpeed * shotHitFrame;
    }
    /// <summary>
    /// �~�`�\���ɂ��\���ڕW���W��Ԃ�
    /// </summary>
    /// <param name="attackerPos"></param>
    /// <param name="targetPos"></param>
    /// <param name="beforePos"></param>
    /// <param name="twoBeforePos"></param>
    /// <param name="shotSpeed"></param>
    /// <returns></returns>
    public static Vector3 Circle(Vector3 attackerPos, Vector3 targetPos, Vector3 beforePos, Vector3 twoBeforePos, float shotSpeed)
    {
        //3�_����~�`�̒��S�_�����߂�
        Vector3 centerPos = Circumcenter(targetPos, beforePos, twoBeforePos);

        Vector3 axis = Vector3.Cross(beforePos - centerPos, targetPos - centerPos);
        float angle = Vector3.Angle(beforePos - centerPos, targetPos - centerPos);

        float predictionFlame = Vector3.Distance(targetPos, attackerPos) / (shotSpeed * Time.fixedDeltaTime);
        for (int i = 0; i < PREDICTION_FLAME_COUNT; i++)
        {
            predictionFlame = Vector3.Distance(RotateToPos(targetPos, centerPos, axis, angle * predictionFlame), attackerPos) / (shotSpeed * Time.fixedDeltaTime);
        }

        return RotateToPos(targetPos, centerPos, axis, angle * predictionFlame);
    }

    /// <summary>
    /// �O�p�`�̒��_�O�_�̈ʒu����O�S�̈ʒu��Ԃ�
    /// </summary>
    /// <param name="posA"></param>
    /// <param name="posB"></param>
    /// <param name="posC"></param>
    /// <returns></returns>
    private static Vector3 Circumcenter(Vector3 posA, Vector3 posB, Vector3 posC)
    {
        //�O�ӂ̒����̓����o��
        float edgeA = Vector3.SqrMagnitude(posB - posC);
        float edgeB = Vector3.SqrMagnitude(posC - posA);
        float edgeC = Vector3.SqrMagnitude(posA - posB);

        //�d�S���W�n�Ōv�Z����
        float a = edgeA * (-edgeA + edgeB + edgeC);
        float b = edgeB * (edgeA - edgeB + edgeC);
        float c = edgeC * (edgeA + edgeB - edgeC);

        float abc = a + b + c;
        if (abc == 0)//0�����h��
        {
            abc = 3;
        }
        return (posA * a + posB * b + posC * c) / abc;
    }
    /// <summary>
    /// �ڕW�ʒu���Z���^�[�ʒu�Ŏ��Ɗp�x�ŉ�]�������ʒu��Ԃ�
    /// </summary>
    /// <param name="target"></param>
    /// <param name="center"></param>
    /// <param name="axis"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    private static Vector3 RotateToPos(Vector3 target, Vector3 center, Vector3 axis, float angle)
    {
        return Quaternion.AngleAxis(angle, axis) * (target - center) + center;
    }
}
