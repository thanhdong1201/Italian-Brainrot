using UnityEngine;
using UnityEngine.UI;

public class UIGamePlay : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button pauseBtn;

    private void Start()
    {
        InitializeButtons();
    }
    private void InitializeButtons()
    {
        pauseBtn.onClick.AddListener(() => UIManager.Instance.PauseGame(true));
    }
}
