using UnityEngine;
using DG.Tweening;

public class UIScaleAnimation : MonoBehaviour, IUIAnimation
{
    [SerializeField] private bool playOnEnable = true;
    [SerializeField] private bool closeAfterOpen = false;

    [SerializeField] private float delay = 0f;
    [SerializeField] private float duration = 0.6f;
    [SerializeField] private Ease easeOpen = Ease.OutBack;
    [SerializeField] private Ease easeClose = Ease.InBack;

    private RectTransform rectTransform;
    private Tween tween;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
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
        rectTransform.localScale = Vector3.zero;
        tween = rectTransform.DOScale(Vector3.one, duration).SetEase(easeOpen).SetDelay(delay).SetUpdate(UpdateType.Normal, true).OnComplete(()=> PlayCloseAnimationAfterOpen());
    }
    public void PlayCloseAnimation()
    {
        rectTransform.localScale = Vector3.one;
        tween = rectTransform.DOScale(Vector3.zero, duration).SetEase(easeClose).SetUpdate(UpdateType.Normal, true).OnComplete(()=> gameObject.SetActive(false));
    }
    private void PlayCloseAnimationAfterOpen()
    {
        if (!closeAfterOpen) return;
        tween = rectTransform.DOScale(Vector3.zero, duration).SetEase(easeClose).SetDelay(2f).SetUpdate(UpdateType.Normal, true).OnComplete(() => gameObject.SetActive(false));
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