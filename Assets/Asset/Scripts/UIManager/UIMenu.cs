using UnityEngine;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{
    [SerializeField] private MainMenuManager mainMenuManager;
    [Header("Buttons")]
    [SerializeField] private Button playBtn;
    [SerializeField] private Button characterBtn;
    [SerializeField] private Button settingsBtn;
    [SerializeField] private Button quitBtn;
    [Header("SpecialPanels")]
    [SerializeField] private GameObject loadingPanel;

    private UILoadingBar loadingBar;

    private void Start()
    {
        loadingBar = loadingPanel.GetComponent<UILoadingBar>();
        loadingBar.SetLoadingDuration(3f);

        InitializeButtons();
    }
    private void InitializeButtons()
    {
        playBtn.onClick.AddListener(() => mainMenuManager.ShowPanel(UIMenuPanel.Mode));
        characterBtn.onClick.AddListener(() => mainMenuManager.ShowPanel(UIMenuPanel.Character));
        settingsBtn.onClick.AddListener(() => mainMenuManager.ShowPanel(UIMenuPanel.Settings));
        quitBtn.onClick.AddListener(() => Application.Quit());
    }
}
