using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Timer Timer { get; private set; }
    public QuizzManager QuizzManager;

    private void Awake()
    {
        // Nếu đã có instance khác tồn tại trong scene, hủy bản mới này
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        Timer = GetComponent<Timer>();
    }
    private void OnDestroy()
    {
        // Chỉ reset nếu instance này chính là cái bị hủy
        if (Instance == this)
        {
            Instance = null;
        }
    }
    private void Start()
    {

    }
    public void StartGame()
    {
        UIManager.Instance.ShowPanel(UIPanel.Gameplay);
        QuizzManager.ActiveNewQuizz();
    }
}
