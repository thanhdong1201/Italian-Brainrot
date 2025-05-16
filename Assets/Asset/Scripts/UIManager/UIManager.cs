using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [Header("Panels")]
    [SerializeField] private GameObject gameplayPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject completePanel;
    [SerializeField] private GameObject retryPanel;

    [Header("SpecialPanels")]
    [SerializeField] private GameObject loadingPanel;

    private Dictionary<UIPanel, GameObject> uiPanels;

    private void Awake()
    {
        // Nếu đã có instance khác tồn tại trong scene, hủy bản mới này
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        InitializeUIPanels();
    }
    private void OnDestroy()
    {
        // Chỉ reset nếu instance này chính là cái bị hủy
        if (Instance == this)
        {
            Instance = null;
        }
    }
    private void InitializeUIPanels()
    {
        uiPanels = new Dictionary<UIPanel, GameObject>
        {
            { UIPanel.Gameplay, gameplayPanel },
            { UIPanel.Pause, pausePanel },
            { UIPanel.Complete, completePanel },
            { UIPanel.Retry, retryPanel },
        };
    }
    public void ShowPanel(UIPanel panel)
    {
        foreach (var uiPanel in uiPanels)
        {
            uiPanel.Value.SetActive(uiPanel.Key == panel);
        }
    }
    public void PauseGame(bool pause)
    {
        if (pause)
        {
            ShowPanel(UIPanel.Pause);
            SoundManager.Instance.PauseMusic(true);
            Time.timeScale = 0;
        }
        else
        {
            ShowPanel(UIPanel.Gameplay);
            SoundManager.Instance.PauseMusic(false);
            Time.timeScale = 1;
        }
    }
}
public enum UIPanel
{
    Gameplay,
    Pause,
    Complete,
    GameOver,
    Retry,
}
