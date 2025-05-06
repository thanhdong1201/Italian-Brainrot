using DG.Tweening;
using UnityEngine;

public class UIBounceAnimation : MonoBehaviour
{
    public enum BounceDirection { FromLeft, FromRight, FromTop, FromBottom }

    [SerializeField] private bool playOnAwake = true;
    [Header("General")]
    [SerializeField] private float duration = 0.6f;
    [SerializeField] private float delay = 0f;

    [Header("Bounce")]
    [SerializeField] private BounceDirection bounceDirection = BounceDirection.FromTop;
    [SerializeField] private Ease easeBounce = Ease.OutBounce;

    private RectTransform rectTransform;
    private Tween tween;
    private Vector2 originalPos;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPos = rectTransform.anchoredPosition;
    }
    private void OnEnable()
    {
        if(playOnAwake) PlayAnimation();
    }
    private void OnDisable()
    {
        StopAnimation();
    }
    private void OnDestroy()
    {
        StopAnimation();
    }
    public void PlayAnimation()
    {
        StopAnimation();
        Vector2 startPos = originalPos;
        switch (bounceDirection)
        {
            case BounceDirection.FromLeft:
                startPos = originalPos + new Vector2(-Screen.width, 0);
                break;
            case BounceDirection.FromRight:
                startPos = originalPos + new Vector2(Screen.width, 0);
                break;
            case BounceDirection.FromTop:
                startPos = originalPos + new Vector2(0, Screen.height);
                break;
            case BounceDirection.FromBottom:
                startPos = originalPos + new Vector2(0, -Screen.height);
                break;
        }
        rectTransform.anchoredPosition = startPos;
        tween = rectTransform.DOAnchorPos(originalPos, duration)
            .SetEase(easeBounce)
            .SetDelay(delay)
            .SetUpdate(UpdateType.Normal, true);
    }
    public void PlayHideAnimatiom()
    {
        StopAnimation();
        tween = rectTransform.DOScale(Vector3.zero, duration / 3f)
            .SetEase(Ease.InBack)
            .SetDelay(delay)
            .SetUpdate(UpdateType.Normal, true)
            .OnComplete(() => gameObject.SetActive(false));
    }
    public void StopAnimation()
    {
        if (tween != null)
        {
            tween.Kill();
            tween = null;
        }
        rectTransform.anchoredPosition = originalPos;
    }
}
