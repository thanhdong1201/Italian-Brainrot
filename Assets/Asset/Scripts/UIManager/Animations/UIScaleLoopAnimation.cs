using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScaleLoopAnimation : MonoBehaviour
{
    [SerializeField] private float multiplier = 1.2f;
    [SerializeField] private float duration = 0.6f;

    private RectTransform rectTransform;
    private Tween tween;
    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
        rectTransform = GetComponent<RectTransform>();
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
        tween = rectTransform.DOScale(originalScale * multiplier, duration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine).SetUpdate(UpdateType.Normal, true);
    }
    private void StopAnimation()
    {
        tween?.Kill(true);
        tween = null;
    }
}
