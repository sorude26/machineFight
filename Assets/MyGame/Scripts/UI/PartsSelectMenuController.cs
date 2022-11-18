using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame;

public class PartsSelectMenuController : MonoBehaviour
{
    [SerializeField] GameObject _menuPanel = default;
    // Start is called before the first frame update
    void Start()
    {
        PlayerInput.SetEnterInput(InputMode.Menu, InputType.Fire3, ClosePanel);
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (this.gameObject.transform.childCount > 0)
        {
            ButtonSelectController.OnButtonFirstSelect(this.gameObject);
        }
    }

    private void ClosePanel()
    {
        if (this.gameObject.activeSelf == true)
        {
            ButtonSelectController.OnButtonNonSelect();
            _menuPanel.SetActive(true);
            ButtonSelectController.OnButtonFirstSelect(_menuPanel);
            this.gameObject.SetActive(false);
        }
        
    }
}
