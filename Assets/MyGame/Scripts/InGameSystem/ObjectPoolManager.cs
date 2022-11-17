using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    private const int DEFAULT_POOL_COUNT = 10;
    public const int DEFAULT_LIMIT_COUNT = 500;
    private static ObjectPoolManager instance;
    public static ObjectPoolManager Instance
    {
        get
        {
            if (instance == null)
            {
                var poolObj = new GameObject("PoolBase");
                instance = poolObj.AddComponent<ObjectPoolManager>();
                instance._objectDic = new Dictionary<string, List<GameObject>>();
                DontDestroyOnLoad(poolObj);
                SceneControl.OnSceneChange += instance.CleanUpObject;
            }
            return instance;
        }
    }
    private Dictionary<string, List<GameObject>> _objectDic = default;
    public void CreatePool(GameObject poolObject,int poolCount = DEFAULT_POOL_COUNT)
    {
        if (_objectDic.ContainsKey(poolObject.name))
        {
            return;
        }
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < poolCount; i++)
        {
            var obj = Instantiate(poolObject, this.transform);
            obj.SetActive(false);
            list.Add(obj);
        }
        _objectDic.Add(poolObject.name, list);
    }
    public GameObject Use(GameObject useObject)
    {
        if (!_objectDic.ContainsKey(useObject.name))
        {
            CreatePool(useObject);
        }
        foreach (var listObj in _objectDic[useObject.name])
        {
            if (listObj.activeInHierarchy)
            {
                continue;
            }
            return listObj;
        }
        var obj = Instantiate(useObject, this.transform);
        _objectDic[useObject.name].Add(obj);
        obj.SetActive(false);
        return obj;
    }
    public GameObject LimitUse(GameObject useObject)
    {
        if (!_objectDic.ContainsKey(useObject.name))
        {
            CreatePool(useObject);
        }
        foreach (var listObj in _objectDic[useObject.name])
        {
            if (listObj.activeInHierarchy)
            {
                continue;
            }
            return listObj;
        }
        return null;
    }

    public GameObject Use(GameObject useObject, Vector3 pos)
    {
        var obj = Use(useObject);
        obj.transform.position = pos;
        return obj;
    }
    public bool LimitUse(GameObject useObject,Vector3 pos,int limitCount = DEFAULT_LIMIT_COUNT)
    {
        if (!_objectDic.ContainsKey(useObject.name))
        {
            CreatePool(useObject);
        }
        if (_objectDic[useObject.name].Count >= limitCount && limitCount > DEFAULT_POOL_COUNT)
        {
            var obj = LimitUse(useObject);
            if (obj != null)
            {
                obj.transform.position = pos;
                return true;
            }
            return false;
        }
        Use(useObject,pos);
        return true;
    }
    public int GetCount(GameObject countObject)
    {
        if (!_objectDic.ContainsKey(countObject.name))
        {
            return 0;
        }
        int activeCount = 0;
        foreach (var listObj in _objectDic[countObject.name])
        {
            if (listObj.activeInHierarchy)
            {
               activeCount++;
            }
        }
        return activeCount;
    }
    public void CleanUpObject()
    {
        foreach (var objList in _objectDic.Values)
        {
            foreach (var obj in objList)
            {
                obj.SetActive(false);
            }
        }
    }
}
