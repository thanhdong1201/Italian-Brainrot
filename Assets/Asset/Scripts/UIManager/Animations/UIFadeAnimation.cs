using DG.Tweening;
using UnityEngine;

public class UIFadeAnimation : MonoBehaviour, IUIAnimation
{
    [SerializeField] private bool playOnEnable = true;
    [SerializeField] private bool closeAfterOpen = false;
    [Header("FadeOut")]
    [SerializeField] private float delayFadeOut = 0f;
    [SerializeField] private float durationFadeOut = 0.6f;
    [SerializeField] private Ease easeFadeOut = Ease.InSine;

    [Header("FadeIn")]
    [SerializeField] private float delayFadeIn = 0f;
    [SerializeField] private float durationFadeIn = 0.6f;
    [SerializeField] private Ease easeFadeIn = Ease.OutSine;

    private CanvasGroup canvasGroup;
    private Tween tween;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    private void OnEnable()
    {
        if (playOnEnable) PlayOpenAnimation();
    }
    private void OnDisable()
    {
        StopAnimation();
    }
    private void OnDestroy()
    {
        StopAnimation();
    }
    public void PlayOpenAnimation()
    {
        //PlayFadeOutAnimation
        canvasGroup.alpha = 0;
        tween = canvasGroup.DOFade(1, durationFadeOut).SetEase(easeFadeOut).SetDelay(delayFadeOut).SetUpdate(UpdateType.Normal, true).OnComplete(() => PlayCloseAnimation());
    }
    public void PlayCloseAnimation()
    {
        //PlayFadeInAnimation
        if (!closeAfterOpen) return;
        canvasGroup.alpha = 1;
        tween = canvasGroup.DOFade(0, durationFadeIn).SetEase(easeFadeIn).SetDelay(delayFadeIn).SetUpdate(UpdateType.Normal, true).OnComplete(() => gameObject.SetActive(false));
    }
    private void StopAnimation()
    {
        if (tween != null)
        {
            tween.Kill();
            tween = null;
        }
    }
}
