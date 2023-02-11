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

        //Materialごとにメッシュを登録
        var materialDic = new Dictionary<string, Material>();
        var meshFilterDic = new Dictionary<string, List<MeshFilter>>();
        for (int i = 0; i < meshFilters.Length; i++)
        {
            string materialName = meshRenderers[i].material.name;
            //登録されていない場合はListを生成
            if (!meshFilterDic.ContainsKey(materialName))
            {
                var filters = new List<MeshFilter>();
                meshFilterDic.Add(materialName, filters);
                materialDic.Add(materialName, meshRenderers[i].material);
            }
            meshFilterDic[materialName].Add(meshFilters[i]);
        }
        //Materialごとに結合処理を行う
        foreach (KeyValuePair<string, List<MeshFilter>> filterList in meshFilterDic)
        {
            //結合メッシュの土台を生成
            var obj = new GameObject($"Comb:{filterList.Key}");
            obj.transform.SetParent(target);
            obj.transform.localPosition = Vector3.zero;
            //描画用コンポーネントを追加
            var combFilter = obj.AddComponent<MeshFilter>();
            var combRenderer = obj.AddComponent<MeshRenderer>();
            //結合するオブジェクトのメッシュと座標を保存
            CombineInstance[] combines = new CombineInstance[filterList.Value.Count];
            for (int i = 0; i < filterList.Value.Count; i++)
            {
                combines[i].mesh = filterList.Value[i].sharedMesh;
                combines[i].transform = filterList.Value[i].transform.localToWorldMatrix;
                filterList.Value[i].gameObject.SetActive(false);
            }
            //結合処理
            combFilter.mesh = new Mesh();
            combFilter.mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;//頂点数がオーバーフローするため、変更
            combFilter.mesh.CombineMeshes(combines);
            //Material反映
            combRenderer.material = materialDic[filterList.Key];

            MyGame.MeshControl.CreateMesh(obj.name, combFilter);
            Debug.Log("Create");
        }
    }
}
#endif