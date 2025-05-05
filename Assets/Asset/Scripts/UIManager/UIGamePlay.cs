using UnityEngine;
using UnityEngine.UI;

public class UIGamePlay : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button pauseBtn;
    [SerializeField] private Button resumeBtn;
    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;

    private void Start()
    {
        InitializeButtons();
    }
    private void InitializeButtons()
    {
        pauseBtn.onClick.AddListener(() => PauseGame(true));
        resumeBtn.onClick.AddListener(() => PauseGame(false));
    }
    private void PauseGame(bool pause)
    {
        if (pause)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
