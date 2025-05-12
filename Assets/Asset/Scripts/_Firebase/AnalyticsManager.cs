using Firebase;
using Firebase.Analytics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance { get; private set; }

    private bool isInitialized;
    private const float RETRY_DELAY = 5f;

    private List<Action> pendingEvents = new List<Action>();
    private Coroutine checkNetworkCoroutine;

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
        InitializeFirebase();
    }

    private void InitializeFirebase()
    {
        if (!IsNetworkAvailable())
        {
            Debug.LogWarning("[AnalyticsManager] No internet, retrying initialization...");
            ScheduleRetry();
            return;
        }

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.IsFaulted || task.Result != DependencyStatus.Available)
            {
                Debug.LogError($"[AnalyticsManager] Firebase init failed: {task.Exception?.Message ?? task.Result.ToString()}");
                Debug.Log("[AnalyticsManager] Retrying initialization...");
                ScheduleRetry();
                return;
            }

            try
            {
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                FirebaseAnalytics.SetSessionTimeoutDuration(new TimeSpan(0, 10, 0));
                isInitialized = true;
                Debug.Log("[AnalyticsManager] Firebase Analytics initialized");
                if (pendingEvents.Count > 0 && checkNetworkCoroutine == null)
                {
                    checkNetworkCoroutine = StartCoroutine(CheckNetworkAndSendPendingEvents());
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[AnalyticsManager] Firebase setup failed: {ex.Message}\n{ex.StackTrace}");
                Debug.Log("[AnalyticsManager] Retrying initialization...");
                ScheduleRetry();
            }
        });
    }

    private void ScheduleRetry() => Invoke(nameof(InitializeFirebase), RETRY_DELAY);

    private bool IsNetworkAvailable() => Application.internetReachability != NetworkReachability.NotReachable;

    private IEnumerator CheckNetworkAndSendPendingEvents()
    {
        while (pendingEvents.Count > 0)
        {
            if (IsNetworkAvailable() && isInitialized)
            {
                Debug.Log("[AnalyticsManager] Network restored, sending pending events...");
                var eventsToSend = new List<Action>(pendingEvents);
                pendingEvents.Clear();
                foreach (var evt in eventsToSend)
                {
                    evt?.Invoke();
                }
            }
            yield return new WaitForSeconds(RETRY_DELAY);
        }
        checkNetworkCoroutine = null;
    }

    private void LogEvent(string methodName, Action logAction)
    {
        if (!isInitialized || !IsNetworkAvailable())
        {
            pendingEvents.Add(logAction);
            if (checkNetworkCoroutine == null)
            {
                checkNetworkCoroutine = StartCoroutine(CheckNetworkAndSendPendingEvents());
            }
            Debug.Log($"[AnalyticsManager] {methodName} queued: Firebase not initialized or no network");
            return;
        }

        logAction?.Invoke();
    }

    public void LogLevelStart(string levelName)
    {
        LogEvent("LogLevelStart", () =>
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart, new Parameter("level_name", levelName));
            Debug.Log($"[AnalyticsManager] Event logged: level_start | Level: {levelName}");
        });
    }

    public void LogLevelComplete(string levelName, string starRating)
    {
        LogEvent("LogLevelComplete", () =>
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelEnd,
                new Parameter("level_name", levelName),
                new Parameter("star_rating", starRating));
            Debug.Log($"[AnalyticsManager] Event logged: level_complete | Level: {levelName} | Star Rating: {starRating}");
        });
    }

    public void LogAdImpression(string adType)
    {
        LogEvent("LogAdImpression", () =>
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventAdImpression, new Parameter("ad_type", adType));
            Debug.Log($"[AnalyticsManager] Event logged: ad_impression | Type: {adType}");
        });
    }

    public void LogRewardedAdCompleted(string rewardType)
    {
        LogEvent("LogRewardedAdCompleted", () =>
        {
            FirebaseAnalytics.LogEvent("rewarded_ad_completed", new Parameter("reward_type", rewardType));
            Debug.Log($"[AnalyticsManager] Event logged: rewarded_ad_completed | Reward: {rewardType}");
        });
    }

    public void WaitForInitialization(Action action)
    {
        StartCoroutine(Delay(action));
    }

    private IEnumerator Delay(Action action)
    {
        yield return new WaitUntil(() => isInitialized);
        action?.Invoke();
    }
}