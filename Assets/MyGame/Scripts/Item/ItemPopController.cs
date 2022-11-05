using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPopController : MonoBehaviour
{
    [SerializeField]
    private ItemBase[] _items = default;
    public void PopItem()
    {
        int index = Random.Range(0, _items.Length);
        var item = ObjectPoolManager.Instance.Use(_items[index].gameObject);
        item.transform.position = transform.position;
        item.transform.rotation = Quaternion.identity;
        item.gameObject.SetActive(true);
    }
}
