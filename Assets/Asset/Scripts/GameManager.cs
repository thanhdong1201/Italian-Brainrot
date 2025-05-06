using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public LoadSceneManager LoadSceneManager;
    public Timer Timer { get; private set; }

    [Header("Broadcasting Events")]
    [SerializeField] private VoidEventChannelSO onStartGame;    
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        Timer = GetComponent<Timer>();
    }

    public void StartGame()
    {
        UIManager.Instance.ShowPanel(UIPanel.Gameplay);
        onStartGame?.RaiseEvent();
    }
}
