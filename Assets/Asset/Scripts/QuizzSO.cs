using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "QuizzList", menuName = "ScriptableObjects/QuizzSO")]
public class QuizzSO : ScriptableObject
{
    public List<Quizz> Quizzes = new List<Quizz>();
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
