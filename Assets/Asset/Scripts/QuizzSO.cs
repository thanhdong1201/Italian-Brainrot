using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Quizz", menuName = "ScriptableObjects/QuizzSO")]
public class QuizzSO : ScriptableObject
{
    public List<Quizz> Quizzes = new();

    [SerializeField] private int quizzesPerGame = 10;
    public int QuizzesPerGame => quizzesPerGame;

    public int TotalCorrectAnswers { get; private set; }
    public int CompletedQuizzCount { get; private set; }
    public bool IsGameCompleted => CompletedQuizzCount >= quizzesPerGame;

    public void OnCorrectAnswer()
    {
        if (!IsGameCompleted) TotalCorrectAnswers++;
    }

    public void OnQuizzCompleted()
    {
        if (!IsGameCompleted) CompletedQuizzCount++;
    }

    public void Reset()
    {
        TotalCorrectAnswers = 0;
        CompletedQuizzCount = 0;
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
