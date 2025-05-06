using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMode : MonoBehaviour
{
    [SerializeField] private MainMenuManager mainMenuManager;
    [Header("Buttons")]
    [SerializeField] private Button quizzBtn;
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
        quizzBtn.onClick.AddListener(() => LoadScene());
        quitBtn.onClick.AddListener(() => Quit());
    }

    public void LoadScene()
    {
        loadingPanel.SetActive(true);
        GameManager.Instance.LoadSceneManager.PrepareToLoadScene(3f);
    }
    private void Quit()
    {
        mainMenuManager.ShowPanel(UIMenuPanel.Menu);
    }
}
