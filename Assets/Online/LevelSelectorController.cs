using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectorController : MonoBehaviour
{
    public static LevelSelectorController instance;
    [SerializeField] LevelData[] levelData;
    List<int> levelVotes= new List<int>();    
    [SerializeField] GameObject levelButtonPrefab;
    [SerializeField] GameObject levelButtonsGrid;
    public PlayerObjectController localPlayerController;
    public GameObject localPlayerObject;
    List<LevelButton> levelButtonScripts= new List<LevelButton>();
    //Manager
    private CustomNetworkManager manager;
    private CustomNetworkManager Manager
    {
        get
        {
            if (manager != null)
            {
                return manager;
            }
            return manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }
    }
    private void Awake()
    {
        if (instance == null) { instance = this; }
    }
    private void Start()
    {
        foreach (LevelData levelData in levelData)
        {
            GameObject button= Instantiate(levelButtonPrefab, parent: levelButtonsGrid.transform);
            LevelButton tempButton = button.GetComponent<LevelButton>();
            tempButton.SetLevelButton(levelData);
            levelButtonScripts.Add(tempButton);
            levelVotes.Add(0);
        }
        localPlayerObject = GameObject.Find("LocalGamePlayer");
        localPlayerController = localPlayerObject.GetComponent<PlayerObjectController>();
    }
    //Level Selector
    public void ChoiceLevel(int mapId)
    {
        localPlayerController.ChangeMapChoice(mapId);
    }
    public void UpdateLevelList()
    {
        foreach (PlayerObjectController player in Manager.gamePlayers)
        {
            foreach (LevelButton levelItemScript in levelButtonScripts)
            {
                if (levelItemScript.levelId == player.mapChoice)
                {
                    //int tMap = player.mapChoice;
                    //levelVotes[tMap]++;
                    levelItemScript.SetVotes(player.character, player.skinIdx, player.playeridNumber);
                    //break ;
                }
            }
        }
    }
}
