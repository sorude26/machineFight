using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �w�肳�ꂽ�}�e���A����`�悷�郂�j�^�[���쐬����X�N���v�g
/// </summary>
public class UIMonitorArea : MonoBehaviour
{
    //leftUp��rightDown�͂��̃X�N���v�g���A�^�b�`����Ă���I�u�W�F�N�g�̒����̎q�I�u�W�F�N�g�Ƃ��Ĕz�u���邱��
    [SerializeField]
    Transform _leftUp;
    [SerializeField]
    Transform _rightDown;

    [SerializeField, Range(0f, 1f)]
    float _uLeft;
    [SerializeField, Range(0f, 1f)]
    float _uRight;
    [SerializeField, Range(0f, 1f)]
    float _vUp;
    [SerializeField, Range(0f, 1f)]
    float _vDown;

    [SerializeField]
    Material _material;

    private void Awake()
    {
        //���b�V������
        //UV�o�^
        //�}�e���A���o�^
        //�`��
    }

    private void OnDrawGizmos()
    {
        var pos = _leftUp.localPosition;
        pos.z = 0;
        _leftUp.localPosition = pos;
        pos = _rightDown.localPosition;
        pos.z = 0;
        _rightDown.localPosition = pos;

        float sizeX = (_rightDown.localPosition.x - _leftUp.localPosition.x);
        float sizeY = (_rightDown.localPosition.y - _leftUp.localPosition.y);
        Vector3 rightUp = _leftUp.position + _leftUp.right * sizeX;
        Vector3 leftDown = _leftUp.position + _leftUp.up * sizeY;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_leftUp.position, rightUp);
        Gizmos.DrawLine(_rightDown.position, leftDown);
        Gizmos.DrawLine(_leftUp.position, leftDown);
        Gizmos.DrawLine(_rightDown.position, rightUp);
    }
}
