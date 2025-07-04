using UnityEngine;
using Mirror;
using Steamworks;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Events;
using Avocado.CoreSystem;
using System.Collections;
using Avocado.Weapons;
using Avocado.Interaction.Interactables;
using Avocado.ProjectileSystem.Components;
using Avocado.Projectiles;

public class PlayerObjectController : NetworkBehaviour
{
    //Player Data
    [SyncVar] public int connectionID;
    [SyncVar] public int playeridNumber;
    [SyncVar] public ulong playerSteamID;
    [SyncVar(hook = nameof(PlayerNameUpdate))] public string playerName;
    [SyncVar(hook = nameof(PlayerReadyUpdate))] public bool ready;

    [SerializeField] List<string> levelScenesNames;
  
    private CustomNetworkManager manager;
    Player player;
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
        player = GetComponent<Player>();
        primaryWeapon.EventHandler.OnRequestAttackAction += TryPrimaryWeaponAttackAction;
        secondaryWeapon.EventHandler.OnRequestAttackAction += TrySecondaryWeaponAttackAction;
        
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

       // inputs = GetComponent<PlayerInputHandler>();
     

        // CmdSetPlayerName(SteamFriends.GetPersonaName().ToString());
        if (SteamChecker.IsSteamAvailable())
        {
            CmdSetPlayerName(SteamFriends.GetPersonaName());
        }
        else
        {
            // nombre local de test
            CmdSetPlayerName("Player" + playeridNumber);
        }
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



    #region Animator
    [Header("Animator Principal")]
    [SerializeField] Animator anim;

    // Este método lo llamamos desde la FSM
    public void NetworkSetBool(string param, bool value)
    {
        if (isServer)
        {
            anim.SetBool(param, value);
            RpcSetBool(param, value);
        }
        else if (authority)
        {
            CmdSetBool(param, value);
        }
    }

    [Command]
    void CmdSetBool(string param, bool value)
    {
        anim.SetBool(param, value);
        RpcSetBool(param, value);
    }

    [ClientRpc]
    void RpcSetBool(string param, bool value)
    {
        // Todos los clientes reciben el cambio
        anim.SetBool(param, value);
    }

    // Haz lo mismo para triggers si los usas:
    public void NetworkSetTrigger(string param)
    {
        if (isServer)
        {
            anim.SetTrigger(param);
            RpcSetTrigger(param);
        }
        else if (authority)
        {
            CmdSetTrigger(param);
        }
    }

    [Command]
    void CmdSetTrigger(string param)
    {
        anim.SetTrigger(param);
        RpcSetTrigger(param);
    }

    [ClientRpc]
    void RpcSetTrigger(string param)
    {
        anim.SetTrigger(param);
    }


    #endregion


    #region Weapons
    #region Selection
    [SerializeField] List<WeaponDataSO> allWeapons;
    [SerializeField] private WeaponPickup weaponPickupPrefab;

    public void SpawnDiscardedWeapon(WeaponDataSO weaponDataSO, Vector2 spawnPoint, Vector2 direction)
    { 
        int index= allWeapons.IndexOf(weaponDataSO);
        CmdSpawnDiscardedWeapon(index, spawnPoint, direction);
    }
    [Command]
    public void CmdSpawnDiscardedWeapon(int weaponDataId, Vector2 spawnPoint, Vector2 direction)
    {
        var data = allWeapons[weaponDataId];

        // Instancia y configura
        var wp = Instantiate(
            weaponPickupPrefab,
            spawnPoint,
            Quaternion.identity
        );
        wp.SetContext(data);
        wp.Rigidbody2D.velocity = direction;

        NetworkServer.Spawn(wp.gameObject);
    }


    [Command]
    public void CmdPickupWeapon(uint pickupNetId)
    {
        if (NetworkServer.spawned.TryGetValue(pickupNetId, out var obj))
        {
            NetworkServer.Destroy(obj.gameObject);
        }
    }


