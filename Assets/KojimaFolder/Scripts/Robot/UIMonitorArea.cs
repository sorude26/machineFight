using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 指定されたマテリアルを描画するモニターを作成するスクリプト
/// </summary>
public class UIMonitorArea : MonoBehaviour
{
    //leftUpとrightDownはこのスクリプトがアタッチされているオブジェクトの直下の子オブジェクトとして配置すること
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
        //メッシュ生成
        //UV登録
        //マテリアル登録
        //描画
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
