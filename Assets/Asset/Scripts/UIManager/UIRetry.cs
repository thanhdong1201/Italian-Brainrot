using UnityEngine;
using UnityEngine.UI;

public class UIRetry : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button ignoreRetryBtn;
    [SerializeField] private Button retryWithAdsBtn;

    [Header("Events")]
    [SerializeField] private VoidEventChannelSO onRetry;
    [SerializeField] private VoidEventChannelSO onIgnoreRetry;

    private void Start()
    {
        InitializeButtons();
    }
    private void InitializeButtons()
    {
        ignoreRetryBtn.onClick.AddListener(() => IgnoreRetry());
        retryWithAdsBtn.onClick.AddListener(() => RetryWithAds());
    }
    private void IgnoreRetry()
    {
        UIManager.Instance.ShowPanel(UIPanel.Gameplay);
        onIgnoreRetry.RaiseEvent();
    }
    private void RetryWithAds()
    {
        AdManager.Instance.ShowRewardedAd(() =>
        {
            AnalyticsManager.Instance.LogAdImpression("rewarded");
            AnalyticsManager.Instance.LogRewardedAdCompleted("retry");
            UIManager.Instance.ShowPanel(UIPanel.Gameplay);
            onRetry.RaiseEvent();
        });
    }
}
