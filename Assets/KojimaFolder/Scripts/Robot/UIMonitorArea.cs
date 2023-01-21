using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
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
        //モニターの作成を行う
        MakeMonitor();
    }

    private void MakeMonitor()
    {
        //メッシュ生成
        Mesh mesh = new Mesh();
        //頂点
        float left = _leftUp.localPosition.x;
        float right = _rightDown.localPosition.x;
        float up = _leftUp.localPosition.y;
        float down = _rightDown.localPosition.y;
        //左下、右下、左上、右上
        Vector3 leftDown = new Vector3(left, down, 0f);
        Vector3 rightDown = new Vector3(right, down, 0f);
        Vector3 leftUp = new Vector3(left, up, 0f);
        Vector3 rightUp = new Vector3(right, up, 0f);
        mesh.vertices = new Vector3[4] { leftDown, rightDown, leftUp, rightUp };
        //UV
        mesh.uv = new Vector2[] {
            new Vector2(_uLeft, _vDown),
            new Vector2(_uRight, _vDown),
            new Vector2(_uLeft, _vUp),
            new Vector2(_uRight, _vUp),
        };
        //トライアングル
        mesh.triangles = new int[] { 0, 2, 1, 1, 2, 3 };
        //メッシュ登録
        MeshFilter meshFilter = this.gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        //マテリアル登録
        MeshRenderer meshRenderer = this.gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = _material;
    }

    private void OnDrawGizmos()
    {
        var pos = _leftUp.localPosition;
        pos.z = 0;
        _leftUp.localPosition = pos;
        pos = _rightDown.localPosition;
        pos.z = 0;
        _rightDown.localPosition = pos;

        float sizeX = (_rightDown.localPosition.x - _leftUp.localPosition.x) * this.transform.lossyScale.x;
        float sizeY = (_rightDown.localPosition.y - _leftUp.localPosition.y) * this.transform.lossyScale.y;
        Vector3 rightUp = _leftUp.position + _leftUp.right * sizeX;
        Vector3 leftDown = _leftUp.position + _leftUp.up * sizeY;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_leftUp.position, rightUp);
        Gizmos.DrawLine(_rightDown.position, leftDown);
        Gizmos.DrawLine(_leftUp.position, leftDown);
        Gizmos.DrawLine(_rightDown.position, rightUp);
    }
}
