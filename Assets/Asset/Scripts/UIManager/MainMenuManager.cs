using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject chooseModePanel;
    [SerializeField] private GameObject settingsPanel;

    private Dictionary<UIMenuPanel, GameObject> uiPanels;

    private void Start()
    {
        InitializeUIPanels();
    }

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
}
public enum UIMenuPanel
{
    Menu,
    ChooseMode,
    Settings,
}
