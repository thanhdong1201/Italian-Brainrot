using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject chooseModePanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject quizzModePanel;
    [SerializeField] private GameObject completePanel;
    [SerializeField] private GameObject gameOverPanel;

    [Header("SpecialPanels")]
    [SerializeField] private GameObject loadingPanel;

    private Dictionary<UIPanel, GameObject> uiPanels;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        InitializeUIPanels();
    }
    private void InitializeUIPanels()
    {
        uiPanels = new Dictionary<UIPanel, GameObject>
        {
            { UIPanel.MainMenu, mainMenuPanel },
            { UIPanel.ChooseMode, chooseModePanel },
            { UIPanel.Settings, settingsPanel },
            { UIPanel.QuizzMode, quizzModePanel },
            { UIPanel.Complete, completePanel },
            { UIPanel.GameOver, gameOverPanel }
        };
        ShowPanel(UIPanel.MainMenu);
    }
    public void ShowPanel(UIPanel panel)
    {
        foreach (var uiPanel in uiPanels)
        {
            uiPanel.Value.SetActive(uiPanel.Key == panel);
        }
        if(panel == UIPanel.QuizzMode)
        {
            loadingPanel.SetActive(true);
        }
    }
}
public enum UIPanel
{
    MainMenu,
    ChooseMode,
    Settings,
    QuizzMode,
    Complete,
    GameOver
}
