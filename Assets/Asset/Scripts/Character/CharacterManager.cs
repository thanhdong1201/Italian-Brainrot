using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private List<UICharacter> uiCharacters;
    [SerializeField] private List<CharacterSO> characters;

    [SerializeField] private MainMenuManager mainMenuManager;
    [SerializeField] private UISelectedCharacter uiSelectedCharacter;
    [SerializeField] private Button backBtn;

    private void Awake()
    {
        InitializeCharacters();
    }
    private void Start()
    {
        backBtn.onClick.AddListener(() => mainMenuManager.ShowPanel(UIMenuPanel.Menu));
    }
    private void OnDestroy()
    {
        backBtn.onClick.RemoveListener(() => mainMenuManager.ShowPanel(UIMenuPanel.Menu));
    }
    private void InitializeCharacters()
    {
        for (int i = 0; i < uiCharacters.Count; i++)
        {
            uiCharacters[i].SetCharacterData(characters[i], this);
        }
    }
    public void SetSelectedCharacter(CharacterSO characterSO)
    {
        uiSelectedCharacter.gameObject.SetActive(true);
        uiSelectedCharacter.SetSelectedCharacter(characterSO);
    }
}
