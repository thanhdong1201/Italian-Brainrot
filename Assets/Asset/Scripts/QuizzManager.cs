using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizzManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button continueBtn;
    [SerializeField] private Image finalAnswerImage;
    [SerializeField] private List<string> corrects = new List<string>();
    [SerializeField] private List<string> wrongs = new List<string>();

    [SerializeField] private SetupQuizz_TextToImage setupQuizz_TextToImage;
    [SerializeField] private SetupQuizz_ImageToText setupQuizz_ImageToText;
    [SerializeField] private SetupQuizz_SoundToText setupQuizz_SoundToText;
    [SerializeField] private SetupQuizzBase setupQuizzBase;

    [Header("Image Quizz")]
    [SerializeField] private QuizzSO quizzSO;

    [Header("Events")]
    [SerializeField] private VoidEventChannelSO onCountDownCompleted;
    [SerializeField] private VoidEventChannelSO onRestartCoutDown;

    private Dictionary<QuizzType, SetupQuizzBase> setupQuizzBasesDictionary = new Dictionary<QuizzType, SetupQuizzBase>();
    private QuizzSO quizzSOInstance;
    private QuizzButton selectedQuizzButton;

    public Quizz SelectedQuizz { get; private set; }

    private void OnEnable()
    {
        onCountDownCompleted.OnEventRaised += CheckQuizz;
    }
    private void OnDisable()
    {
        onCountDownCompleted.OnEventRaised -= CheckQuizz;
    }
    private void Awake()
    {
        quizzSOInstance = Instantiate(quizzSO);
        continueBtn.onClick.AddListener(() => ActiveNewQuizz());

        InitializeQuizzUI();
    }
    private void Start()
    {
        ActiveNewQuizz();
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
        if (quizzSOInstance.Quizzes.Count == 0) return;

        continueBtn.interactable = false;
        ResetQuizz();
        GetRandomQuizz();
        GetRandomQuizzAnswer();
    }
    public void GetRandomQuizz()
    {
        if (quizzSOInstance.Quizzes.Count == 0) return;

        int randomQuizzIndex = Random.Range(0, quizzSOInstance.Quizzes.Count);
        SelectedQuizz = quizzSOInstance.Quizzes[randomQuizzIndex];
        quizzSOInstance.Quizzes.RemoveAt(randomQuizzIndex);
        ShowQuizzUI(SelectedQuizz.Type);
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
        }
        else
        {
            FailQuizz();
        }

        selectedQuizzButton.ShowAnswer(selectedQuizzButton.QuizzAnswer.Answer);
    }

    private void CorrectQuizz()
    {
        setupQuizzBase.SetAnswerText(corrects[Random.Range(0, corrects.Count)]);
        SoundManager.Instance.PlayCorrectSound();
        SoundManager.Instance.PlayMusic(SelectedQuizz.CorrectAudioClip);
        quizzSOInstance.Quizzes.Remove(SelectedQuizz);

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
        //finalAnswerImage.sprite = SelectedQuizz.GetCorrectAnswer().Sprite;
        continueBtn.gameObject.SetActive(true);
        continueBtn.interactable = true;
    }
    private void FailQuizz()
    {
        setupQuizzBase.SetAnswerText(wrongs[Random.Range(0, wrongs.Count)]);
        SoundManager.Instance.PlayIncorrectSound();
        Invoke(nameof(ActiveNewQuizz), 3f);
    }
    private void CompleteQuizzes()
    {

    }
}