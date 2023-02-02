using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILeftView : MonoBehaviour
{
    [SerializeField]
    Transform _transform;
    [SerializeField]
    Image _image;
    [SerializeField]
    Text _text;

    private void FixedUpdate()
    {
        float angle = _transform.localEulerAngles.NomalizeRotate180().x;
        _image.transform.localRotation = Quaternion.Euler(0f, 0f, angle + 90.0f);
        string angleText = string.Format("{0:00.0}", -angle);
        if (_text.text != angleText)
        {
            _text.text = angleText;
        }
    }
}
