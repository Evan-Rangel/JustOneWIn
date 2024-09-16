using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using UnityEngine.Experimental.GlobalIllumination;
public class PlayerObjectController : NetworkBehaviour
{
    //Player Data
    [SyncVar] public int connectionID;
    [SyncVar] public int playeridNumber;
    [SyncVar] public ulong playerSteamID;
    [SyncVar(hook = nameof(PlayerNameUpdate))] public string playerName;
    [SyncVar(hook = nameof(PlayerReadyUpdate))] public bool ready;
    
    private CustomNetworkManager manager;

    private CustomNetworkManager Manager
    { 
        get
        {
            if (manager!=null)
            {
                return manager;
            }
            return manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }
    }
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void PlayerReadyUpdate(bool oldValue, bool newValue)
    {
        if (isServer)
        {
            LobbyController.instance.ChangeTestText("Is server");

            this.ready = newValue;
        }
        if (isClient)
        {
            LobbyController.instance.ChangeTestText("Is Client");

            //this.ready = newValue;
            LobbyController.instance.UpdatePlayerList();
        }
    }
    [Command]
    private void CmdSetPlayerReady()
    {
        LobbyController.instance.ChangeTestText("Command");

        this.PlayerReadyUpdate(this.ready, !this.ready);
    }
    public void ChangeReady()
    {
        LobbyController.instance.ChangeTestText("Not enter");
        if (authority)
        {
            LobbyController.instance.ChangeTestText("authority");
            CmdSetPlayerReady();
        }
    }
    public override void OnStartAuthority()
    {
        CmdSetPlayerName(SteamFriends.GetPersonaName().ToString());
        gameObject.name = "LocalGamePlayer";
        LobbyController.instance.ChangeTestText( "OnStartAuthority");

        LobbyController.instance.FindLocalPlayer();
        LobbyController.instance.UpdateLobbyName();
    }
    public override void OnStartClient()
    {
        Manager.gamePlayers.Add(this);
        LobbyController.instance.UpdateLobbyName();
        LobbyController.instance.UpdatePlayerList();
    }
    public override void OnStopClient()
    {
        Manager.gamePlayers.Remove(this);
        LobbyController.instance.UpdatePlayerList();
    }
    [Command]
    private void CmdSetPlayerName(string playerName)
    {
        this.PlayerNameUpdate(this.playerName, playerName);
    }
    public void PlayerNameUpdate(string oldValue, string newValue)
    {
        if (isServer)
        {
            this.playerName = newValue;
        }
        if (isClient)
        {
            LobbyController.instance.UpdatePlayerList();
        }
    }

    public void CanStartGame(string sceneName)
    {
        //if (authority)
        {
            CmdCanStartGame(sceneName);
        }
    }
    [Command]
    public void CmdCanStartGame(string sceneName)
    {
        Manager.StartGame(sceneName);
    } 
    
    [SyncVar(hook = nameof(SendPlayerCharacter))] public CharacterData character;
    [Command]
    public void CmdUpdatePlayerCharacter(CharacterData newData)
    {
        SendPlayerCharacter(character, newData);
    }
    public void SendPlayerCharacter(CharacterData oldValue, CharacterData newValue)
    {
        if (isServer)
        {
            character = newValue;
        }
        if (isClient&&(oldValue!=newValue))
        {
            UpdateCharacter(newValue);
        }
    }
    void UpdateCharacter(CharacterData message)
    {
        character = message;
        LobbyController.instance.UpdatePlayerList();
    }

    [SyncVar(hook = nameof(SendCharacterSkin))] public int skinIdx;
    [Command]
    public void CmdUpdateCharacterSkin(int newData)
    {
        SendCharacterSkin(skinIdx, newData);
    }
    public void SendCharacterSkin(int oldValue, int newValue)
    {
        if (isServer)
        {
            skinIdx = newValue;
        }
        if (isClient&&(oldValue!=newValue))
        {
            UpdateSkin(newValue);
        }
    }
    void UpdateSkin(int message)
    {
        skinIdx = message;
        LobbyController.instance.UpdatePlayerList();
    }
}
