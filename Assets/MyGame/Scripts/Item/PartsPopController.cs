using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsPopController : PopController 
{ 
    [SerializeField]
    private PartsDropItem _item = default;
    [SerializeField]
    private ItemBase _defItem = default;
    [SerializeField]
    private int _defItemCount = default;
    [SerializeField]
    private MachineBuilder _machineBuilder = null;
    public override void PopItem()
    {
        for (int i = 0; i < _defItemCount; i++)
        {
            var defItem = ObjectPoolManager.Instance.Use(_defItem.gameObject);
            defItem.transform.SetPositionAndRotation(Diffusivity(transform.position), Quaternion.identity);
            defItem.SetActive(true);
        }
        var partsType = (PartsType)Random.Range(0, PartsBuildParam.PARTS_TYPE_NUM);
        var partsId = _machineBuilder.BuildData[partsType];
        var item = ObjectPoolManager.Instance.Use(_item.gameObject);
        item.transform.SetPositionAndRotation(transform.position + Vector3.up, Quaternion.identity);
        item.GetComponent<PartsDropItem>().SetData(partsType, partsId);
        item.SetActive(true);
    }
}
