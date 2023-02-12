using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
public class EditerMeshCombination : MonoBehaviour
{
    private void Reset()
    {
        SeveMesh();
    }
    private void SeveMesh()
    {
        Combine(transform);       
    }
    public void Combine(Transform target)
    {
        var meshFilters = target.GetComponentsInChildren<MeshFilter>();
        var meshRenderers = target.GetComponentsInChildren<MeshRenderer>();

        //Material���ƂɃ��b�V����o�^
        var materialDic = new Dictionary<string, Material>();
        var meshFilterDic = new Dictionary<string, List<MeshFilter>>();
        for (int i = 0; i < meshFilters.Length; i++)
        {
            string materialName = meshRenderers[i].material.name;
            //�o�^����Ă��Ȃ��ꍇ��List�𐶐�
            if (!meshFilterDic.ContainsKey(materialName))
            {
                var filters = new List<MeshFilter>();
                meshFilterDic.Add(materialName, filters);
                materialDic.Add(materialName, meshRenderers[i].material);
            }
            meshFilterDic[materialName].Add(meshFilters[i]);
        }
        //Material���ƂɌ����������s��
        foreach (KeyValuePair<string, List<MeshFilter>> filterList in meshFilterDic)
        {
            //�������b�V���̓y��𐶐�
            var obj = new GameObject($"Comb:{filterList.Key}");
            obj.transform.SetParent(target);
            obj.transform.localPosition = Vector3.zero;
            //�`��p�R���|�[�l���g��ǉ�
            var combFilter = obj.AddComponent<MeshFilter>();
            var combRenderer = obj.AddComponent<MeshRenderer>();
            //��������I�u�W�F�N�g�̃��b�V���ƍ��W��ۑ�
            CombineInstance[] combines = new CombineInstance[filterList.Value.Count];
            for (int i = 0; i < filterList.Value.Count; i++)
            {
                combines[i].mesh = filterList.Value[i].sharedMesh;
                combines[i].transform = filterList.Value[i].transform.localToWorldMatrix;
                filterList.Value[i].gameObject.SetActive(false);
            }
            //��������
            combFilter.mesh = new Mesh();
            combFilter.mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;//���_�����I�[�o�[�t���[���邽�߁A�ύX
            combFilter.mesh.CombineMeshes(combines);
            //Material���f
            combRenderer.material = materialDic[filterList.Key];

            MyGame.MeshControl.CreateMesh(obj.name, combFilter);
            Debug.Log("Create");
        }
    }
}
#endif