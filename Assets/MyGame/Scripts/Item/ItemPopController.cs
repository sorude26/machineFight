using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPopController : PopController
{
    [SerializeField]
    private ItemBase[] _items = default;
    [SerializeField]
    private int _count = 1;
    [SerializeField]
    private ItemBase _defItem = default;
    [SerializeField]
    private int _defItemCount = 8;
    public override void PopItem()
    {
        if (_defItem != null)
        {
            for (int i = 0; i < _defItemCount; i++)
            {
                var defItem = ObjectPoolManager.Instance.Use(_defItem.gameObject);
                defItem.transform.SetPositionAndRotation(Diffusivity(transform.position), Quaternion.identity);
                defItem.SetActive(true);
            }
        }
        for (int i = 0; i < _count; i++)
        {
            int index = Random.Range(0, _items.Length);
            var item = ObjectPoolManager.Instance.Use(_items[index].gameObject);
            item.transform.SetPositionAndRotation(Diffusivity(transform.position), Quaternion.identity);
            item.SetActive(true);
        }
    }
    
}
