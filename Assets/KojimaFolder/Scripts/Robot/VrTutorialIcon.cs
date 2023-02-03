using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VrTutorialIcon : MonoBehaviour
{
    Vector3 _defaultScale;
    float _addScale;

    private void Start()
    {
        _defaultScale = this.transform.localScale;
        StartCoroutine(UpDownScale(-0.5f, 0.5f));
    }

    private void Update()
    {
        //ƒvƒŒƒCƒ„[‚Ì•û‚ðŒü‚­
        Vector3 eyePos = VrEyeLocater.GetPosition()?.position ?? Vector3.zero;
        Vector3 look = this.transform.position - eyePos;
        Quaternion rotate = Quaternion.LookRotation(look);
        Vector3 angle = rotate.eulerAngles.NomalizeRotate180();
        angle.x = 0;
        this.transform.eulerAngles = angle;

        //Žûk‚³‚¹‚é
        this.transform.localScale = _defaultScale + new Vector3(0, _addScale, 0);
    }

    IEnumerator UpDownScale(float min, float max)
    {
        bool up = true;
        _addScale = 0;
        while (true)
        {
            _addScale += up ? Time.deltaTime : -Time.deltaTime;
            if (_addScale > max)
            {
                up = false;
                _addScale = max;
            }
            if (_addScale < min)
            {
                up = true;
                _addScale = min;
            }
            yield return null;
        }
    }
}
