using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMoveAxis : MonoBehaviour
{
    const float WIDTH_AND_HEIGHT = 50f;

    [SerializeField]
    RectTransform image;

    private void FixedUpdate()
    {
        if (PlayerVrCockpit.Instance == null) return;
        Vector2 input = PlayerVrCockpit.Move();
        input *= WIDTH_AND_HEIGHT;
        image.localPosition = input;
    }
}
