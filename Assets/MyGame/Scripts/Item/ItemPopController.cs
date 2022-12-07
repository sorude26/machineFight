using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPopController : PopController
{
    [SerializeField]
    private ItemBase[] _items = default;
    [SerializeField]
    private int _count = 1;
    public override void PopItem()
    {
        for (int i = 0; i < _count; i++)
        {
            int index = Random.Range(0, _items.Length);
            var item = ObjectPoolManager.Instance.Use(_items[index].gameObject);
            item.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            item.SetActive(true);
        }
    }
}
