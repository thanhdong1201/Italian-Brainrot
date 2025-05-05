using UnityEngine;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button playBtn;
    [SerializeField] private Button settingsBtn;
    [SerializeField] private Button quitBtn;

    private void Start()
    {
        InitializeButtons();
    }
    private void InitializeButtons()
    {
        playBtn.onClick.AddListener(() => UIManager.Instance.ShowPanel(UIPanel.ChooseMode));
        settingsBtn.onClick.AddListener(() => UIManager.Instance.ShowPanel(UIPanel.Settings));
        quitBtn.onClick.AddListener(() => Application.Quit());
    }
}
