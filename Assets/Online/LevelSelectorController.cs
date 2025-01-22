using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;
using TMPro;
public class LevelSelectorController : MonoBehaviour
{
    public static LevelSelectorController instance;
    [SerializeField] int timeToVote;
    [SerializeField] TMP_Text voteText;
    [SerializeField] LevelData[] levelData;
    Dictionary<string, int> levelVotes= new Dictionary<string, int>();    
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
            levelVotes.Add(levelData.levelName, 0);
        }
        localPlayerObject = GameObject.Find("LocalGamePlayer");
        localPlayerController = localPlayerObject.GetComponent<PlayerObjectController>();
        
        StartCoroutine(StartTimerVote());
    }
    IEnumerator StartTimerVote()
    {
        for (int i = 0; i <= timeToVote; i++)
        {
            voteText.text = (timeToVote - i).ToString();
            yield return Helpers.GetWait(1);
        }
        if (localPlayerController.playeridNumber == 1)
        {
            StartLevel();
        }
    }
    void StartLevel()
    {
        List<string> mapMaxVotes= new List<string>();
        int maxVote=0;
        //Recount the votes of the maps
        foreach (LevelButton button in levelButtonScripts)
        {
            if (button.playersID.Count == maxVote)
            {
                mapMaxVotes.Add(button.levelName);
            }
            if (button.playersID.Count>maxVote)
            {
                mapMaxVotes.Clear(); 
                maxVote = button.playersID.Count;
                mapMaxVotes.Add(button.levelName);
            }
        }
        string mapSelected = mapMaxVotes[Random.Range(0, mapMaxVotes.Count)];
        Manager.StartGame(mapSelected);
    }
    //Level Selector
    public void ChoiceLevel(int mapId)
    {
        foreach (LevelButton button in levelButtonScripts)
        { 
            button.DisableButton();
        }
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
                    levelItemScript.SetVotes(player.character, player.skinIdx, player.playeridNumber);
                }
            }
        }
    }
}
