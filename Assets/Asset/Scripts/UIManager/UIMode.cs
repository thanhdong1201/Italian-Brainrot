using UnityEngine;
using UnityEngine.UI;

public class UIMode : MonoBehaviour
{
    [SerializeField] private MainMenuManager mainMenuManager;
    [SerializeField] private ListQuizzSO listQuizzSO;
    [Header("Buttons")]
    [SerializeField] private Button easyBtn;
    [SerializeField] private Button mediumBtn;
    [SerializeField] private Button hardBtn;
    [SerializeField] private Button quitBtn;
    [Header("SpecialPanels")]
    [SerializeField] private GameObject loadingPanel;

    private UILoadingBar loadingBar;

    private void OnEnable()
    {
        
    }
    private void OnDestroy()
    {
        easyBtn.onClick.RemoveAllListeners();
        mediumBtn.onClick.RemoveAllListeners();
        hardBtn.onClick.RemoveAllListeners();
        quitBtn.onClick.RemoveAllListeners();
    }
    private void Start()
    {
        loadingBar = loadingPanel.GetComponent<UILoadingBar>();
        loadingBar.SetLoadingDuration(3f);

        InitializeButtons();
    }
    private void InitializeButtons()
    {
        easyBtn.onClick.AddListener(() => LoadScene(QuizzMode.Easy));
        mediumBtn.onClick.AddListener(() => LoadScene(QuizzMode.Medium));
        hardBtn.onClick.AddListener(() => LoadScene(QuizzMode.Hard));
        quitBtn.onClick.AddListener(() => Quit());
    }
    private void LoadScene(QuizzMode mode)
    {
        listQuizzSO.SetQuizz(mode);
        loadingPanel.SetActive(true);
        GameManager.Instance.LoadSceneManager.PrepareToLoadScene(3f);
    }
    private void Quit()
    {
        mainMenuManager.ShowPanel(UIMenuPanel.Menu);
    }
}
