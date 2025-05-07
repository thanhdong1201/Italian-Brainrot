using UnityEngine;
using GoogleMobileAds.Api;
using System;
using System.Collections;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance { get; private set; }

    [Header("AdMob Ad Unit IDs (Replace with real IDs in production)")]
    [SerializeField] private string bannerAdUnitId = "ca-app-pub-3940256099942544/6300978111";
    [SerializeField] private string interstitialAdUnitId = "ca-app-pub-3940256099942544/1033173712";
    [SerializeField] private string rewardedAdUnitId = "ca-app-pub-3940256099942544/5224354917";

    [Header("Ad Settings")]
    [SerializeField] private float interstitialCooldown = 10f;

    public Action OnRewardedAdCompleted { get; set; }

    private BannerView bannerView;
    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;
    private float lastInterstitialTime;
    public bool IsAdMobInitialized { get; private set; } = false;

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
    }

    private void Start()
    {
        InitializeAdMob();
        ShowBanner();
    }

    private void InitializeAdMob()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.LogWarning("[AdManager] No internet connection, retrying in 10 seconds.");
            Invoke(nameof(InitializeAdMob), 10f);
            return;
        }

        // Cấu hình quảng cáo dành cho trẻ em (COPPA) và không cá nhân hóa
        RequestConfiguration requestConfiguration = new RequestConfiguration
        {
            TagForChildDirectedTreatment = TagForChildDirectedTreatment.True,
            MaxAdContentRating = MaxAdContentRating.G
        };
        MobileAds.SetRequestConfiguration(requestConfiguration);

        MobileAds.SetiOSAppPauseOnBackground(true);
        MobileAds.Initialize(initStatus =>
        {
            IsAdMobInitialized = true;
            LoadBannerAd();
            LoadInterstitialAd();
            LoadRewardedAd();
            Debug.Log("[AdManager] AdMob Initialized at time: " + Time.time);
        });
    }

    #region Banner Ads
    public void LoadBannerAd()
    {
        Debug.Log("[AdManager] Loading Banner Ad...");
        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        int width = (int)(Screen.width / Screen.dpi);
        bannerView = new BannerView(bannerAdUnitId, AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(width), AdPosition.Bottom);
        //bannerView = new BannerView(bannerAdUnitId, AdSize.Banner, AdPosition.Bottom);

        AdRequest request = new AdRequest();
        request.Extras.Add("npa", "1"); // Yêu cầu quảng cáo không cá nhân hóa

        bannerView.LoadAd(request);
        bannerView.Hide();

        bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("[AdManager] Banner Ad Loaded Successfully at time: " + Time.time);
            bannerView.Hide();
        };

        bannerView.OnBannerAdLoadFailed += (error) =>
        {
            Debug.LogWarning($"[AdManager] Banner Ad Failed to load: {error} at time: {Time.time}");
            Invoke(nameof(LoadBannerAd), 10f);
        };
    }
    private IEnumerator WaitAndShowBanner()
    {
        yield return new WaitUntil(() => IsAdMobInitialized);
        //yield return new WaitForSeconds(5f);

        if (bannerView != null)
        {
            bannerView.Show();
            Debug.Log("[AdManager] Banner Ad Shown");
        }
        else
        {
            Debug.LogWarning("[AdManager] Banner Ad not ready, reloading");
            LoadBannerAd();
        }
    }
    public void ShowBanner()
    {
        StartCoroutine(WaitAndShowBanner());
    }
    public void HideBanner()
    {
        bannerView?.Hide();
    }
    #endregion

    #region Interstitial Ads
    public void LoadInterstitialAd()
    {
        interstitialAd?.Destroy();

        AdRequest request = new AdRequest();
        request.Extras.Add("npa", "1");

        InterstitialAd.Load(interstitialAdUnitId, request, (ad, error) =>
        {
            if (error != null)
            {
                Debug.LogWarning($"[AdManager] Interstitial Ad Failed: {error.GetMessage()}");
                Invoke(nameof(LoadInterstitialAd), 10f);
                return;
            }

            interstitialAd = ad;

            interstitialAd.OnAdPaid += (adValue) =>
                Debug.Log($"[AdManager] Interstitial Ad Paid: {adValue.Value / 1_000_000f} {adValue.CurrencyCode}");

            interstitialAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("[AdManager] Interstitial Ad Closed");
                LoadInterstitialAd();
            };

            interstitialAd.OnAdFullScreenContentFailed += (err) =>
            {
                Debug.LogWarning($"[AdManager] Interstitial Ad Failed to show: {err.GetMessage()}");
                LoadInterstitialAd();
            };
        });
    }

    public bool ShowInterstitial()
    {
        if (Time.time - lastInterstitialTime < interstitialCooldown)
        {
            return false;
        }

        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
            lastInterstitialTime = Time.time;
            Debug.Log("[AdManager] Interstitial Ad Shown");
            return true;
        }

        Debug.LogWarning("[AdManager] Interstitial Ad not ready, reloading");
        LoadInterstitialAd();
        return false;
    }
    #endregion

    #region Rewarded Ads
    public void LoadRewardedAd()
    {
        rewardedAd?.Destroy();

        AdRequest request = new AdRequest();
        request.Extras.Add("npa", "1");

        RewardedAd.Load(rewardedAdUnitId, request, (ad, error) =>
        {
            if (error != null)
            {
                Debug.LogWarning($"[AdManager] Rewarded Ad Failed: {error.GetMessage()}");
                Invoke(nameof(LoadRewardedAd), 10f);
                return;
            }

            rewardedAd = ad;

            rewardedAd.OnAdPaid += (adValue) =>
                Debug.Log($"[AdManager] Rewarded Ad Paid: {adValue.Value / 1_000_000f} {adValue.CurrencyCode}");

            rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("[AdManager] Rewarded Ad Closed");
                LoadRewardedAd();
            };

            rewardedAd.OnAdFullScreenContentFailed += (err) =>
            {
                Debug.LogWarning($"[AdManager] Rewarded Ad Failed to show: {err.GetMessage()}");
                LoadRewardedAd();
            };
        });
    }

    public bool ShowRewardedAd(Action onRewardComplete = null)
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show(reward =>
            {
                Debug.Log("[AdManager] User earned reward");
                OnRewardedAdCompleted?.Invoke();
                onRewardComplete?.Invoke();
            });
            Debug.Log("[AdManager] Rewarded Ad Shown");
            return true;
        }

        Debug.LogWarning("[AdManager] Rewarded Ad not ready, reloading");
        LoadRewardedAd();
        return false;
    }
    #endregion

    private void OnDestroy()
    {
        bannerView?.Destroy();
        interstitialAd?.Destroy();
        rewardedAd?.Destroy();
    }
}