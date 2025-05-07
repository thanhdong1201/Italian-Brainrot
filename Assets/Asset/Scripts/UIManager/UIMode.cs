using UnityEngine;
using UnityEngine.UI;

public class UIMode : MonoBehaviour
{
    [SerializeField] private MainMenuManager mainMenuManager;
    [SerializeField] private ListQuizzSO listQuizzSO;
    [Header("Buttons")]
    [SerializeField] private Button easyQuizzBtn;
    [SerializeField] private Button mediumQuizzBtn;
    [SerializeField] private Button hardQuizzBtn;
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
        easyQuizzBtn.onClick.AddListener(() => LoadScene(DifficultyLevel.Easy));
        mediumQuizzBtn.onClick.AddListener(() => LoadScene(DifficultyLevel.Medium));
        hardQuizzBtn.onClick.AddListener(() => LoadScene(DifficultyLevel.Hard));
        quitBtn.onClick.AddListener(() => Quit());
    }

    public void LoadScene(DifficultyLevel difficultyLevel)
    {
        listQuizzSO.SetQuizz(difficultyLevel);
        loadingPanel.SetActive(true);
        GameManager.Instance.LoadSceneManager.PrepareToLoadScene(3f);
    }
    private void Quit()
    {
        mainMenuManager.ShowPanel(UIMenuPanel.Menu);
    }
}
