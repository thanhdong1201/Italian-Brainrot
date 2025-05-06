using UnityEngine;
using UnityEngine.UI;

public class UISettings : MonoBehaviour
{
    [SerializeField] private MainMenuManager mainMenuManager;
    [Header("Buttons")]
    //[SerializeField] private Button playBtn;
    //[SerializeField] private Button settingsBtn;
    [SerializeField] private Button quitBtn;

    private void Start()
    {
        InitializeButtons();
    }
    private void InitializeButtons()
    {
        //playBtn.onClick.AddListener(() => UIManager.Instance.ShowPanel(UIPanel.ChooseMode));
        //settingsBtn.onClick.AddListener(() => UIManager.Instance.ShowPanel(UIPanel.Settings));
        quitBtn.onClick.AddListener(() => mainMenuManager.ShowPanel(UIMenuPanel.Menu));
    }
}
