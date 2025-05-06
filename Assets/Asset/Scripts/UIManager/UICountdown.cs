using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class UICountdown : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;
    private float countdownTime = 3f;

    private void Start()
    {
        Countdown();
    }
    public void Countdown()
    {
        StartCoroutine(StartCountdown());
    }
    private IEnumerator StartCountdown()
    {
        gameObject.SetActive(true);
        countdownText.text = "Are you ready!!!";
        countdownText.transform.localScale = Vector3.one * 2f;
        countdownText.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(1f);

        for (int i = (int)countdownTime; i > 0; i--)
        {
            countdownText.text = i.ToString();
            countdownText.transform.localScale = Vector3.one * 2f;
            countdownText.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
            yield return new WaitForSeconds(1f);
        }

        countdownText.text = "Go!";
        countdownText.transform.localScale = Vector3.one * 2f;
        countdownText.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(1f);

        countdownText.SetText("");
        gameObject.SetActive(false);
        GameManager.Instance.StartGame();
    }
}
