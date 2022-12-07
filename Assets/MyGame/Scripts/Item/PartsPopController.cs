using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsPopController : PopController 
{ 
    [SerializeField]
    private PartsDropItem _item = default;
    [SerializeField]
    private MachineBuilder _machineBuilder = null;
    public override void PopItem()
    {
        var partsType = (PartsType)Random.Range(0, PartsBuildParam.PARTS_TYPE_NUM);
        var partsId = _machineBuilder.BuildData[partsType];
        var item = ObjectPoolManager.Instance.Use(_item.gameObject);
        item.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        item.GetComponent<PartsDropItem>().SetData(partsType, partsId);
        item.SetActive(true);
    }
}
