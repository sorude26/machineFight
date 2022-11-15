using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetObjectPool<TObject,TPool> : MonoBehaviour 
    where TObject : MonoBehaviour where TPool :TargetObjectPool<TObject, TPool>
{
    /// <summary> 指定無しの際にプールする数 </summary>
    protected const int DEFAULT_POOL_COUNT = 15;
    protected static TPool instance = default;
    protected static readonly Vector3 INVISIBLE_POS = new Vector3(0, -1000, 0);
    protected Dictionary<string, List<TObject>> _poolDic = new Dictionary<string, List<TObject>>();
    public static TPool Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = new GameObject($"{typeof(TObject)}Pool");
                instance = obj.AddComponent<TPool>();
                DontDestroyOnLoad(obj);
                SceneControl.OnSceneChange += instance.CleanUpObject;
            }
            return instance;
        }
    }
    /// <summary>
    /// 指定数までオブジェクトをプールに追加する
    /// </summary>
    /// <param name="poolObject"></param>
    /// <param name="key"></param>
    /// <param name="count"></param>
    private void AddObject(TObject poolObject,string key,int count)
    {
        for (int i = this._poolDic[key].Count; i < count; i++)
        {
            var obj = Instantiate(poolObject, this.transform);
            obj.transform.position = INVISIBLE_POS;
            obj.gameObject.SetActive(false);
            this._poolDic[key].Add(obj);
        }
    }
    /// <summary>
    /// プールを作成する
    /// </summary>
    /// <param name="poolObject"></param>
    /// <param name="createCount"></param>
    public static void CreatePool(TObject poolObject, int createCount = DEFAULT_POOL_COUNT)
    {
        if (Instance._poolDic.ContainsKey(poolObject.name))//既にプールが存在する場合
        {            
            instance.AddObject(poolObject, poolObject.name, createCount);
            return;
        }
        var pool = new List<TObject>();
        for (int i = 0; i < createCount; i++)
        {
            var obj = Instantiate(poolObject, instance.transform);
            obj.transform.position = INVISIBLE_POS;
            obj.gameObject.SetActive(false);
            pool.Add(obj);
        }
        instance._poolDic.Add(poolObject.name, pool);
    }
    /// <summary>
    /// プールしたオブジェクトを返す
    /// </summary>
    /// <param name="useObject"></param>
    /// <returns></returns>
    public static TObject GetObject(TObject useObject)
    {
        if (!Instance._poolDic.ContainsKey(useObject.name)) //プールが生成されていないオブジェクトの場合、プールを生成する
        {
            CreatePool(useObject);
        }
        foreach (var poolObject in instance._poolDic[useObject.name])
        {
            if (poolObject is null || poolObject.gameObject.activeInHierarchy)
            {
                continue;
            }
            poolObject.transform.position = INVISIBLE_POS;
            return poolObject;
        }
        var obj = Instantiate(useObject, instance.transform);
        obj.transform.position = INVISIBLE_POS;
        instance._poolDic[useObject.name].Add(obj);
        obj.gameObject.SetActive(false);
        return obj;
    }
    public void CleanUpObject()
    {
        foreach (var objList in _poolDic.Values)
        {
            foreach (var obj in objList)
            {
                obj.gameObject.SetActive(false);
            }
        }
    }
}
