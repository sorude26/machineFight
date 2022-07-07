using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
/// <summary>
/// アタッチされたオブジェクトのメッシュ保存を行う
/// </summary>
public class EditerMeshCreater : MonoBehaviour
{
    private void Reset()
    {
        SeveMesh();
    }
    /// <summary>
    /// オブジェクトのメッシュを保存する
    /// </summary>
    private void SeveMesh()
    {
        if (TryGetComponent(out MeshFilter mesh))
        {
            MyGame.MeshControl.CreateMesh(gameObject.name, mesh);
            Debug.Log("Create");
        }
    }
}
#endif