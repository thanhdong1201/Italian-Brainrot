using UnityEngine;

[CreateAssetMenu(fileName = "Quizz", menuName = "ScriptableObjects/ListQuizzSO")]
public class ListQuizzSO : ScriptableObject
{
    [SerializeField] private QuizzSO EasyQuizz;
    [SerializeField] private QuizzSO MediumQuizz;
    [SerializeField] private QuizzSO HardQuizz;

    public QuizzSO SelectedQuizz;

    public void SetQuizz(QuizzMode mode)
    {
        switch (mode)
        {
            case QuizzMode.Easy:
                SelectedQuizz = EasyQuizz;
                break;
            case QuizzMode.Medium:
                SelectedQuizz = MediumQuizz;
                break;
            case QuizzMode.Hard:
                SelectedQuizz = HardQuizz;
                break;
        }
    }
}
public enum QuizzMode
{
    Easy,
    Medium,
    Hard
}
