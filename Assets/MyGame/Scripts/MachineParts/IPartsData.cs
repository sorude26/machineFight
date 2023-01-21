using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPartsData
{
    public int PartsID { get; }
    public string PartName { get; }
    public int Model { get; }
}