    // Llama este método cuando quieras iniciar un ataque
    public void NetworkStartAttack(int attackType) // 0 = primario, 1 = secundario
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (!levelScenesNames.Contains(sceneName) ||
          (attackType == 0 && !player.primaryWeaponHasData) ||
          attackType == 1 && !player.secondaryWeaponHasData)
            return;
        if (isServer)
        {
            RpcStartAttack(attackType);
        }
        else if (authority)
        {
            CmdStartAttack(attackType);
        }
    }

    [Command]
    void CmdStartAttack(int attackType)
    {
        RpcStartAttack(attackType);
    }

    [ClientRpc]
    void RpcStartAttack(int attackType)
    {
        // Aquí, activa la animación y estado de ataque en todos los clientes.
        if (attackType == 0)
            player.PrimaryAttackState.Enter();
        else
            player.SecondaryAttackState.Enter();
    }
    public void NetworkStopAttack(int attackType)
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (!levelScenesNames.Contains(sceneName) || 
            (attackType==0 &&!player.primaryWeaponHasData)||
            attackType==1 && !player.secondaryWeaponHasData)
            return;
        if (isServer)
        {
            RpcStopAttack(attackType);
        }
        else if (authority)
        {
            CmdStopAttack(attackType);
        }
    }

    [Command]
    void CmdStopAttack(int attackType)
    {
        RpcStopAttack(attackType);
    }

    [ClientRpc]
    void RpcStopAttack(int attackType)
    {
        if (attackType == 0)
            player.PrimaryAttackState.Exit();
        else
            player.SecondaryAttackState.Exit();
    }
    #endregion


    [SerializeField]Weapon primaryWeapon;
    [SerializeField] Weapon secondaryWeapon; 
   

    private void OnDestroy()
    {
        primaryWeapon.EventHandler.OnRequestAttackAction -= TryPrimaryWeaponAttackAction;
        secondaryWeapon.EventHandler.OnRequestAttackAction -= TrySecondaryWeaponAttackAction;
    }

    private void TryPrimaryWeaponAttackAction()
    {
       // Debug.Log("TryPrimaryWeaponAttackAction");
       // if (authority) // este componente SÍ tiene autoridad
            CmdPrimaryWeaponAttackAction();
    }

    [Command]
    private void CmdPrimaryWeaponAttackAction()
    {
       // Debug.Log("COMMAND");

        // Aquí ya puedes ejecutar la lógica real del ataque en el servidor
        primaryWeapon.Enter();
        primaryWeapon.EventHandler.AttackAction();
    }  
    private void TrySecondaryWeaponAttackAction()
    {
        //Debug.Log("COMMANDSECONDARY");

         //if (authority) // este componente SÍ tiene autoridad
        CmdSecondaryWeaponAttackAction();
    }

    [Command]
    private void CmdSecondaryWeaponAttackAction()
    {
        // Aquí ya puedes ejecutar la lógica real del ataque en el servidor
        secondaryWeapon.Enter();
        secondaryWeapon.EventHandler.AttackAction();
    }
    #region Shoots


    /* [Command]
     public void CmdDispararProjectile(Vector3 posicion, Vector2 direccion)
     {
         // 1. Instanciar el proyectil (solo en servidor)
         GameObject projGO = Instantiate(projectilePrefab);
         projGO.transform.position = posicion;
         projGO.transform.rotation = Quaternion.LookRotation(Vector3.forward, direccion);

         // 2. Opcional: inicializar datos del proyectil (daño, velocidad, etc.)
         Projectile projectileComp = projGO.GetComponent<Projectile>();
         projectileComp.InicializarDatos(damage, otherStats, ...);

         // 3. Spawn en la red para que aparezca en todos los clientes
         NetworkServer.Spawn(projGO);
     }
    */

    #endregion
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

    #region UI / Items / Player Icon
    #region Items
    public UnityEvent grabItem;
    public UnityEvent useItem;
    public UnityEvent dropItem;
    [SerializeField] Transform itemSpawner;

    [Command]
    public void InstantiateItem(Vector2 direction)
    {
        if (currentItem == null) return;
        if (currentItem.GetComponent<Rigidbody2D>() == null)
        {
            NotifyFlash();
        }
        else
        {
            GameObject itemInstance = Instantiate(currentItem);
            itemInstance.transform.position = itemSpawner.position;
            itemInstance.GetComponent<Rigidbody2D>().velocity = direction * 15;
            NetworkServer.Spawn(itemInstance);
        }

        currentItem = null;
    }

    [ClientRpc(includeOwner = false)]
    public void NotifyFlash()
    {
        FakeLight_S.instance.StartBlindEffect();
    }


    [SyncVar(hook = nameof(SendItemIdx))] public int itemIdx;
    public GameObject currentItem;
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
            grabItem.Invoke();

        }
        if (isClient)
        {
            UpdateItemIdx(newValue);
        }
    }
    void UpdateItemIdx(int message)
    {
        itemIdx = message;
        currentItem = GameManager.instance.GetItemByIndex(itemIdx);
        grabItem.Invoke();
    }


    #endregion

    #region PlayerIcon
    [SyncVar(hook = nameof(SendIconTexture))] public Texture2D iconText;
    [Command]
    public void CmdUpdateIconTexture(Texture2D newData)
    {
        SendIconTexture(this.iconText, newData);
    }
    public void ChangeIconTexture(Texture2D newData)
    {
        CmdUpdateIconTexture(newData);
    }
    public void SendIconTexture(Texture2D oldValue, Texture2D newValue)
    {
        if (isServer)
        {
            this.iconText = newValue;
        }
        if (isClient)
        {
            UpdateIconTexture(newValue);
        }
    }
    void UpdateIconTexture(Texture2D message)
    {
        iconText = message;
    }

    #endregion
    #endregion
    #region Collisions

    Transform respawn;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!authority) return;


        ICollidable coll = collision.transform.GetComponent<ICollidable>();
        if (coll != null)
        {
            CmdNotifyObjectCollidable(collision.gameObject);
            //ActiveShadowEffect();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name == "Respawn")
        {
            respawn = collision.transform;
        }
        if (collision.transform.name == "Death")
        {
            FakeLight_S.instance.StartRespawnEffect();

            CmdNotifyRespawn();
        }


        ICollidable coll = collision.transform.GetComponent<ICollidable>();
        if (coll != null)
        {
            CmdNotifyObjectCollidable(collision.gameObject);
        }
        Item _item = collision.transform.GetComponent<Item>();
        if (_item != null)
        {
            ChangeItemIdx(Random.Range(0, GameManager.instance.itemList.Count));
        }
    }
    [Command]
    public void CmdNotifyObjectCollidable(GameObject collision)
    {
        collision.GetComponent<ICollidable>().OnCollision();
    }
    [Command]
    public void CmdNotifyRespawn()
    {
        StartCoroutine(RespawnEffect());
        StartCoroutine(ShakeCamera());
    }
    #endregion
    #region Effects
    [Header("Effects")]
    [SerializeField] AnimationCurve curve;
    [SerializeField] float shakeDuration;
    IEnumerator ShakeCamera()
    {
        float elapsedTime=0;
        while (elapsedTime < shakeDuration)
        {
            float strength = curve.Evaluate(elapsedTime/shakeDuration);
            GetComponent<PCameraController>().shakeStrength = strength;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        GetComponent<PCameraController>().shakeStrength = 0;

    }
    IEnumerator RespawnEffect()
    {
      

        yield return Helpers.GetWait(0.1f);
        propertyBlock.SetFloat("_Alpha", 0);
        foreach (SpriteRenderer item in sprs)
        {
            item.SetPropertyBlock(propertyBlock);
        }

        yield return Helpers.GetWait(0.15f);
        propertyBlock.SetFloat("_Alpha", 1);
        foreach (SpriteRenderer item in sprs)
        {
            item.SetPropertyBlock(propertyBlock);
        } 
        
        yield return Helpers.GetWait(0.3f);
        propertyBlock.SetFloat("_Alpha", 0);
        foreach (SpriteRenderer item in sprs)
        {
            item.SetPropertyBlock(propertyBlock);
        }
        
        yield return Helpers.GetWait(0.15f);
        propertyBlock.SetFloat("_Alpha", 1);
        foreach (SpriteRenderer item in sprs)
        {
            item.SetPropertyBlock(propertyBlock);
        } 
        
        yield return Helpers.GetWait(0.3f);
        propertyBlock.SetFloat("_Alpha", 0);
        foreach (SpriteRenderer item in sprs)
        {
            item.SetPropertyBlock(propertyBlock);
        }

        transform.position = respawn.position;

        yield return Helpers.GetWait(0.15f);
        propertyBlock.SetFloat("_Alpha", 1);
        foreach (SpriteRenderer item in sprs)
        {
            item.SetPropertyBlock(propertyBlock);
        }

        yield return Helpers.GetWait(0.3f);
        propertyBlock.SetFloat("_Alpha", 0);
        foreach (SpriteRenderer item in sprs)
        {
            item.SetPropertyBlock(propertyBlock);
        }

        yield return Helpers.GetWait(0.15f);
        propertyBlock.SetFloat("_Alpha", 1);
        foreach (SpriteRenderer item in sprs)
        {
            item.SetPropertyBlock(propertyBlock);
        }


        yield return Helpers.GetWait(0.3f);
        propertyBlock.SetFloat("_Alpha", 0);
        foreach (SpriteRenderer item in sprs)
        {
            item.SetPropertyBlock(propertyBlock);
        }

        yield return Helpers.GetWait(0.1f);
        propertyBlock.SetFloat("_Alpha", 1);
        foreach (SpriteRenderer item in sprs)
        {
            item.SetPropertyBlock(propertyBlock);
        }
 
    }
    public void ActiveShadowEffect()
    {   
        FakeLight_S fLight = GetComponentInChildren<FakeLight_S>();
        fLight.StartBlindEffect();
    }
    #endregion
}
