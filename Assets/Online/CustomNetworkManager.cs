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
    List<Transform> playerSpawns;
    int spawnIndex = 0;
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
         if (SceneManager.GetActiveScene().name == "Lobby")
         {
            playerSpawns = new List<Transform>(GameObject.Find("SpawnPoints").GetComponentsInChildren<Transform>());
            Transform spawn= playerSpawns[spawnIndex];
            spawnIndex++;
            PlayerObjectController GamePlayerInstance = Instantiate(gamePlayerPrefab, spawn.position, spawn.rotation);
             GamePlayerInstance.connectionID = conn.connectionId;
             GamePlayerInstance.playeridNumber = gamePlayers.Count + 1;
            //GamePlayerInstance.playerSteamID = (ulong)SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)SteamLobby.instance.currentLobbyID, gamePlayers.Count);
            if (SteamChecker.IsSteamAvailable())
            {
                GamePlayerInstance.playerSteamID = (ulong)SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)SteamLobby.instance.currentLobbyID, gamePlayers.Count);
            }
            else
            {
                // fallback local: usar connectionId como ID
                GamePlayerInstance.playerSteamID = (ulong)conn.connectionId;
            }
            NetworkServer.AddPlayerForConnection(conn, GamePlayerInstance.gameObject);
         }
       
    }
    public void StartGame(string SceneName)
    {
        ServerChangeScene(SceneName);
    }
}
