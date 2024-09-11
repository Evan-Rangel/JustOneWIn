using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] CharacterData[] characters;
   // Dictionary<CHARACTERS, CharacterData> charsData;
    [SerializeField] GameObject characterButtonPrefab;
    [SerializeField] Transform charactersGrid;
    [SerializeField] LevelData[] levels;
    [SerializeField] GameObject levelButtonPrefab;
    [SerializeField] Transform levelsGrid;

    [SerializeField] GameObject levelsPanel;
    [SerializeField] GameObject charactersPanel;

    public static MenuManager instance;
    public int GetMaxCharacters { get { return (characters.Length>0) ? characters.Length:0; } }
    public CharacterData RequestCharacterData (int idx ) {  return characters[idx]; }

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
        /*
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
       }*/
    }
    // CharacterData RequestCharacterData(int idx)
    // { 

    //}
    public void SelectCharacter(CharacterData _data)
    {
        levelsPanel.SetActive(true);
        charactersPanel.SetActive(false);
    }
}
