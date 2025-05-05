using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Timer Timer { get; private set; }
    public QuizzManager QuizzManager { get; private set; }
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
        QuizzManager = FindFirstObjectByType<QuizzManager>();
    }
    private void Start()
    {

    }
    public void StartGame()
    {
        QuizzManager.StartGame();
    }
}
