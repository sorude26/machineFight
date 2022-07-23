using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetObjectPool<TObject> : MonoBehaviour where TObject : MonoBehaviour
{
    protected const int DEFAULT_POOL_COUNT = 15;
    protected static TargetObjectPool<TObject> instance = default;
    protected static readonly Vector3 INVISIBLE_POS = new Vector3(0, -1000, 0);
    protected Dictionary<string, int> _keysDic = default;
    protected Dictionary<int, List<TObject>> _objectDic = default;
    public static TargetObjectPool<TObject> Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = new GameObject($"{typeof(TObject)}Pool");
                instance = obj.AddComponent<TargetObjectPool<TObject>>();
                instance._keysDic = new Dictionary<string, int>();
                instance._objectDic = new Dictionary<int, List<TObject>>();
            }
            return instance;
        }
    }
    public static void CreatePool(TObject poolObject, int createCount = DEFAULT_POOL_COUNT)
    {
        if (Instance._keysDic.ContainsKey(poolObject.name))
        {
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
        instance._keysDic.Add(poolObject.name, instance._objectDic.Count);
        instance._objectDic.Add(instance._keysDic[poolObject.name], pool);
    }
    /// <summary>
    /// プールしたオブジェクトを返す
    /// </summary>
    /// <param name="useObject"></param>
    /// <returns></returns>
    public static TObject GetObject(TObject useObject)
    {
        if (!Instance._keysDic.ContainsKey(useObject.name)) //プールが生成されていないオブジェクトの場合、プールを生成する
        {
            CreatePool(useObject);
        }
        foreach (var poolObject in instance._objectDic[instance._keysDic[useObject.name]])
        {
            if (poolObject is null || poolObject.gameObject.activeInHierarchy)
            {
                continue;
            }
            poolObject.transform.position = INVISIBLE_POS;
            poolObject.gameObject.SetActive(true);
            return poolObject;
        }
        var obj = Instantiate(useObject, instance.transform);
        obj.transform.position = INVISIBLE_POS;
        instance._objectDic[instance._keysDic[useObject.name]].Add(obj);
        return obj;
    }
}
