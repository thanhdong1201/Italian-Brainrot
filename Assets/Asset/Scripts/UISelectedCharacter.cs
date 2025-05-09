using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISelectedCharacter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private Image characterImage;
    [SerializeField] private Button playBtn;
    [SerializeField] private Button pauseBtn;
    [SerializeField] private Button backBtn;
    private CharacterSO characterSO;
    private void OnDisable()
    {
        playBtn.onClick.RemoveListener(() => PlayMusic());
        pauseBtn.onClick.RemoveListener(() => PauseMusic());
        backBtn.onClick.RemoveListener(() => StopMusic());
    }
    public void SetSelectedCharacter(CharacterSO characterSO)
    {
        this.characterSO = characterSO;
        characterNameText.text = characterSO.CharacterName;
        characterImage.sprite = characterSO.Sprite;
        playBtn.onClick.AddListener(() => PlayMusic());
        pauseBtn.onClick.AddListener(() => PauseMusic());
        backBtn.onClick.AddListener(() => StopMusic());
    }
    private void PlayMusic()
    {
        SoundManager.Instance.PlayMusic(characterSO.Audio);
        playBtn.gameObject.SetActive(false);
        pauseBtn.gameObject.SetActive(true);
    }
    private void PauseMusic()
    {
        SoundManager.Instance.PauseMusic(true);
        playBtn.gameObject.SetActive(true);
        pauseBtn.gameObject.SetActive(false);
    }
    private void StopMusic()
    {
        SoundManager.Instance.StopMusicNow();
        playBtn.gameObject.SetActive(true);
        pauseBtn.gameObject.SetActive(false);
    }
}
