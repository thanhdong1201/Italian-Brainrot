using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public LoadSceneManager LoadSceneManager;
    //public Timer Timer { get; private set; }

    [Header("Listening from Events")]
    [SerializeField] private VoidEventChannelSO showInterstitial;
    [Header("Broadscasting to Events")]
    [SerializeField] private VoidEventChannelSO onStartGame;
    [SerializeField] private VoidEventChannelSO onChangeWallpaper;

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

        //Timer = GetComponent<Timer>();
    }
    private void Start()
    {

    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        showInterstitial.OnEventRaised += ShowInterstitialAd;
        onChangeWallpaper.OnEventRaised += ShowRewardedAdForChangeWallpaper;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        showInterstitial.OnEventRaised -= ShowInterstitialAd;
        onChangeWallpaper.OnEventRaised -= ShowRewardedAdForChangeWallpaper;
    }
    private void Update()
    {
        //Timer.UpdateTimer();
    }
    #region GameControl
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;
        SoundManager.Instance.StopMusic();

        if (AdManager.Instance != null)
        {
            AdManager.Instance.WaitForInitialization(() => AdManager.Instance.ShowBanner());
        }
    }
    public void StartGame()
    {
        UIManager.Instance.ShowPanel(UIPanel.Gameplay);
        onStartGame?.RaiseEvent(); 
    }
    #endregion
    #region Admob & Firebase Analytics
    private void ShowInterstitialAd()
    {
        if (AdManager.Instance.ShowInterstitial())
        {
            AnalyticsManager.Instance.LogAdImpression("interstitial");
        }
    }
    private void ShowRewardedAdForChangeWallpaper()
    {
        AdManager.Instance.ShowRewardedAd(() =>
        {
            AnalyticsManager.Instance.LogAdImpression("rewarded");
            AnalyticsManager.Instance.LogRewardedAdCompleted("change_wallpaper");
            WallpaperManager.Instance.ChangeWallpaper();
        });
    }
    #endregion

    #region Editor
#if UNITY_EDITOR
    [Button]
    private void ResetSaveGame()
    {
        SaveManager.ResetAll();
    }
#endif
    #endregion
}
