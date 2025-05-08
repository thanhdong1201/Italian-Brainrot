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
    private bool isBannerLoaded = false;
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
        //WaitForInitialization(() =>
        //{
        //    ShowBanner();
        //});
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
            Debug.Log("[AdManager] AdMob Initialized at time: " + Time.time);
        });

        WaitForInitialization(() =>
        {
            LoadBannerAd();
            LoadInterstitialAd();
            LoadRewardedAd();
        });
    }
    public void WaitForInitialization(Action action)
    {
        StartCoroutine(Delay(action));
    }
    private IEnumerator Delay(Action action)
    {
        yield return new WaitUntil(() => IsAdMobInitialized);
        action?.Invoke();
    }
    #region Banner Ads
    public void LoadBannerAd()
    {
        if (bannerView != null && isBannerLoaded)
        {
            return; // Không reload nếu đã load
        }

        bannerView?.Destroy();
        bannerView = null;

        //int width = (int)(Screen.width / Screen.dpi);
        //AdSize adSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(width);
        //bannerView = new BannerView(bannerAdUnitId, adSize, AdPosition.Bottom);
        bannerView = new BannerView(bannerAdUnitId, AdSize.Banner, AdPosition.Bottom);

        AdRequest request = new AdRequest();
        request.Extras.Add("npa", "1");

        bannerView.LoadAd(request);
        isBannerLoaded = false;

        bannerView.OnBannerAdLoaded += () =>
        {
            isBannerLoaded = true;
            Debug.Log("[AdManager] Banner Ad Loaded Successfully at time: " + Time.time);
        };

        bannerView.OnBannerAdLoadFailed += (error) =>
        {
            isBannerLoaded = false;
            Debug.LogWarning($"[AdManager] Banner Ad Failed to load: {error}");
            Invoke(nameof(LoadBannerAd), 10f);
        };
    }
    [Button]
    public bool ShowBanner()
    {
        if (bannerView != null && isBannerLoaded)
        {
            bannerView.Show();
            Debug.Log("[AdManager] Banner Ad Shown");
            return true;
        }

        //Debug.LogWarning("[AdManager] Banner Ad not ready, reloading");
        LoadBannerAd();
        return false;
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