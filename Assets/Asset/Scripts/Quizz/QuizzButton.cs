using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class QuizzButton : MonoBehaviour
{
    public QuizzAnswer QuizzAnswer { get; private set; }

    [Header("UI")]
    [SerializeField] private Image answerImage;
    [SerializeField] private Image selectImage;
    [SerializeField] private Image correctImage;
    [SerializeField] private Image wrongImage;

    [SerializeField] private TextMeshProUGUI answerText;

    private SetupQuizzBase setupQuizzBase;

    [Header("Animation")]
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
    #region UI
    public void AddSetupQuizzBase(SetupQuizzBase setupQuizzBase)
    {
        this.setupQuizzBase = setupQuizzBase;
    }
    public void SetUp(QuizzAnswer quizzAnswer, QuizzManager quizzManager)
    {
        QuizzAnswer = quizzAnswer;

        if (setupQuizzBase.SelectedQuizz.Type == QuizzType.TextToImage)
        {
            answerImage.sprite = quizzAnswer.Sprite;
        }
        if (setupQuizzBase.SelectedQuizz.Type == QuizzType.ImageToText || setupQuizzBase.SelectedQuizz.Type == QuizzType.SoundToText)
        {
            answerText.text = quizzAnswer.Text;
        }

        selectImage.gameObject.SetActive(false);
        correctImage.gameObject.SetActive(false);
        wrongImage.gameObject.SetActive(false);
        setupQuizzBase.UpdateQuizzButton(null);
        PlayAnimation(false);
    }
    public void SetSelectImage(bool isSelected)
    {
        selectImage.gameObject.SetActive(isSelected);
        PlayAnimation(isSelected);
    }
    public void SelectThisImage()
    {
        setupQuizzBase.UpdateQuizzButton(this);
    }
    public void ShowAnswer(bool isCorrect)
    {
        correctImage.gameObject.SetActive(isCorrect);
        wrongImage.gameObject.SetActive(!isCorrect);
    }
    #endregion
    #region Animations
    private void PlayAnimation(bool isSelected)
    {
        StopAnimation();
        if (isSelected)
        {
            tween = transform.DOScale(scaleFactor * localScale, duration).SetUpdate(UpdateType.Normal, true);
        }
        else
        {
            tween = transform.DOScale(localScale, duration).SetUpdate(UpdateType.Normal, true);
        }
    }
    private void StopAnimation()
    {
        if (tween != null)
        {
            tween.Kill(true);
            tween = null;
        }
    }
    #endregion
}
