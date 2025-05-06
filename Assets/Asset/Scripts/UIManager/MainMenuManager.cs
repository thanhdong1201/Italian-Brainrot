using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject chooseModePanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("SpecialPanels")]
    [SerializeField] private GameObject loadingPanel;

    [Header("Wallpapers")]
    [SerializeField] private List<GameObject> wallpapers;

    private Dictionary<UIMenuPanel, GameObject> uiPanels;

    private void Awake()
    {
    }
    private void Start()
    {
        InitializeUIPanels();
    }
    private void OnEnable()
    {
        ChangeWallpaper();
    }
    #region UIPanels
    private void InitializeUIPanels()
    {
        uiPanels = new Dictionary<UIMenuPanel, GameObject>
        {
            { UIMenuPanel.Menu, mainMenu },
            { UIMenuPanel.ChooseMode, chooseModePanel },
            { UIMenuPanel.Settings, settingsPanel },

        };
        ShowPanel(UIMenuPanel.Menu);
    }
    public void ShowPanel(UIMenuPanel panel)
    {
        foreach (var uiPanel in uiPanels)
        {
            uiPanel.Value.SetActive(uiPanel.Key == panel);
        }
    }
    #endregion
    #region Wallpaper
    [Button]
    private void ChangeWallpaper()
    {
        int randomIndex = Random.Range(0, wallpapers.Count);
        for (int i = 0; i < wallpapers.Count; i++)
        {
            wallpapers[i].SetActive(i == randomIndex);
        }
    }
    #endregion
}
public enum UIMenuPanel
{
    Menu,
    ChooseMode,
    Settings,
}
