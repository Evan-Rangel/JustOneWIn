using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class LobbyController : MonoBehaviour
{
    public static LobbyController instance;

    //UI Elements
    public TMP_Text lobbyNameText;
    public TMP_Text lobbyTestText;
    //Player Data
    public GameObject playerListViewContent;
    public GameObject playerListItemPrefab;
    public GameObject localPlayerObject;
    //Other Data
    public ulong currentLobbyID;
    public bool playerItemCreated = false;
    private List<PlayerListItem> playerListItems = new List<PlayerListItem>();
    public PlayerObjectController localPlayerController;


    //Ready
    public Button startGameButton;
    public TMP_Text readyButtonText;


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
    public void ChangeTestText(string text)
    {
        lobbyTestText.text = text;
    }
    public void ReadyPlayer()
    {
        lobbyTestText.text = "Pressed ";

        localPlayerController.ChangeReady();
        //count++;
    }
    public void UpdateButton()
    {
        if (localPlayerController.ready)
        {
            readyButtonText.text = "Unready";
        }
        else 
        {
            readyButtonText.text = "Ready";

        }
    }
    public void CheckIfAllReady()
    {
        //startGameButton.interactable = true;
        
        bool allReady = false;
        foreach (PlayerObjectController player in Manager.gamePlayers) 
        {
            if (player.ready)
            {
                allReady = true;
            }
            else
            {
                allReady = false;
                break;
            }
        }
        if (allReady)
        {
            if (localPlayerController.playeridNumber==1)
            {
                startGameButton.interactable = true;
            }
            else
            {
                startGameButton.interactable = false;
            }
        }
        else 
        {
            startGameButton.interactable = false ;
        }
    }
    public void UpdateLobbyName()
    {
        currentLobbyID = Manager.GetComponent<SteamLobby>().currentLobbyID;
        lobbyNameText.text = SteamMatchmaking.GetLobbyData(new CSteamID(currentLobbyID), "name");
    }

    public void UpdatePlayerList()
    {
        if (!playerItemCreated) { CreateHostPlayerItem(); }
        if (playerListItems.Count < Manager.gamePlayers.Count) { CreateClientPlayerItem(); }
        if (playerListItems.Count > Manager.gamePlayers.Count) { RemovePlayerItem(); }
        if (playerListItems.Count == Manager.gamePlayers.Count) { UpdatePlayerItem(); }
    }
    public void FindLocalPlayer()
    {
        localPlayerObject = GameObject.Find("LocalGamePlayer");
        localPlayerController=localPlayerObject.GetComponent<PlayerObjectController>();
    }
    public void CreateHostPlayerItem()
    {
        foreach (var player in Manager.gamePlayers) {
            GameObject newPlayerItem = Instantiate(playerListItemPrefab)as GameObject;
            PlayerListItem NewPlayerItemScript= newPlayerItem.GetComponent<PlayerListItem>();
            NewPlayerItemScript.playerName = player.playerName;
            NewPlayerItemScript.connectionID = player.connectionID;
            NewPlayerItemScript.playerSteamID= player.playerSteamID;
            NewPlayerItemScript.ready = player.ready;
            NewPlayerItemScript.SetPlayerValues();

            newPlayerItem.transform.SetParent(playerListViewContent.transform);
            newPlayerItem.transform.localScale = Vector3.one;
            playerListItems.Add(NewPlayerItemScript);
        }
        playerItemCreated = true;
    }
    public void CreateClientPlayerItem()
    { 
       foreach(PlayerObjectController player in Manager.gamePlayers )
        {
            if (!playerListItems.Any(b=>b.connectionID==player.connectionID))
            {
                GameObject newPlayerItem = Instantiate(playerListItemPrefab) as GameObject;
                PlayerListItem NewPlayerItemScript = newPlayerItem.GetComponent<PlayerListItem>();
                NewPlayerItemScript.playerName = player.playerName;
                NewPlayerItemScript.connectionID = player.connectionID;
                NewPlayerItemScript.playerSteamID = player.playerSteamID;
                NewPlayerItemScript.ready = player.ready;
                NewPlayerItemScript.SetPlayerValues();

                newPlayerItem.transform.SetParent(playerListViewContent.transform);
                newPlayerItem.transform.localScale = Vector3.one;
                playerListItems.Add(NewPlayerItemScript);
            }
        }
    }
    public void UpdatePlayerItem() 
    {
        foreach (PlayerObjectController player in Manager.gamePlayers)
        {
            foreach (PlayerListItem playerListItemScript in playerListItems)
            {
                if (playerListItemScript.connectionID==player.connectionID)
                {
                    playerListItemScript.playerName = player.playerName;
                    playerListItemScript.ready = player.ready;
                    playerListItemScript.SetPlayerValues();
                    if (player==localPlayerController)
                    {
                        UpdateButton();
                    }
                }
            }
        }
        CheckIfAllReady();
    }
    public void RemovePlayerItem()
    {
        List<PlayerListItem> playerListItemToRemove= new List<PlayerListItem>();
        foreach (PlayerListItem playerListItem in playerListItems)
        {
            if (!Manager.gamePlayers.Any(b=>b.connectionID==playerListItem.connectionID))
            {
                playerListItemToRemove.Add(playerListItem);
            }
        }
        if (playerListItemToRemove.Count>0)
        {
            foreach (PlayerListItem _playerListItemToRemove in playerListItemToRemove)
            {
                GameObject objectToRemove = _playerListItemToRemove.gameObject;
                playerListItems.Remove(_playerListItemToRemove) ;
                Destroy(objectToRemove);
                objectToRemove = null;
            }
        }
    }
    public void StartGame(string sceneName)
    {
        localPlayerController.CanStartGame(sceneName);
    }
}
