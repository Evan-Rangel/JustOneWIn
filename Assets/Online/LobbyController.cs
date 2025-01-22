using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

[Serializable]
public enum AllCharacters
{ 
    Blue,
    Red
}
public class LobbyController : MonoBehaviour
{
    public static LobbyController instance;
    
    //UI Elements
    [SerializeField] Transform charactersGrid;
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
    //Hace referencia al jugador local del cliente.
    public PlayerObjectController localPlayerController; 
    //Ready
    public Button startGameButton;
    public Button readyButton;
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
   
    public bool IsCharacterLocked(int value)
    {
        foreach (PlayerObjectController player in Manager.gamePlayers)
        {
            if (player.isLocked && player.character == value)
            {
                return true;
            }
        }
        return false;
    }
    public void LockCharacter(bool isLocked)
    {
        readyButton.interactable = isLocked;
        localPlayerController.LockCharacter(isLocked);
        if (localPlayerController.ready && !isLocked)
        {
            ReadyPlayer();
        }
    }
    public void ChangeCharacter( int _char)
    {
        localPlayerController.ChangeCharacter(_char);
    }
    public void ChangeSkin(int _skin)
    {
        localPlayerController.ChangeSkin(_skin);
    }
    //Texto para el cliente.
    public void ChangeTestText(string text)
    {
        lobbyTestText.text = text;
    }
    public void ReadyPlayer()
    {
        localPlayerController.ChangeReady();
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
    //Funcion llamada en OnStartAuthority en "PlayerObjectController"
    public void FindLocalPlayer()
    {
        localPlayerObject = GameObject.Find("LocalGamePlayer");
        localPlayerController=localPlayerObject.GetComponent<PlayerObjectController>();
    }
    //Para el host
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
            if (player == localPlayerController)
            {
                NewPlayerItemScript.StartListeners();
            }
            newPlayerItem.transform.SetParent(playerListViewContent.transform);
            newPlayerItem.transform.localScale = Vector3.one;
            playerListItems.Add(NewPlayerItemScript);
        }
        playerItemCreated = true;
    }
    //Para los clientes
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
                if (player == localPlayerController)
                {
                    NewPlayerItemScript.StartListeners();
                }
                newPlayerItem.transform.SetParent(playerListViewContent.transform);
                newPlayerItem.transform.localScale = Vector3.one;
                playerListItems.Add(NewPlayerItemScript);
              
            }
        }
    }
    //Aqui se actualizan los valores del PlayerListItem para cada uno de los clientes
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
                    
                    playerListItemScript.characterIdx = player.character;
                    playerListItemScript.skinIdx = player.skinIdx;
                    playerListItemScript.SetPlayerValues();
                    if (player == localPlayerController)
                    {
                        playerListItemScript.IsInteractuable(true);
                        UpdateButton();
                        player.ChangeIconTexture( (Texture2D)playerListItemScript.playerIcon.texture);
                    }
                    else
                    {
                        player.gameObject.GetComponent<PlayerInput>().enabled =  false;
                        player.gameObject.GetComponent<PlayerInputHandler>().enabled = false;
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
       /* foreach (PlayerObjectController player in Manager.gamePlayers)
        {
            foreach (PlayerListItem playerListItem in playerListItems)
            {
                if (playerListItem.connectionID==player.connectionID)
                {
                    player.ChangeIconTexture((Texture2D)playerListItem.playerIcon.texture);
                    //player.playerIcon = playerListItem.playerIcon;
                   // player.iconText = (Texture2D)playerListItem.playerIcon.texture;
                    //playerListItem.playerIcon.transform.parent= player.transform;
                }
            }
        }*/
        localPlayerController.CanStartGame(sceneName);
    }
}
