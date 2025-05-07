using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuizzList", menuName = "ScriptableObjects/QuizzListSO")]
public class ListQuizzSO : ScriptableObject
{
    [SerializeField] private QuizzSO easyQuizz;
    [SerializeField] private QuizzSO mediumQuizz;
    [SerializeField] private QuizzSO hardQuizz;

    [SerializeField] private QuizzSO currentQuizzSO;
    public QuizzSO GetQuizzData()
    {
        return currentQuizzSO;
    }

    public void SetQuizz(DifficultyLevel difficultyLevel)
    {
        switch (difficultyLevel)
        {
            case DifficultyLevel.Easy:
                currentQuizzSO = easyQuizz;
                break;
            case DifficultyLevel.Medium:
                currentQuizzSO = mediumQuizz;
                break;
            case DifficultyLevel.Hard:
                currentQuizzSO = hardQuizz;
                break;
            default:
                currentQuizzSO = null;
                break;
        }
    }
}
public enum DifficultyLevel
{
    Easy,
    Medium,
    Hard
}
