using UnityEngine;
using UnityEngine.UI;

public class UIRateGame : MonoBehaviour
{
    [SerializeField] private Button rateBtn;
    [SerializeField] private Button closeBtn;
    [SerializeField] private GameObject rateGamePanel;

    [Header("Settings")]
    [SerializeField] private int launchesBeforePrompt = 3;

    private const string HasRatedKey = "App_HasRated";
    private const string LaunchCountKey = "App_LaunchCount";

    private const string GOOGLE_PLAY_URL = "market://details?id={0}&showAllReviews=true";

    private void OnEnable()
    {
        rateBtn.onClick.AddListener(OpenPlayStoreReview);
        closeBtn.onClick.AddListener(() => ResetData());
    }
    private void OnDisable()
    {
        rateBtn.onClick.RemoveAllListeners();
        closeBtn.onClick.RemoveAllListeners();
    }
    private void Start()
    {
        if (HasUserRated())
            return;

        int launchCount = PlayerPrefs.GetInt(LaunchCountKey, 0) + 1;
        PlayerPrefs.SetInt(LaunchCountKey, launchCount);
        PlayerPrefs.Save();

        if (launchCount >= launchesBeforePrompt)
        {
            rateGamePanel.SetActive(true);
        }
    }
    private void OpenPlayStoreReview()
    {
        gameObject.SetActive(false);
        string packageName = Application.identifier;
        string url = string.Format(GOOGLE_PLAY_URL, packageName);
        MarkUserAsRated();
        if (Application.platform == RuntimePlatform.Android)
        {
            Application.OpenURL(url);
        }
    }
    private void MarkUserAsRated()
    {
        PlayerPrefs.SetInt(HasRatedKey, 1);
        PlayerPrefs.Save();
    }

    private bool HasUserRated()
    {
        return PlayerPrefs.GetInt(HasRatedKey, 0) == 1;
    }
    [Button]
    private void ResetData()
    {
        PlayerPrefs.DeleteKey(LaunchCountKey);
        PlayerPrefs.DeleteKey(HasRatedKey);
        PlayerPrefs.Save();
        rateGamePanel.SetActive(false);
        Debug.Log("Data reset: LaunchCount and HasRated keys deleted.");
    }
}