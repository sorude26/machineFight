using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���o�[�̈ʒu�ɂ����value��0�`1�ɕω�
/// �����̃N���b�N�|�C���g(���o�[���グ��������Ƃ��ɒ�R��������|�C���g)��ݒ�ł���
/// </summary>
public class ThrottleLever : Switch
{
    const float LEVER_LEAP_SPEED = 0.2f;
    [SerializeField]
    Transform _minPosition;
    [SerializeField]
    Transform _maxPosition;
    [SerializeField]
    Transform _leverBody;

    protected override void Awake()
    {
        base.Awake();
        //���o�[�{�̂��ړ�������
        _leverBody.position = Vector3.Lerp(_minPosition.position, _maxPosition.position, _value);
    }

    public float GetValueByZone(int zone)
    {
        return _value;
    }

    protected override void SwitchUpdateOnLockIn()
    {
        base.SwitchUpdateOnLockIn();
        //�e���W�����
        Vector3 handPos = _lockinHand.transform.parent.position;
        Vector3 minPos = _minPosition.position;
        Vector3 maxPos = _maxPosition.position;
        Vector3 minToMax = maxPos - minPos;
        //���ςŒl�����o��
        float handSqrLength = Vector3.Dot(handPos - minPos, minToMax.normalized);
        if (handSqrLength < 0)
        {
            handSqrLength *= handSqrLength;
            handSqrLength = -handSqrLength;
        }
        else
        {
            handSqrLength *= handSqrLength;
        }
        float sqrLength = minToMax.sqrMagnitude;
        //�l���Z�o
        float value = handSqrLength / sqrLength;
        //Lerp�����Ēl�̕ω����Ȃ߂炩��
        value = Mathf.Lerp(_value, value, LEVER_LEAP_SPEED);
        value = Mathf.Max(0, value);
        value = Mathf.Min(1, value);
        //�l��K�p
        _value = value;

        //���o�[�{�̂��ړ�������
        _leverBody.position = minPos + minToMax * value;
    }

    protected override void LockIn(SwitchCtrlHand from)
    {
        from.LockIn(this);
        _lockinHand = from;
        //rotate
        _currentHandRotate = from.transform.rotation;
        //move(���ϕ�)
        _handMove = from.HoldPosition(_holdType) - _holdPosition.transform.position;
    }

    protected override void MoveRotateUpdateOnLockIn()
    {
        //rotate
        Quaternion myRotate = GetMyHoldRotation();
        //�X�C�b�`�ʒu����̑��ΓI�ȉ�]��؂�o���A�e����Weight��������
        Vector3 currentHandRotateEuler = (Quaternion.Inverse(myRotate) * _lockinHand.transform.parent.rotation).eulerAngles.NomalizeRotate();
        currentHandRotateEuler.x = SetWeight(currentHandRotateEuler.x, _rotateLockX);
        currentHandRotateEuler.y = SetWeight(currentHandRotateEuler.y, _rotateLockY);
        currentHandRotateEuler.z = SetWeight(currentHandRotateEuler.z, _rotateLockZ);
        static float SetWeight(float current, float weight)
        {
            if (weight == 0f) return current;
            //Weight
            current *= (1 - weight);
            return current;
        }

        //��]�K�p
        Quaternion newRotate = myRotate * Quaternion.Euler(currentHandRotateEuler);
        newRotate = Quaternion.Lerp(_currentHandRotate, newRotate, ROTATE_LERP_SPEED);
        _lockinHand.transform.rotation = newRotate;
        _currentHandRotate = newRotate;

        //move(���ϕ�)
        _handMove = Vector3.Lerp(_handMove, Vector3.zero, MOVE_LERP_SPEED);
        _lockinHand.transform.position = _handMove + _lockinHand.transform.position - _lockinHand.HoldPosition(_holdType) + _holdPosition.position;
    }
}
