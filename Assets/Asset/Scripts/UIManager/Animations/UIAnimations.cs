using UnityEngine;
using DG.Tweening;

public class UIAnimations : MonoBehaviour
{
    public enum RotationAxis { X, Y, Z }
    public enum BounceDirection { FromLeft, FromRight, FromTop, FromBottom }

    [Header("General")]
    [SerializeField] private float duration = 0.6f;
    [SerializeField] private float delay = 0f;

    [Header("Scale")]
    [SerializeField] private Ease easeScale = Ease.OutBack;
    [SerializeField] private Vector3 scaleTarget = Vector3.one;

    [Header("Rotate")]
    [SerializeField] private RotationAxis rotationAxis = RotationAxis.Z;
    [SerializeField] private Ease easeRotate = Ease.Linear;
    [SerializeField] private int rotationLoops = 3;

    [Header("Bounce")]
    [SerializeField] private BounceDirection bounceDirection = BounceDirection.FromTop;
    [SerializeField] private Ease easeBounce = Ease.OutBounce;

    private RectTransform rectTransform;
    private Tween scaleTween, rotateTween, bounceTween, hideTween;
    private Vector2 originalPos;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPos = rectTransform.anchoredPosition;
    }
    private void OnEnable()
    {
        PlayAnimation();
    }
    private void OnDisable()
    {
        StopAnimation();
    }
    private void OnDestroy()
    {
        StopAnimation();
    }
    private void PlayAnimation()
    {
        StopAnimation();

        rotationAxis = (RotationAxis)Random.Range(0, System.Enum.GetValues(typeof(RotationAxis)).Length);
        bounceDirection = (BounceDirection)Random.Range(0, System.Enum.GetValues(typeof(BounceDirection)).Length);

        int random = Random.Range(0, 3);
        switch (random)
        {
            case 0:
                PlayScaleAnimation();
                PlayRotateAnimation();
                break;
            case 1:
                PlayScaleAnimation();
                PlayBounceAnimation();
                break;
            case 2:
                PlayRotateAnimation();
                PlayBounceAnimation();
                break;
        }
    }

    private void PlayScaleAnimation()
    {
        rectTransform.localScale = Vector3.zero;
        scaleTween = rectTransform.DOScale(scaleTarget, duration)
            .SetEase(easeScale)
            .SetDelay(delay)
            .SetUpdate(UpdateType.Normal, true);
    }

    private void PlayRotateAnimation()
    {
        Vector3 rotationVector = Vector3.zero;
        switch (rotationAxis)
        {
            case RotationAxis.X:
                rotationVector = new Vector3(360, 0, 0);
                break;
            case RotationAxis.Y:
                rotationVector = new Vector3(0, 360, 0);
                break;
            case RotationAxis.Z:
                rotationVector = new Vector3(0, 0, 360);
                break;
        }
        rectTransform.rotation = Quaternion.identity;
        rotateTween = rectTransform.DORotate(rotationVector, duration / rotationLoops, RotateMode.FastBeyond360)
            .SetEase(easeRotate)
            .SetDelay(delay)
            .SetLoops(rotationLoops, LoopType.Incremental)
            .SetUpdate(UpdateType.Normal, true);
    }

    private void PlayBounceAnimation()
    {
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
        bounceTween = rectTransform.DOAnchorPos(originalPos, duration)
            .SetEase(easeBounce)
            .SetDelay(delay)
            .SetUpdate(UpdateType.Normal, true);
    }
    public void PlayHideAnimatiom()
    {
        StopAnimation();
        hideTween = rectTransform.DOScale(Vector3.zero, duration/3f)
            .SetEase(Ease.InBack)
            .SetDelay(delay)
            .SetUpdate(UpdateType.Normal, true)
            .OnComplete(()=> gameObject.SetActive(false));
    }
    public void StopAnimation()
    {
        if(scaleTween != null)
        {
            scaleTween.Kill();
            scaleTween = null;
        }
        if (rotateTween != null)
        {
            rotateTween.Kill();
            rotateTween = null;
        }
        if (bounceTween != null)
        {
            bounceTween.Kill();
            bounceTween = null;
        }
        if (hideTween != null)
        {
            hideTween.Kill();
            hideTween = null;
        }
        rectTransform.localScale = Vector3.one;
        rectTransform.anchoredPosition = originalPos;
        rectTransform.rotation = Quaternion.identity;
    }
}