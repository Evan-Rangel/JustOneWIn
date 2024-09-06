using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] CharacterData[] characters;
    [SerializeField] GameObject characterButtonPrefab;
    [SerializeField] Transform charactersGrid;
    [SerializeField] LevelData[] levels;
    [SerializeField] GameObject levelButtonPrefab;
    [SerializeField] Transform levelsGrid;

    [SerializeField] GameObject levelsPanel;
    [SerializeField] GameObject charactersPanel;

    public static MenuManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        foreach (LevelData levelD in levels)
        {
            LevelButton _level = Instantiate(levelButtonPrefab).GetComponent<LevelButton>();
            _level.SetLevelButton(levelD);
            _level.transform.parent = levelsGrid;
        }
        foreach (CharacterData charD in characters)
        {
            CharacterButton _char = Instantiate(characterButtonPrefab).GetComponent<CharacterButton>();
            _char.SetCharacterData(charD);
            _char.transform.parent = charactersGrid;
        }
    }
    public void SelectCharacter(CharacterData _data)
    {
        levelsPanel.SetActive(true);
        charactersPanel.SetActive(false);
    }
}
