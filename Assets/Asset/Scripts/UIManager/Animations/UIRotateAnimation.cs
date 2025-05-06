using DG.Tweening;
using UnityEngine;

public class UIRotateAnimation : MonoBehaviour
{
    public enum RotationAxis { X, Y, Z }

    [SerializeField] private bool playOnAwake = true;
    [Header("General")]
    [SerializeField] private float duration = 0.6f;
    [SerializeField] private float delay = 0f;

    [Header("Rotate")]
    [SerializeField] private RotationAxis rotationAxis = RotationAxis.Z;
    [SerializeField] private Ease easeRotate = Ease.Linear;
    [SerializeField] private int rotationLoops = 3;

    private RectTransform rectTransform;
    private Tween tween;
    private Vector3 originalRotation;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalRotation = rectTransform.localEulerAngles;
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
        Vector3 rotationVector = Vector3.zero;
        switch (rotationAxis)
        {
            case RotationAxis.X:
                rotationVector = originalRotation + new Vector3(360, 0, 0);
                break;
            case RotationAxis.Y:
                rotationVector = originalRotation + new Vector3(0, 360, 0);
                break;
            case RotationAxis.Z:
                rotationVector = originalRotation + new Vector3(0, 0, 360);
                break;
        }
        rectTransform.rotation = Quaternion.Euler(originalRotation);
        tween = rectTransform.DORotate(rotationVector, duration / rotationLoops, RotateMode.FastBeyond360)
            .SetEase(easeRotate)
            .SetDelay(delay)
            .SetLoops(rotationLoops, LoopType.Incremental)
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
        rectTransform.localScale = Vector3.one;
        rectTransform.rotation = Quaternion.Euler(originalRotation);
    }
}
