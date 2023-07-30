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
    [SerializeField]
    private Canvas _canvas = null;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        ButtonSelectController.OnButtonFirstSelect(_menuPanel);
        if (OVRManager.isHmdPresent == false && PlayerVrCockpit.Instance.IsDebagVR == false)
        {
            _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }
    }
    public void BuildModel()
    {
        _modelBuilder.ViewModel(PlayerData.instance.BuildPreset);
    }
    public void BuildModel(PartsBuildParam viewParam)
    {
        _modelBuilder.ViewChange(viewParam);
    }
}
