using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsColorChanger : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer[] _armors = default;
    [SerializeField]
    private MeshRenderer[] _eyes = default;
    [SerializeField]
    private MeshRenderer[] _covers = default;
    [SerializeField]
    private bool _startColorSetMode = false;
    [SerializeField]
    private int _startColorID = default;
    private void Start()
    {
        if (_startColorSetMode == true)
        {
            ChangeColor(_startColorID);
        }
    }

    public void ChangeColor(int id)
    {
        if (id == 0)
        {
            return;
        }
        var colorData = PartsManager.Instance.AllModelData.GetColor(id);
        if (colorData == null) { return; }
        if (_armors != null)
        {
            foreach (var armor in _armors)
            {
                armor.material = colorData.ArmorMaterial;
            }
        }
        if (_covers != null)
        {
            foreach (var cover in _covers)
            {
                cover.material = colorData.CoverMaterial;
            }
        }
        if (_eyes != null)
        {
            foreach (var eye in _eyes)
            {
                eye.material = colorData.EyeMaterial;
            }
        }
    }
}
