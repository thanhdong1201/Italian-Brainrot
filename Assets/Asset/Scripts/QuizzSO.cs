using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Quizz", menuName = "ScriptableObjects/QuizzSO")]
public class QuizzSO : ScriptableObject
{
    public List<Quizz> Quizzes = new List<Quizz>();

    [SerializeField] private int totalQuizzPerGame = 10;
    public int TotalQuizzPerGame => totalQuizzPerGame;
    private int totalCorrectAnswers = 0;
    public int TotalCorrectAnswers => totalCorrectAnswers;
    public int QuizzCount { get; private set; } = 0;
    private bool isGameCompleted = false;
    public bool IsGameCompleted() => isGameCompleted;

    public void OnCorrectAnswer()
    {
        if (isGameCompleted) return;

        totalCorrectAnswers++;
    }
    public void OnQuizzCompleted()
    {
        if (isGameCompleted) return;

        QuizzCount++;

        if (QuizzCount >= totalQuizzPerGame)
        {
            isGameCompleted = true;
        }
    }


}

[Serializable]
public class Quizz 
{
    public string Question;
    public QuizzType Type;
    public AudioClip CorrectAudioClip;

    public Sprite QuestionSprite;
    public AudioClip QuestionAudioClip;

    public List<QuizzAnswer> QuizzAnswers;

    public QuizzAnswer GetCorrectAnswer()
    {
        var correctAnswer = QuizzAnswers.First(answer => answer != null && answer.Answer);
        return correctAnswer;
    }
}
[Serializable]
public class QuizzAnswer
{
    public bool Answer;
    public Sprite Sprite;
    public string Text;
}

public enum QuizzType { TextToImage, SoundToText, ImageToText }
