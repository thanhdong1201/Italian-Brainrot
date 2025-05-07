using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdController : MonoBehaviour
{
    [Header("Listening from Events")]
    [SerializeField] private VoidEventChannelSO showInterstitial;
    [SerializeField] private VoidEventChannelSO showRewarded;
    [Header("Broadscasting to Events")]
    [SerializeField] private VoidEventChannelSO onChangeWallpaper;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        showInterstitial.OnEventRaised += ShowInterstitialAd;
        showRewarded.OnEventRaised += ShowRewardedAd;
        onChangeWallpaper.OnEventRaised += ShowRewardedAdForChangeWallpaper;
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        showInterstitial.OnEventRaised -= ShowInterstitialAd;
        showRewarded.OnEventRaised -= ShowRewardedAd;
        onChangeWallpaper.OnEventRaised -= ShowRewardedAdForChangeWallpaper;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AdManager.Instance.ShowBanner();
    }
    [Button]
    private void ShowInterstitialAd()
    {
        if (AdManager.Instance.ShowInterstitial())
        {
            //AnalyticsManager.Instance.LogAdImpression("interstitial");
            Debug.Log("Interstitial ad shown.");
        }
    }
    [Button]
    private void ShowRewardedAd()
    {
        AdManager.Instance.ShowRewardedAd(() =>
        {
            //AnalyticsManager.Instance.LogAdImpression("rewarded");
            Debug.Log("Rewarded ad shown.");
        });
    }
    [Button]
    private void ShowRewardedAdForChangeWallpaper()
    {
        AdManager.Instance.ShowRewardedAd(() =>
        {
            //AnalyticsManager.Instance.LogAdImpression("rewarded");
            WallpaperManager.Instance.ChangeWallpaper();
            Debug.Log("Wallpaper changed");
        });
    }
}