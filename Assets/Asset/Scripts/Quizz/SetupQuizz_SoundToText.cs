using UnityEngine;
using UnityEngine.UI;

public class SetupQuizz_SoundToText : SetupQuizzBase
{
    public override QuizzType Type => QuizzType.SoundToText;

    [Header("Others")]
    [SerializeField] private Button playSoundBtn;

    public override void InitializeContent()
    {
        base.InitializeContent();
        playSoundBtn.onClick.AddListener(PlayQuestSound);
    }

    private void PlayQuestSound()
    {
        SoundManager.Instance.PlayMusic(SelectedQuizz.QuestionAudioClip);
    }
}
