using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class SetupQuizzBase : MonoBehaviour
{
    public abstract QuizzType Type { get; }
    [Header("UI")]
    [SerializeField] protected Transform contentHolder;
    [SerializeField] protected TextMeshProUGUI questionText;
    [SerializeField] protected TextMeshProUGUI answerText;

    [SerializeField] protected QuizzManager quizzManager;
    public List<QuizzButton> QuizzButtons { get; private set; } = new List<QuizzButton>();
    public QuizzButton SelectedQuizzButton { get; private set; }
    public Quizz SelectedQuizz { get; private set; }

    public virtual void InitializeContent()
    {
        foreach (Transform child in contentHolder)
        {
            QuizzButton quizzButton = child.GetComponent<QuizzButton>();
            QuizzButtons?.Add(quizzButton);
            quizzButton.AddSetupQuizzBase(this);
        }

    }
    public virtual void SetupNewQuizz(Quizz selectedQuizz)
    {
        SelectedQuizzButton = null;
        SelectedQuizz = null;
        answerText.text = "";

        questionText.text = selectedQuizz.Question;
        SelectedQuizz = quizzManager.SelectedQuizz;
    }

    public void UpdateQuizzButton(QuizzButton quizzButton)
    {
        foreach (QuizzButton btn in QuizzButtons)
        {
            btn.SetSelectImage(btn == quizzButton);
            if (btn == quizzButton)
            {
                SelectedQuizzButton = btn;
            }
        }
    }
    public void SetAnswerText(string text)
    {
        answerText.text = text;
    }
}
