using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISystemStartMonitor : MonoBehaviour
{
    [SerializeField]
    float _min;
    [SerializeField]
    float _max;
    [SerializeField]
    float _speed;
    [SerializeField]
    RectTransform _barLeftRight;
    [SerializeField]
    RectTransform _barUpDown;

    private void Start()
    {
        StartCoroutine(LeftRight(_barLeftRight, _min, _max, _speed));
        StartCoroutine(UpDown(_barUpDown, _min, _max, _speed));
    }

    IEnumerator LeftRight(RectTransform target, float min, float max, float speed)
    {
        while (target.localPosition.x < max)
        {
            target.localPosition = target.localPosition + new Vector3(speed, 0, 0);
            yield return new WaitForFixedUpdate();
        }
        while (target.localPosition.x > min)
        {
            target.localPosition = target.localPosition + new Vector3(-speed, 0, 0);
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator UpDown(RectTransform target, float min, float max, float speed)
    {
        while (target.localPosition.y < max)
        {
            target.localPosition = target.localPosition + new Vector3(0, speed, 0);
            yield return new WaitForFixedUpdate();
        }
        while (target.localPosition.y > min)
        {
            target.localPosition = target.localPosition + new Vector3(0, -speed, 0);
            yield return new WaitForFixedUpdate();
        }
    }
}
