using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizzManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI quizzNumberText;
    [SerializeField] private Button continueBtn;
    [SerializeField] private Image finalAnswerImage;
    [SerializeField] private List<string> corrects = new List<string>();
    [SerializeField] private List<string> wrongs = new List<string>();

    [SerializeField] private SetupQuizz_TextToImage setupQuizz_TextToImage;
    [SerializeField] private SetupQuizz_ImageToText setupQuizz_ImageToText;
    [SerializeField] private SetupQuizz_SoundToText setupQuizz_SoundToText;

    [Header("Image Quizz")]
    [SerializeField] private ListQuizzSO listQuizzSO;

    [Header("Listening to Events")]
    [SerializeField] private VoidEventChannelSO onCountDownCompleted;

    [Header("Broadcasting Events")]
    [SerializeField] private VoidEventChannelSO onStartGame;
    [SerializeField] private VoidEventChannelSO onRestartCoutDown;
    [SerializeField] private VoidEventChannelSO onRetry, onIgnoreRety;

    private Dictionary<QuizzType, SetupQuizzBase> setupQuizzBasesDictionary = new Dictionary<QuizzType, SetupQuizzBase>();
    private QuizzSO quizzSOInstance;
    private QuizzButton selectedQuizzButton;
    private SetupQuizzBase setupQuizzBase;
    private int quizzNumber = 0;

    public Quizz SelectedQuizz { get; private set; }

    private void OnEnable()
    {
        onCountDownCompleted.OnEventRaised += CheckQuizz;
        onStartGame.OnEventRaised += ActiveNewQuizz;
        onRetry.OnEventRaised += OnRetry;
        onIgnoreRety.OnEventRaised += OnIgnoreRetry;
    }
    private void OnDisable()
    {
        onCountDownCompleted.OnEventRaised -= CheckQuizz;
        onStartGame.OnEventRaised -= ActiveNewQuizz;
        onRetry.OnEventRaised -= OnRetry;
        onIgnoreRety.OnEventRaised -= OnIgnoreRetry;
    }
    private void Awake()
    {
        quizzSOInstance = Instantiate(listQuizzSO.SelectedQuizz);
        continueBtn.onClick.AddListener(() => ActiveNewQuizz());

        InitializeQuizzUI();
    }
    private void Start()
    {
        if (AnalyticsManager.Instance != null)
        {
            AnalyticsManager.Instance.WaitForInitialization(() => AnalyticsManager.Instance.LogLevelStart("level_quizz"));
        }
    }
    private void InitializeQuizzUI()
    {
        setupQuizzBasesDictionary.Add(QuizzType.TextToImage, setupQuizz_TextToImage);
        setupQuizzBasesDictionary.Add(QuizzType.ImageToText, setupQuizz_ImageToText);
        setupQuizzBasesDictionary.Add(QuizzType.SoundToText, setupQuizz_SoundToText);

        foreach (var setupQuizz in setupQuizzBasesDictionary)
        {
            setupQuizz.Value.gameObject.SetActive(true);
            setupQuizz.Value.InitializeContent();
            setupQuizz.Value.gameObject.SetActive(false);
        }

        quizzNumberText.text = $"Question {1}/{quizzSOInstance.QuizzesPerGame}";
    }
    private void ShowQuizzUI(QuizzType quizzType)
    {
        foreach (var setupQuizz in setupQuizzBasesDictionary)
        {
            setupQuizz.Value.gameObject.SetActive(setupQuizz.Key == quizzType);
            if (setupQuizz.Key == quizzType)
            {
                setupQuizzBase = setupQuizz.Value;
                setupQuizz.Value.SetupNewQuizz(SelectedQuizz);
            }
        }

    }
    private void ResetQuizz()
    {
        SelectedQuizz = null;
        selectedQuizzButton = null;
        onRestartCoutDown?.RaiseEvent();
        SoundManager.Instance.StopMusic();
    }
    public void ActiveNewQuizz()
    {
        if (quizzSOInstance.IsGameCompleted)
        {
            CompleteQuizzes();
            return;
        }

        continueBtn.interactable = false;
        ResetQuizz();
        GetRandomQuizz(true);
        GetRandomQuizzAnswer();
    }
    public void GetRandomQuizz(bool increaseQuizzNumber)
    {
        int randomQuizzIndex = Random.Range(0, quizzSOInstance.Quizzes.Count);
        SelectedQuizz = quizzSOInstance.Quizzes[randomQuizzIndex];
        quizzSOInstance.Quizzes.RemoveAt(randomQuizzIndex);
        ShowQuizzUI(SelectedQuizz.Type);

        if (increaseQuizzNumber)
        {
            quizzNumber++;
            quizzNumberText.text = $"Question {quizzNumber}/{quizzSOInstance.QuizzesPerGame}";
        }
    }
    public void GetRandomQuizzAnswer()
    {
        if (SelectedQuizz == null) return;

        List<QuizzAnswer> randomizedAnswers = new List<QuizzAnswer>(SelectedQuizz.QuizzAnswers);
        for (int i = 0; i < 4; i++)
        {
            int randomIndex = Random.Range(0, randomizedAnswers.Count);
            setupQuizzBase.QuizzButtons[i].SetUp(randomizedAnswers[randomIndex], this);
            randomizedAnswers.RemoveAt(randomIndex);
        }
    }
    private void CheckQuizz()
    {
        selectedQuizzButton = setupQuizzBase.SelectedQuizzButton;
        if (selectedQuizzButton == null || SelectedQuizz == null)
        {
            FailQuizz();
            return;
        }

        if (selectedQuizzButton.QuizzAnswer == SelectedQuizz.GetCorrectAnswer())
        {
            CorrectQuizz();
            selectedQuizzButton.ShowAnswer(selectedQuizzButton.QuizzAnswer.Answer);
        }
        else
        {
            FailQuizz();
            selectedQuizzButton.ShowAnswer(selectedQuizzButton.QuizzAnswer.Answer);
        }
    }

    private void CorrectQuizz()
    {
        setupQuizzBase.SetAnswerText(corrects[Random.Range(0, corrects.Count)]);
        SoundManager.Instance.PlayCorrectSound();
        SoundManager.Instance.PlayMusic(SelectedQuizz.CorrectAudioClip);
        quizzSOInstance.Quizzes.Remove(SelectedQuizz);
        quizzSOInstance.OnCorrectAnswer();
        quizzSOInstance.OnQuizzCompleted();

        if (SelectedQuizz.Type == QuizzType.TextToImage)
        {
            finalAnswerImage.sprite = SelectedQuizz.GetCorrectAnswer().Sprite;
        }
        if (SelectedQuizz.Type == QuizzType.ImageToText)
        {
            finalAnswerImage.sprite = SelectedQuizz.GetCorrectAnswer().Sprite;
        }
        if (SelectedQuizz.Type == QuizzType.SoundToText)
        {
            finalAnswerImage.sprite = SelectedQuizz.QuestionSprite;
        }

        continueBtn.gameObject.SetActive(true);
        continueBtn.interactable = true;
    }
    private void FailQuizz()
    {
        SoundManager.Instance.StopMusic();
        //quizzSOInstance.OnQuizzCompleted();
        setupQuizzBase.SetAnswerText(wrongs[Random.Range(0, wrongs.Count)]);
        SoundManager.Instance.PlayIncorrectSound();
        Invoke(nameof(ShowRetryUI), 2f);
    }
    private void ShowRetryUI()
    {
        SoundManager.Instance.StopMusic();
        UIManager.Instance.ShowPanel(UIPanel.Retry);
        SoundManager.Instance.PlayGameOverSound();
    }
    private void CompleteQuizzes()
    {
        SoundManager.Instance.StopMusic();
        UIManager.Instance.ShowPanel(UIPanel.Complete);
        AnalyticsManager.Instance.LogLevelComplete("level_quizz", CalculateStarRating().ToString());
    }
    private void OnRetry()
    {
        SoundManager.Instance.StopMusic();
        Invoke(nameof(RetryQuizz), 1f);
    }
    private void RetryQuizz()
    {
        continueBtn.interactable = false;
        ResetQuizz();
        GetRandomQuizz(false);
        GetRandomQuizzAnswer();
    }
    private void OnIgnoreRetry()
    {
        CompleteQuizzes();
    }
    public StarRating CalculateStarRating()
    {
        int correctAnswers = quizzSOInstance.TotalCorrectAnswers;
        int maxQuizz = quizzSOInstance.QuizzesPerGame;

        if (maxQuizz <= 0)
            return StarRating.ZeroStars;

        float ratio = (float)correctAnswers / maxQuizz;

        if (ratio >= 1f)  // 100%
        {
            SoundManager.Instance.PlayCompleteGameSound();
            return StarRating.ThreeStars;
        }
        else if (ratio >= 0.666f)  // >= 10/15
        {
            SoundManager.Instance.PlayCompleteGameSound();
            return StarRating.TwoStars;
        }
        else if (ratio >= 0.333f)  // >= 5/15
        {

            return StarRating.OneStar;
        }
        else
        {

            return StarRating.ZeroStars;
        }
    }

    public int GetTotalCorrectAnswers() => quizzSOInstance.TotalCorrectAnswers;
    public int GetMaxQuizz() => quizzSOInstance.QuizzesPerGame;
}
public enum StarRating
{
    ZeroStars = 0,
    OneStar = 1,
    TwoStars = 2,
    ThreeStars = 3
}