using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ���o�[�̈ʒu�ɂ����value��0�`1�ɕω�
/// �����̃N���b�N�|�C���g(���o�[���グ��������Ƃ��ɒ�R��������|�C���g)���O������ݒ�ł���
/// OnExitZone��OnEnterZone�R�[���o�b�N���O������o�^�\
/// </summary>
public class ThrottleLever : Switch
{
    const float LEVER_LEAP_SPEED = 0.2f;
    const float CLICK_FORCE = 0.02f;
    [SerializeField]
    Transform _minPosition;
    [SerializeField]
    Transform _maxPosition;
    [SerializeField]
    Transform _leverBody;

    //����
    [SerializeField]
    List<float> _clickPoints = new List<float>();
    float _valueByZone = 0;
    int _currentZone = 0;

    protected override void Awake()
    {
        base.Awake();
        //���o�[�{�̂��ړ�������
        _leverBody.position = Vector3.Lerp(_minPosition.position, _maxPosition.position, _value);
        //�]�[���̒l��������
        ChangeValue(_value);
    }

    public event Action<int> OnExitZone;
    public event Action<int> OnEnterZone;
    public int ZoneCount => _clickPoints.Count + 1;
    public int CurrentZone => _currentZone;
    public float ValueByZone => _valueByZone;
    /// <summary>
    /// ���o�[���グ��������Ƃ��ɒ�R��������|�C���g��ݒ肷��
    /// </summary>
    /// <param name="points">0�`1�͈͓̔��Őݒ肳���|�C���g�̈ʒu</param>
    public void SetClickPointsAsNew(float[] points)
    {
        //�����ɕ��ёւ��ĕۊ�
        _clickPoints = points.Where(p => p < 1f && p > 0f).OrderBy(p => p).ToList();
        //�]�[���v�Z
        ChangeValue(_value);
    }


    private void SetValueByZone(float value)
    {
        float r = value;
        float min = 0;
        if (_currentZone != 0)
        {
            min = _clickPoints[_currentZone - 1];
        }
        float max = 1;
        if (_currentZone != _clickPoints.Count)
        {
            max = _clickPoints[_currentZone];
        }

        r -= min;
        r /= (max - min);
        _valueByZone = r;
    }



    private int GetZone(float value)
    {
        int zone =  0;
        foreach (var point in _clickPoints)
        {
            if (point > value)
            {
                return zone;
            }
            ++zone;
        }
        return zone;
    }

    private void ChangeValue(float value)
    {
        if (_clickPoints.Count == 0)
        {
            //�敪������Ă���]�[�������݂��Ȃ��Ȃ�΂��̂܂ܒl��K�p���ďI��
            _value = value;
            _valueByZone = value;
            return;
        }


        int current = _currentZone;
        int newZone = GetZone(value);
        if (current == newZone)
        {
            //�]�[����ς���K�v���Ȃ����߂��̂܂ܒl��K�p���ďI������
            _value = value;
            //ValueByZone�K�p
            SetValueByZone(value);
            return;
        }

        if (current < newZone)
        {
            //���o�[����ɍs���Ă���Ƃ�
            float max = _clickPoints[current];
            //�~������֍s�����ꍇ�̓]�[���ύX
            if (max < value - CLICK_FORCE)
            {
                //�]�[���ύX�K�p
                ChangeZone(value, newZone);
                //ValueByZone�K�p
                SetValueByZone(value);
                return;
            }
            _value = max;
            _valueByZone = 1;
            return;
        }
        else
        {
            //���o�[�����ɍs���Ă���Ƃ�
            float min = _clickPoints[current - 1];
            //�~����艺�֍s�����ꍇ�̓]�[���ύX
            if (min > value + CLICK_FORCE)
            {
                //�]�[���ύX�K�p
                ChangeZone(value, newZone);
                //ValueByZone�K�p
                SetValueByZone(value);
                return;
            }
            _value = min;
            _valueByZone = 0;
            return;
        }
    }
    private void ChangeZone(float value, int newZone)
    {
        _value = value;
        //�]�[�����ς���Ă��Ȃ��ꍇ�͂������牺�̏����͕s�v
        if (_currentZone == newZone) return;

        //�]�[���ύX���̃R�[���o�b�N���s
        OnExitZone?.Invoke(_currentZone);
        OnEnterZone?.Invoke(newZone);
        //�]�[���ύX
        _currentZone = newZone;
        //�G�t�F�N�g����
        Vibrate();
        _audioSource?.PlayOneShot(_audioClip);
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
        ChangeValue(value);

        //���o�[�{�̂��ړ�������
        _leverBody.position = minPos + minToMax * _value;
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
