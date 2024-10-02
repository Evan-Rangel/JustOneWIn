using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using UnityEngine.Experimental.GlobalIllumination;
using System;
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
            if (manager != null)
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
            this.ready = newValue;
        }
        if (isClient)
        {
            LobbyController.instance.UpdatePlayerList();
        }
    }
    [Command]
    private void CmdSetPlayerReady()
    {
        this.PlayerReadyUpdate(this.ready, !this.ready);
    }
    public void ChangeReady()
    {
        CmdSetPlayerReady();
    }
    public override void OnStartAuthority()
    {
        CmdSetPlayerName(SteamFriends.GetPersonaName().ToString());
        gameObject.name = "LocalGamePlayer";
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

    [SyncVar(hook = nameof(SendPlayerCharacter))] public int character;
    [Command]
    public void CmdUpdatePlayerCharacter(int newData)
    {
        this.SendPlayerCharacter(this.character, newData);
    }
    public void ChangeCharacter(int newData)
    {
        //if (authority) 
        {
            CmdUpdatePlayerCharacter(newData);
        }
    }
    public void SendPlayerCharacter(int oldValue, int newValue)
    {
        //    if (!Manager.lockedCharacters.Contains(newValue))
        {
            if (isServer)
            {

                this.character = newValue;
            }
            if (isClient && (oldValue != newValue))
            {
                UpdateCharacter(newValue);
            }
            //Manager.LockCharacter(newValue);
        }
    }
    void UpdateCharacter(int message)
    {
        character = message;
        LobbyController.instance.UpdatePlayerList();
    }

    [SyncVar] public bool isLocked;
    [Command]
    public void CmdLockCharacters(bool newValue)
    {
        this.SendLocked(isLocked, newValue);
    }
    public void SendLocked(bool oldValue, bool newValue)
    {
        if (isServer)
        {

            this.isLocked = newValue;
        }
        if (isClient && (oldValue != newValue))
        {
            SetCharactersLocked(newValue);
        }
    }
    public void LockCharacter(bool newValue)
    {
        CmdLockCharacters(newValue);
    }
    void SetCharactersLocked(bool newValue)
    {
        isLocked= newValue;
        LobbyController.instance.UpdatePlayerList();
    }

    [SyncVar(hook = nameof(SendCharacterSkin))] public int skinIdx;
    [Command]
    public void CmdUpdateCharacterSkin(int newData)
    {
        SendCharacterSkin(this.skinIdx, newData);
    }
    public void ChangeSkin(int newData)
    {
        CmdUpdateCharacterSkin(newData);
    }
    public void SendCharacterSkin(int oldValue, int newValue)
    {
        if (isServer)
        {
            this.skinIdx = newValue;
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

    [SyncVar] public int mapChoice;
    [Command]
    public void CmdUpdateMapChoiced(int newData)
    {
        SendMapChoiced(this.mapChoice, newData);
    }
    public void ChangeMapChoice(int mapChoiced)
    {
        CmdUpdateMapChoiced(mapChoiced);
    }
    public void SendMapChoiced(int oldValue, int newValue)
    {
        if (isServer)
        {
            this.mapChoice = newValue;
        }
        if (isClient)
        {
            UpdateMapChoice(newValue);
        }
    }
    void UpdateMapChoice(int message)
    {
        mapChoice = message;
        LevelSelectorController.instance.UpdateLevelList();
    }
}
