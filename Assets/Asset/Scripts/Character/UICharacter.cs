using UnityEngine;
using UnityEngine.UI;

public class UICharacter : MonoBehaviour
{
    public CharacterSO CharacterSO { get; private set; }

    [SerializeField] private Image characterImage;
    [SerializeField] private Image lockImage;
    private CharacterManager characterManager;
    private Button btn;

    private void Awake()
    {
        btn = GetComponent<Button>();
    }
    private void OnEnable()
    {
        btn.onClick.AddListener(() => OnCharacterSelected());
        UpdateCharacter();
    }
    private void OnDisable()
    {
        btn.onClick.RemoveListener(() => OnCharacterSelected());
    }
    private void OnDestroy()
    {
        btn.onClick.RemoveListener(() => OnCharacterSelected());
    }
    //Only call once from game start
    public void SetCharacterData(CharacterSO character, CharacterManager characterManager, bool forceToUnlock)
    {
        this.characterManager = characterManager;
        CharacterSO = character;
        CharacterSO.LoadUnlockStatus(forceToUnlock);
        characterImage.sprite = CharacterSO.Sprite;
        UpdateCharacter();
    }
    public void UpdateCharacter()
    {
        if(CharacterSO == null) return;
        lockImage.gameObject.SetActive(!CharacterSO.IsUnlocked);
    }
    private void OnCharacterSelected()
    {
        if (CharacterSO.IsUnlocked) characterManager.SetSelectedCharacter(CharacterSO);
        if (!CharacterSO.IsUnlocked) characterManager.SetUnlockCharacter(CharacterSO);
    }
}
