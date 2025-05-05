using UnityEngine;
using UnityEngine.UI;

public class UIMode : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button quizzBtn;
    //[SerializeField] private Button miniGameBtn;
    [SerializeField] private Button quitBtn;

    private void Start()
    {
        InitializeButtons();
    }
    private void InitializeButtons()
    {
        quizzBtn.onClick.AddListener(() => UIManager.Instance.ShowPanel(UIPanel.QuizzMode));
        quitBtn.onClick.AddListener(() => UIManager.Instance.ShowPanel(UIPanel.MainMenu));
    }
}
