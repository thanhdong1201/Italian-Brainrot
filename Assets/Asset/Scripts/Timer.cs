using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public Action<float> OnTimeChanged;
    public Action OnTimeUp;

    private float remainingTime;
    private const float INITIAL_TIME = 15f; // Thời gian đếm ngược ban đầu  
    private bool isCountingDown = false;

    private void Start()
    {
        StartCountdown();
    }
    private void Update()
    {
        if (isCountingDown)
        {
            remainingTime -= Time.deltaTime;
            remainingTime = Mathf.Max(remainingTime, 0f); // Không để thời gian âm  

            OnTimeChanged?.Invoke(remainingTime);

            if (remainingTime <= 0f)
            {
                isCountingDown = false;
                OnTimeUp?.Invoke();
            }
        }
    }

    public void StartCountdown()
    {
        remainingTime = INITIAL_TIME;
        isCountingDown = true;
    }

    public void ResetTimer()
    {
        remainingTime = INITIAL_TIME;
        isCountingDown = false;
        OnTimeChanged?.Invoke(remainingTime);
    }

    public string GetTime()
    {
        int minutes = (int)(remainingTime / 60f);
        int seconds = (int)(remainingTime % 60f);
        return $"{minutes:00}:{seconds:00}";
    }

    public bool IsTimeUp()
    {
        return remainingTime <= 0f;
    }
}
