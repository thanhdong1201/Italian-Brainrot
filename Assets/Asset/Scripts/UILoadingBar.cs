using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UILoadingBar : MonoBehaviour
{
     private float duration = 5f;
    [SerializeField] private Ease easeType = Ease.Linear;
    [SerializeField] private Slider slider;

    private Tween tween;

    private void OnEnable()
    {
        PlayAnimation();
    }
    private void OnDestroy()
    {
        StopAnimation();
    }
    public void SetLoadingDuration(float duration)
    {
        this.duration = duration;
    }
    [Button]
    private void PlayAnimation()
    {
        StopAnimation();

        slider.value = 0f;

        tween = DOTween.To( () => slider.value, x => slider.value = x, 1f, duration)
        .SetEase(easeType)
        .OnComplete(OnAnimationCompleted);
    }
    private void OnAnimationCompleted()
    {
        slider.value = 1f;
        GameManager.Instance.LoadSceneManager.ContinueLoading();
    }
    private void StopAnimation()
    {
        if (tween != null)
        {
            tween.Kill(true);
            tween = null;
        }
    }
}
