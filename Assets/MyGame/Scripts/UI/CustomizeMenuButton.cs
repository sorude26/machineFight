using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeMenuButton : MonoBehaviour
{
    [SerializeField] GameObject _customizePanel = default;
    void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(CustomizeMenuOpen);
    }


    private void CustomizeMenuOpen()
    {
        ButtonSelectController.OnButtonNonSelect();
        _customizePanel.SetActive(true);
        this.gameObject.transform.parent.gameObject.SetActive(false);
    }
}
