using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRetry : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button ignoreRetryBtn;
    [SerializeField] private Button retryWithAdsBtn;

    [SerializeField] private VoidEventChannelSO onRetry, onIgnoreRety;

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
        onIgnoreRety.RaiseEvent();
    }
    private void RetryWithAds()
    {
        AdManager.Instance.ShowRewardedAd(() =>
        {
            AnalyticsManager.Instance.LogAdImpression("rewarded");
            AnalyticsManager.Instance.LogRewardedAdCompleted("rewarded_retry");
            UIManager.Instance.ShowPanel(UIPanel.Gameplay);
            onRetry.RaiseEvent();
        });
    }
}
