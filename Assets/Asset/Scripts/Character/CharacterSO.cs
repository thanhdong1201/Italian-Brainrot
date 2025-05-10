using UnityEngine;


[CreateAssetMenu(fileName = "Character", menuName = "ScriptableObjects/CharacterSO")]
public class CharacterSO : ScriptableObject
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private AudioClip audioClip;

    public string CharacterName => name;
    public Sprite Sprite => sprite;
    public AudioClip Audio => audioClip;
    public bool IsUnlocked = false;
}
