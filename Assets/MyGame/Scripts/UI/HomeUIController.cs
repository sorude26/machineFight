using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeUIController : MonoBehaviour
{
    public static HomeUIController Instance { get; private set; }
    [SerializeField]
    private GameObject _menuPanel = default;
    [SerializeField]
    private ModelBuilder _modelBuilder = default;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        ButtonSelectController.OnButtonFirstSelect(_menuPanel);
    }
    public void BuildModel()
    {
        _modelBuilder.ViewModel(PlayerData.instance.BuildPreset);
    }
}
