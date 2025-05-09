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

    private void Start()
    {
        InitializeButtons();
    }
    private void InitializeButtons()
    {
        playBtn.onClick.AddListener(() => mainMenuManager.ShowPanel(UIMenuPanel.ChooseMode));
        characterBtn.onClick.AddListener(() => mainMenuManager.ShowPanel(UIMenuPanel.Character));
        settingsBtn.onClick.AddListener(() => mainMenuManager.ShowPanel(UIMenuPanel.Settings));
        quitBtn.onClick.AddListener(() => Application.Quit());
    }
}
