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

    private void Start()
    {
        InitializeButtons();
    }
    private void InitializeButtons()
    {
        quizzBtn.onClick.AddListener(() => QuizzMode());
        quitBtn.onClick.AddListener(() => Quit());
    }
    private void QuizzMode()
    {
        SceneManager.LoadScene("Quizz");
    }
    private void Quit()
    {
        mainMenuManager.ShowPanel(UIMenuPanel.Menu);
    }
}
