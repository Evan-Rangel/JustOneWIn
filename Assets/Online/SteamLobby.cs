using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;

public class SteamLobby : MonoBehaviour
{

    public static SteamLobby instance;

    //callbacks
    protected Callback<LobbyCreated_t> LobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> JoinRequest;
    protected Callback<LobbyEnter_t> LobbyEntered;

    //Lobby Callback
    protected Callback<LobbyMatchList_t> lobbyList;
    protected Callback<LobbyDataUpdate_t> lobbyDataUpdated;

    public List<CSteamID> lobbyIDs = new List< CSteamID > ();

    // variables
    public ulong currentLobbyID;
    private const string hostAddresKey = "HostAddress";
    private CustomNetworkManager manager;


    private void Start()
    {
        if (!SteamManager.Initialized) { return; }
        if (instance == null) instance = this;
      
        manager=GetComponent<CustomNetworkManager>();
        LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        JoinRequest=Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
        LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);

        lobbyList = Callback<LobbyMatchList_t>.Create(OnGetLobbyList);
        lobbyDataUpdated = Callback<LobbyDataUpdate_t>.Create(OnGetLobbyData);
    }

    public void HostLobby()
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, manager.maxConnections);
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK) { return; }

        //Debug.Log("Lobby Created Succesfully");
        manager.StartHost();
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), hostAddresKey, SteamUser.GetSteamID().ToString());
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name", SteamFriends.GetPersonaName().ToString()+"'S Lobby");
    }
    private void OnJoinRequest(GameLobbyJoinRequested_t callback)
    {
        //Debug.Log("Request To Join Lobby");
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }
    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        currentLobbyID = callback.m_ulSteamIDLobby;
        if (NetworkServer.active)
        {
            return;
        }
        manager.networkAddress=SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), hostAddresKey);

        manager.StartClient();
    }

    public void GetLobbiesList()
    {
        if (lobbyIDs.Count>0)
        {
            lobbyIDs.Clear();
        }
        SteamMatchmaking.AddRequestLobbyListResultCountFilter(60);
        SteamMatchmaking.RequestLobbyList();
    }

    void OnGetLobbyList(LobbyMatchList_t result)
    {
        /*
        if(LobbiesListManager.instance.listOfLobbies.Count>0){LobbiesListManager.instance.DestroyLobbies();}

        for(int i=0; i <result.m_nLobbiesMatching;i++)
        {
            CSteamID lobbyUD=SteamMatchmaking.GetLobbyByIndex(i);
            lobbyIDs.Add(lobbyID);
            SteamMatchmaking.RequestLobbyData(lobbyID);
        }


          */


    }
    
    void OnGetLobbyData(LobbyDataUpdate_t result)
    { 
    //LobbiesListManage.instance.DisplayLobbies(lobbyIDs, result);
    }
}
