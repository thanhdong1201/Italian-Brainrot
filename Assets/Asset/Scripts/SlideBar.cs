using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SliderBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private float duration = 15f;
    [SerializeField] private Ease easeType = Ease.Linear;

    [SerializeField] private VoidEventChannelSO onCountDownCompleted;
    [SerializeField] private VoidEventChannelSO onRestartCoutDown;

    private Tween tween;

    private void OnEnable()
    {
        onRestartCoutDown.OnEventRaised += WaitToRestart;
    }
    private void OnDisable()
    {
        onRestartCoutDown.OnEventRaised -= WaitToRestart;
    }

    private void WaitToRestart()
    {
        fillImage.fillAmount = 1f;
        Invoke(nameof(StartAnimation), 1f);
    }
    [Button]
    private void StartAnimation()
    {
        StopAnimation();

        fillImage.fillAmount = 1f;

        tween = fillImage.DOFillAmount(0f, duration)
            .SetEase(easeType)
            .OnComplete(() => OnAnimationCompleted());
    }
    private void OnAnimationCompleted()
    {
        fillImage.fillAmount = 0f;
        onCountDownCompleted?.RaiseEvent();
    }
    private void StopAnimation()
    {
        if (tween != null)
        {
            tween.Kill(true);
            tween = null;
        }
    }
    private void OnDestroy()
    {
        StopAnimation();
        onRestartCoutDown.OnEventRaised -= WaitToRestart;
    }
}