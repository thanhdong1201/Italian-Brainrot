using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIPause : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button resumeBtn;
    [SerializeField] private Button menuBtn;

    private void Start()
    {
        InitializeButtons();
    }
    private void InitializeButtons()
    {
        resumeBtn.onClick.AddListener(() => UIManager.Instance.PauseGame(false));
        menuBtn.onClick.AddListener(() => SceneManager.LoadScene("Menu"));
    }
}
