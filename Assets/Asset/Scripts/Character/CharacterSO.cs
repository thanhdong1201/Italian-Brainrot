using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "ScriptableObjects/CharacterSO")]
public class CharacterSO : ScriptableObject
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private AudioClip audioClip;

    public string CharacterName => name;
    public Sprite Sprite => sprite;
    public AudioClip Audio => audioClip;

    public bool IsUnlocked;

    private const string UnlockKeyPrefix = "CharacterUnlocked_";
    public void Unlock()
    {
        IsUnlocked = true;
        PlayerPrefs.SetInt(UnlockKeyPrefix + name, 1);
        PlayerPrefs.Save();
    }

    public void LoadUnlockStatus()
    {
        IsUnlocked = PlayerPrefs.GetInt(UnlockKeyPrefix + name, 0) == 1;
    }
}
