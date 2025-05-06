using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameComplete : MonoBehaviour
{
    [SerializeField] private QuizzManager quizzManager;
    [SerializeField] private List<GameObject> stars;
    [Header("Characters")]
    [SerializeField] private List<CharacterConversation> characterConversations;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI congratulationText;
    [SerializeField] private TextMeshProUGUI totalAnswerText;
    [SerializeField] private TextMeshProUGUI characterConversationText;

    [Header("Buttons")]
    [SerializeField] private Button relayBtn;
    [SerializeField] private Button nextLevelBtn;
    [SerializeField] private Button menuBtn;

    private void OnEnable()
    {
        ShowSummaryData();
    }
    private void Start()
    {
        InitializeButtons();
    }
    private void InitializeButtons()
    {
        relayBtn.onClick.AddListener(() => GameManager.Instance.LoadSceneManager.Replay());
        menuBtn.onClick.AddListener(() => GameManager.Instance.LoadSceneManager.ReturnMainMenu());
    }
    private void ShowCharacter(GameObject go)
    {
        for(int i = 0; i < characterConversations.Count; i++)
        {
            characterConversations[i].character.SetActive(characterConversations[i].character == go);
        }
    }
    private void ShowStar(int starCount)
    {
        for (int i = 0; i < stars.Count; i++)
        {
            stars[i].SetActive(i < starCount);
        }
    }
    private void ShowSummaryData()
    {
        StarRating starRating = quizzManager.CalculateStarRating();
        int totalAnswer = quizzManager.GetTotalCorrectAnswers();
        int maxQuizz = quizzManager.GetMaxQuizz();
        Debug.Log($"Star Rating: {starRating}");
        PrepareSummaryData(starRating, totalAnswer, maxQuizz);
    }
    private void PrepareSummaryData(StarRating starRating, int totalAnswer, int maxQuizz)
    {
        switch (starRating)
        {
            case StarRating.ZeroStars:
                congratulationText.text = "Better luck next time!";
                characterConversationText.text = characterConversations[0].text;
                ShowCharacter(characterConversations[0].character);
                ShowStar(0);
                break;
            case StarRating.OneStar:
                congratulationText.text = "You can do better!";
                characterConversationText.text = characterConversations[1].text;
                ShowCharacter(characterConversations[1].character);                  
                ShowStar(1);
                break;
            case StarRating.TwoStars:
                congratulationText.text = "Good job!";
                characterConversationText.text = characterConversations[2].text;
                ShowCharacter(characterConversations[2].character);
                ShowStar(2);
                break;
            case StarRating.ThreeStars:
                congratulationText.text = "Excellent!";
                characterConversationText.text = characterConversations[3].text;
                ShowCharacter(characterConversations[3].character);
                ShowStar(3);
                break;
        }

        totalAnswerText.text = $"Correct Answers: {totalAnswer / maxQuizz}";
        totalAnswerText.gameObject.SetActive(true);
    }
}
[Serializable]
public class CharacterConversation
{
    public GameObject character;
    public string text;
}

