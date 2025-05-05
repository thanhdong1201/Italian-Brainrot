using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetupQuizz_ImageToText : SetupQuizzBase
{
    public override QuizzType Type => QuizzType.ImageToText;

    [SerializeField] private Image questionImage;

    public override void SetupNewQuizz(Quizz selectedQuizz)
    {
        base.SetupNewQuizz(selectedQuizz);
        questionImage.sprite = selectedQuizz.QuestionSprite;
    }

}
