using UnityEngine;
using Mirror;
using Steamworks;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Unity.VisualScripting;
using Avocado.Weapons;
using UnityEngine.UI;
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
        propertyBlock = new MaterialPropertyBlock();

        //playerScript = GetComponent<Player>();
    }
    #region Initialization
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
        if (SceneManager.GetActiveScene().name == "Lobby")

        {

            LobbyController.instance.FindLocalPlayer();
            LobbyController.instance.UpdateLobbyName();
        }

    }
    public override void OnStartClient()
    {
        Manager.gamePlayers.Add(this);
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            LobbyController.instance.UpdateLobbyName();
            LobbyController.instance.UpdatePlayerList();
        }

    }
    public override void OnStopClient()
    {
        Manager.gamePlayers.Remove(this);

        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            LobbyController.instance.UpdatePlayerList();
        }

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
            if (SceneManager.GetActiveScene().name == "Lobby")
            {
                LobbyController.instance.UpdatePlayerList();
            }

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
    #endregion
    #region MapChoice
    [SyncVar(hook = nameof(SendMapChoiced))] public int mapChoice;
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
        if (isClient && oldValue != newValue)
        {
            UpdateMapChoice(newValue);
        }
    }
    void UpdateMapChoice(int message)
    {
        mapChoice = message;
        LevelSelectorController.instance.UpdateLevelList();
    }
    #endregion

    #region Weapons

    /*[SyncVar(hook = nameof(OnWeaponChanged))]
    private int currentWeaponIndex;
    public Weapon CurrentWeapon { get; private set; }
    public Player playerScript;
    void OnWeaponChanged(int oldWeapon, int newWeapon)
    {
        if (isClient)
        {
            EquipWeapon(newWeapon);
        }
    }
    [Command]
    public void CmdEquipWeapon(int weaponIndex)
    {
        currentWeaponIndex = weaponIndex;
        EquipWeapon(weaponIndex);
    }
    private void EquipWeapon(int index)
    {
        // Asigna la nueva arma basada en el índice de los prefabs
        CurrentWeapon = GetWeaponByIndex(index);
    }
    private Weapon GetWeaponByIndex(int index)
    {
        // Implementa lógica para obtener el arma correcta
        return playerScript.weaponList[index];
    }*/
    /* [SyncVar(hook = nameof(SendWeaponIndex))] public int weaponIndex;
     [Command]
     public void CmdUpdateWeaponIndex(int newData)
     {
         SendWeaponIndex(this.weaponIndex, newData);
     }
     public void ChangeWeaponIndex(int weaponI)
     {
         CmdUpdateWeaponIndex(weaponI);
     }
     public void SendWeaponIndex(int oldValue, int newValue)
     {
         if (isServer)
         {
             this.weaponIndex = newValue;
         }
         if (isClient && oldValue != newValue)
         {
             UpdateWeaponIndex(newValue);
         }
     }
     void UpdateWeaponIndex(int message)
     {
         weaponIndex = message;
         GameManager.instance.UpdatePlayers();
     }


     [SyncVar(hook = nameof(SendAttackActive))] public bool attackActive;
     [Command]
     public void CmdUpdateAttackActive(bool newData)
     {
         SendAttackActive(this.attackActive, newData);
     }
     public void ChangeAttackActive(bool value)
     {
         CmdUpdateAttackActive(value);
     }
     public void SendAttackActive(bool oldValue, bool newValue)
     {
         if (isServer)
         {
             this.attackActive = newValue;
         }
         if (isClient && oldValue != newValue)
         {
             UpdateAttackActive(newValue);
         }
     }
     void UpdateAttackActive(bool message)
     {
         attackActive = message;
         GameManager.instance.UpdatePlayers();
     }
    */
    #endregion
    #region Lobby

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
        if (isServer)
        {

            this.character = newValue;
        }
        if (isClient && (oldValue != newValue))
        {
            UpdateCharacter(newValue);
        }
    }
    void UpdateCharacter(int message)
    {
        character = message;
        propertyBlock.SetColor("_Color", charsData[character].color);
        foreach (SpriteRenderer item in sprs)
        {
            item.SetPropertyBlock(propertyBlock);
        }
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            LobbyController.instance.UpdatePlayerList();
        }

    }
    [Space]
    [Header("Player Color")]
    SpriteRenderer spr;
    [SerializeField] List<SpriteRenderer> sprs;
    public MaterialPropertyBlock propertyBlock { get; private set; }

    //Color[] playerColors = { Color.cyan, Color.red };
    [SerializeField] CharacterData[] charsData;





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
        isLocked = newValue;
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            LobbyController.instance.UpdatePlayerList();
        }

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
        if (isClient && (oldValue != newValue))
        {
            UpdateSkin(newValue);
        }
    }
    void UpdateSkin(int message)
    {
        skinIdx = message;
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            LobbyController.instance.UpdatePlayerList();
        }

    }
    #endregion


    #region UI
    [SyncVar(hook = nameof(SendItemIdx))] public int itemIdx;
    public Item currentItem { get; private set; }
    [Command]
    public void CmdUpdateItemIdx(int newData)
    {
        SendItemIdx(this.itemIdx, newData);
    }
    public void ChangeItemIdx(int newData)
    {
        CmdUpdateItemIdx(newData);
    }
    public void SendItemIdx(int oldValue, int newValue) 
    {
        if (isServer)
        {
            this.itemIdx = newValue;
            this.currentItem = GameManager.instance.GetItemByIndex(this.itemIdx);
        }
        if (isClient )
        {
            UpdateItemIdx(newValue);
        }
    }
    void UpdateItemIdx(int message)
    {
        itemIdx = message;
        currentItem= GameManager.instance.GetItemByIndex(itemIdx);
    }

    public RawImage playerIcon;
  
 
    #endregion

    #region Collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ICollidable coll = collision.transform.GetComponent<ICollidable>();
        if (coll != null)
        {
            CmdNotifyObjectCollidable(collision.gameObject);
            //ActiveShadowEffect();
        }
    }
    [Command]
    public void CmdNotifyObjectCollidable(GameObject collision)
    {
        collision.GetComponent<ICollidable>().OnCollision();
    }
    #endregion

    #region Effects
    public void ActiveShadowEffect()
    {
        FakeLight_S fLight = GetComponentInChildren<FakeLight_S>();
        fLight.StartShadowEffect();
    }

    #endregion
}
