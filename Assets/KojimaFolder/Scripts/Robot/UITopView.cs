using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITopView : MonoBehaviour
{
    [SerializeField]
    PlayerMachineController _machine;
    [SerializeField]
    Transform _looktransform;
    [SerializeField]
    RectTransform _image;
    [SerializeField]
    Text _text;

    private void FixedUpdate()
    {
        if (_machine == null) return;
        Transform leg = _machine.MachineController.LegController.LegBase.transform;
        Vector3 rotate = (leg.eulerAngles - _looktransform.eulerAngles).NomalizeRotate180();
        float angle = rotate.y;
        angle = -angle;
        _image.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
        string angleText = string.Format("{0:00.0}", angle);
        if (_text.text != angleText)
        {
            _text.text = angleText;
        }
    }
}
