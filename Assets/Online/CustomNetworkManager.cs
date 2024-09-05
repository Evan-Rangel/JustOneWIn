using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using Steamworks;
public class CustomNetworkManager : NetworkManager
{
    [SerializeField] private PlayerObjectController gamePlayerPrefab;
    public List<PlayerObjectController> gamePlayers { get; } = new List<PlayerObjectController>();

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (SceneManager.GetActiveScene().name=="Lobby")
        {
            PlayerObjectController GamePlayerInstance = Instantiate(gamePlayerPrefab);
            GamePlayerInstance.connectionID = conn.connectionId;
            GamePlayerInstance.playeridNumber =gamePlayers.Count+1;
            GamePlayerInstance.playerSteamID = (ulong)SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)SteamLobby.instance.currentLobbyID, gamePlayers.Count);
            NetworkServer.AddPlayerForConnection(conn, GamePlayerInstance.gameObject);
        }
    }
   
    public void StartGame(string SceneName)
    {
        ServerChangeScene(SceneName);
    }
}
