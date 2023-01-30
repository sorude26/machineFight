using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICameraAxis : MonoBehaviour
{
    const float WIDTH_AND_HEIGHT = 50f;

    [SerializeField]
    RectTransform image;

    private void FixedUpdate()
    {
        if (PlayerVrCockpit.Instance == null) return;
        Vector2 input = PlayerVrCockpit.Camera();
        input *= WIDTH_AND_HEIGHT;
        image.localPosition = input;
    }
}
