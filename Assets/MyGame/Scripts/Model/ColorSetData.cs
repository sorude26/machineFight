using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ColorSet/Data")]
public class ColorSetData : ScriptableObject,IPartsData
{
    public int ID = default;
    public string ColorSetName = default;
    public Material ArmorMaterial = default;
    public Material EyeMaterial = default;
    public Material CoverMaterial = default;

    public int PartsID => ID;
    public string PartName => ColorSetName;
    public int Model => -1;
}
