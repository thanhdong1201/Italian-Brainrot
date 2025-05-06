using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ButtonEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float scaleFactor = 0.8f;
    [SerializeField] private float duration = 0.2f; 

    private Tween tween;
    private Vector3 localScale;
    private void Awake()
    {
        localScale = transform.localScale;
    }
    private void OnDisable()
    {
        StopAnimation();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        StopAnimation();
        tween = transform.DOScale(scaleFactor * localScale, duration).SetUpdate(UpdateType.Normal, true);
    }
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        StopAnimation();
        tween = transform.DOScale(localScale, duration).SetUpdate(UpdateType.Normal, true);
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
