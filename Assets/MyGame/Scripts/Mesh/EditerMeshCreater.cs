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
    private void Start()
    {
        if (TryGetComponent(out MeshFilter mesh))
        {
            MeshControl.CreateMesh(gameObject.name, mesh);
            Debug.Log("Create");
        }
    }
}
#endif