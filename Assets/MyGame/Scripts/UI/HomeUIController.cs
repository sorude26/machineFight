using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeUIController : MonoBehaviour
{
    [SerializeField] GameObject _menuPanel = default;
    private void Start()
    {
        ButtonSelectController.OnButtonFirstSelect(_menuPanel);
    }
}
