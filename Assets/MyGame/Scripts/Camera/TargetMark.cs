using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetMark : MonoBehaviour
{
    [SerializeField]
    RectTransform _rect = default;
    public Transform Target = default;
    private bool _isActive = false;
    private void FixedUpdate()
    {
        if (Target == null && _isActive == true)
        {
            _isActive = false;
            _rect.gameObject.SetActive(_isActive);
            return;
        }       
        else if (Target == null)
        {
            return;
        }
        else if (_isActive == false)
        {
            _isActive = true;
            _rect.gameObject.SetActive(_isActive);
        }
        _rect.position = RectTransformUtility.WorldToScreenPoint(Camera.main, Target.position);
    }
}
