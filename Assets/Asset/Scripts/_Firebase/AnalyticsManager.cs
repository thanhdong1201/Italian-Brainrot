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
    private const string GAME_NAME = "Italian Brainrot: IQ Mystery";
    private const int MAX_RETRY_ATTEMPTS = 3;
    private const float RETRY_DELAY = 5f;

    private List<Action> pendingEvents = new List<Action>();
    private int retryCount;

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
        StartCoroutine(CheckNetworkAndSendPendingEvents());

        WaitForInitialization(() =>
        {
            LogGameStart();
        });
    }

    private void InitializeFirebase()
    {
        if (!IsNetworkAvailable())
        {
            Debug.LogWarning("[GameAnalyticsManager] No internet, retrying initialization...");
            ScheduleRetry();
            return;
        }

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.IsFaulted || task.Result != DependencyStatus.Available)
            {
                Debug.LogError($"[GameAnalyticsManager] Firebase init failed: {task.Exception?.Message ?? task.Result.ToString()}");
                if (retryCount < MAX_RETRY_ATTEMPTS)
                {
                    retryCount++;
                    Debug.Log($"[GameAnalyticsManager] Retrying initialization (Attempt {retryCount}/{MAX_RETRY_ATTEMPTS})...");
                    ScheduleRetry();
                }
                else
                {
                    Debug.LogError("[GameAnalyticsManager] Max retry attempts reached. Analytics unavailable.");
                }
                return;
            }

            try
            {
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                FirebaseAnalytics.SetSessionTimeoutDuration(new TimeSpan(0, 10, 0));
                isInitialized = true;
                retryCount = 0;
                Debug.Log($"[GameAnalyticsManager] Firebase Analytics initialized");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[GameAnalyticsManager] Firebase setup failed: {ex.Message}");
                if (retryCount < MAX_RETRY_ATTEMPTS)
                {
                    retryCount++;
                    Debug.Log($"[GameAnalyticsManager] Retrying initialization (Attempt {retryCount}/{MAX_RETRY_ATTEMPTS})...");
                    ScheduleRetry();
                }
                else
                {
                    Debug.LogError("[GameAnalyticsManager] Max retry attempts reached. Analytics unavailable.");
                }
            }
        });
    }

    private void ScheduleRetry() => Invoke(nameof(InitializeFirebase), RETRY_DELAY);

    private bool IsNetworkAvailable() => Application.internetReachability != NetworkReachability.NotReachable;

    private IEnumerator CheckNetworkAndSendPendingEvents()
    {
        while (true)
        {
            if (IsNetworkAvailable() && isInitialized && pendingEvents.Count > 0)
            {
                Debug.Log("[GameAnalyticsManager] Network restored, sending pending events...");
                var eventsToSend = new List<Action>(pendingEvents);
                pendingEvents.Clear();
                foreach (var evt in eventsToSend)
                {
                    evt?.Invoke();
                }
            }
            yield return new WaitForSeconds(RETRY_DELAY);
        }
    }

    private void LogEvent(string methodName, Action logAction)
    {
        if (!isInitialized)
        {
            pendingEvents.Add(logAction);
            Debug.LogWarning($"[GameAnalyticsManager] {methodName} queued: Firebase not initialized");
            return;
        }

        if (!IsNetworkAvailable())
        {
            pendingEvents.Add(logAction);
            Debug.Log($"[GameAnalyticsManager] {methodName} queued: No network");
            return;
        }

        logAction?.Invoke();
    }

    public void LogGameStart()
    {
        LogEvent("LogGameStart", () =>
        {
            FirebaseAnalytics.LogEvent("game_start", new Parameter("game_name", GAME_NAME));
            Debug.Log("[GameAnalyticsManager] Event logged: game_start");
        });
    }

    public void LogLevelStart(string levelName)
    {
        LogEvent("LogLevelStart", () =>
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart, new Parameter("level_name", levelName));
            Debug.Log($"[GameAnalyticsManager] Event logged: level_start | Level: {levelName}");
        });
    }

    public void LogLevelComplete(string levelName, int starRating)
    {
        LogEvent("LogLevelComplete", () =>
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelEnd,
                new Parameter("level_name", levelName),
                new Parameter("star_rating", starRating),
                new Parameter("success", 1));
            Debug.Log($"[GameAnalyticsManager] Event logged: level_complete | Level: {levelName} | Star Rating: {starRating}");
        });
    }

    public void LogAdImpression(string adType)
    {
        LogEvent("LogAdImpression", () =>
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventAdImpression, new Parameter("ad_type", adType));
            Debug.Log($"[GameAnalyticsManager] Event logged: ad_impression | Type: {adType}");
        });
    }

    public void LogRewardedAdCompleted(string rewardType)
    {
        LogEvent("LogRewardedAdCompleted", () =>
        {
            FirebaseAnalytics.LogEvent("rewarded_ad_completed", new Parameter("reward_type", rewardType));
            Debug.Log($"[GameAnalyticsManager] Event logged: rewarded_ad_completed | Reward: {rewardType}");
        });
    }

    public void LogSessionDuration(string seconds)
    {
        LogEvent("LogSessionDuration", () =>
        {
            FirebaseAnalytics.LogEvent("session_duration", new Parameter("duration_seconds", seconds));
            Debug.Log($"[GameAnalyticsManager] Event logged: session_duration | {seconds}s");
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