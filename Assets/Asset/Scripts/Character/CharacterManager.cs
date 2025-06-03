using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private List<UICharacter> uiCharacters;
    [SerializeField] private List<CharacterSO> characters;
    [SerializeField] private MainMenuManager mainMenuManager;
    [SerializeField] private UISelectedCharacter uiSelectedCharacter;
    [SerializeField] private UIAskToWatchAds uiAskToWatchAds;
    [SerializeField] private Button backBtn;

    private Dictionary<CharacterSO, UICharacter> characterUIMap;
    private readonly HashSet<CharacterSO> charactersToUpdate = new HashSet<CharacterSO>();

    private void Awake()
    {
        InitializeCharacters();
    }
    private void Start()
    {
        backBtn.onClick.AddListener(OnBackButtonClicked);
    }
    private void OnDestroy()
    {
        backBtn.onClick.RemoveListener(OnBackButtonClicked);
    }
    private void OnBackButtonClicked()
    {
        uiAskToWatchAds.gameObject.SetActive(false);
        mainMenuManager.ShowPanel(UIMenuPanel.Menu);
    }
    private void InitializeCharacters()
    {
        characterUIMap = new Dictionary<CharacterSO, UICharacter>(uiCharacters.Count);
        for (int i = 0; i < uiCharacters.Count; i++)
        {
            if(i <= 11) uiCharacters[i].SetCharacterData(characters[i], this, true);
            else uiCharacters[i].SetCharacterData(characters[i], this, false);

            characterUIMap[characters[i]] = uiCharacters[i];
        }
    }

    public void SetSelectedCharacter(CharacterSO characterSO)
    {
        uiSelectedCharacter.gameObject.SetActive(true);
        uiSelectedCharacter.SetSelectedCharacter(characterSO);
    }

    public void SetUnlockCharacter(CharacterSO characterSO)
    {
        uiAskToWatchAds.SetCharacter(characterSO);
        uiAskToWatchAds.gameObject.SetActive(true);
        charactersToUpdate.Add(characterSO);
    }

    public void UpdateUICharacters()
    {
        if (charactersToUpdate.Count == 0) return;

        foreach (var characterSO in charactersToUpdate)
        {
            if (characterUIMap.TryGetValue(characterSO, out UICharacter uiCharacter))
            {
                uiCharacter.UpdateCharacter();
            }
        }
        charactersToUpdate.Clear();
    }
}