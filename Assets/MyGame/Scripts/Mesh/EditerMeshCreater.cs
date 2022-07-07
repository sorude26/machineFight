using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
/// <summary>
/// �A�^�b�`���ꂽ�I�u�W�F�N�g�̃��b�V���ۑ����s��
/// </summary>
public class EditerMeshCreater : MonoBehaviour
{
    private void Reset()
    {
        SeveMesh();
    }
    /// <summary>
    /// �I�u�W�F�N�g�̃��b�V����ۑ�����
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