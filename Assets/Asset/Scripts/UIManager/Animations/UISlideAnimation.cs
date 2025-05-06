using UnityEngine;
using DG.Tweening;

public class UISlideAnimation : MonoBehaviour
{
    [SerializeField] private bool playOnAwake = true;
    [Header("General")]
    [SerializeField] private float delay = 0f;
    [SerializeField] private float duration = 0.6f;

    [Header("Slide")]
    [SerializeField] private Ease easeOpen = Ease.OutBack;
    [SerializeField] private Ease easeClose = Ease.InBack;
    [SerializeField] private Vector2 slideOffset;

    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private Tween tween;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
    }

    private void OnEnable()
    {
        if(playOnAwake) PlayOpenAnimation();
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
        rectTransform.anchoredPosition = originalPosition + slideOffset;
        tween = rectTransform.DOAnchorPos(originalPosition, duration).SetEase(easeOpen).SetDelay(delay).SetUpdate(UpdateType.Normal, true);
    }

    public void PlayCloseAnimation()
    {
        tween = rectTransform.DOAnchorPos(originalPosition + slideOffset, duration).SetEase(easeClose).SetDelay(delay).SetUpdate(UpdateType.Normal, true).OnComplete(() => gameObject.SetActive(false));
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
