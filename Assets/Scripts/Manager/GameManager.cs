using Avocado;
using Avocado.CoreSystem;
using Avocado.Weapons;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region FPS Display
    [SerializeField] TMPro.TMP_Text fpsText;
    [SerializeField] float maxFps = 0, minFps = 1000;

    void ShowFps()
    {

        float fps = 1f / Time.unscaledDeltaTime;
        if (fps < minFps)
        {
            minFps = fps;
            //Debug.Log(minFps);
        }
        if (fps > maxFps)
            maxFps = fps;

        fpsText.text = Mathf.RoundToInt(fps).ToString();
        Invoke("ShowFps", 0.1f);

    }
    #endregion
    #region Local Game
    Stats stats;
    public Transform newGameStartPosition;
    #region Coins 
    [Header("Coins")]
    public int coins;
    [SerializeField] TMP_Text coinsText;
    public void AddCoin(int _value)
    {
        if (coins == 999)
            return;
        coins += _value;
        SaveManager.SaveCoins(coins);
        coinsText.text = coins.ToString();
    }
    public bool SubstractCoin(int _value)
    {
        if (coins - _value < 0)
            return false;
        coins -= _value;
        SaveManager.SaveCoins(coins);
        coinsText.text = coins.ToString();
        return true;
    }
    void CoinsInPlayerPrefs()
    {
        coins = SaveManager.GetCoins();
        coinsText.text = coins.ToString();
    }


    [SerializeField] GameObject coinPrefab;
    List<GameObject> coinsPool = new List<GameObject>();
    public GameObject RequestCoin()
    {
        foreach (GameObject coin in coinsPool)
        {
            if (!coin.activeInHierarchy)
            {
                coin.SetActive(true);
                return coin;
            }
        }
        GameObject newCoin = Instantiate(coinPrefab);
        coinsPool.Add(newCoin);
        return newCoin;
    }



    #endregion

    #region UI
    #region Sounds
    public void PlayButtonSelectSound() => AudioManager.instance.PlaySFXSound("ButtonSelect");
    public void PlayButtonSubmitSound() => AudioManager.instance.PlaySFXSound("ButtonSubmit");


    #endregion
    [Header("User Interface")]
    [SerializeField] GameObject statsHolder;
    [SerializeField] GameObject pauseHolder;
    [SerializeField] GameObject pausePrincipalHolder;
    [SerializeField] GameObject[] holderToHideWithEsc;
    [SerializeField] GameObject[] objectsToDelayActivation;
    [field: SerializeField] public GameObject shopHolder { get; private set; }
    [SerializeField] Animator healthAnimator, staminaAnimator, statsBackgroundAnimator;
    [SerializeField] Image healthBar, staminaBar;
    int healthLevel = 1, staminaLevel = 1;
    [SerializeField] ShopItemHolder[] shopItemHolders;
    [field: SerializeField] public WeaponDataSO[] weaponsData { get; private set; }
    [SerializeField] List<WeaponDataSO> weaponUnlocked = new List<WeaponDataSO>();
    [SerializeField] List<WeaponDataSO> weaponsInGame = new List<WeaponDataSO>();
    public GameObject windowSelector;

    #region Boss Health Bar
    [Header("Boss Health Bar")]
    [SerializeField] GameObject bossHealthBarHolder;
    [SerializeField] Animator bossHealthBarAnimator;
    [SerializeField] Image bossHealthBar;
    void DisabelHealthBar()
    {
        gameObject.SetActive(false);
    }

    public void UpdateBossHealthBar(float _value)
    {
        if (!bossHealthBarHolder.activeInHierarchy)bossHealthBarHolder.SetActive(true);
        bossHealthBar.fillAmount = _value;
    }

    public void BossDeath()
    {
        DisabelHealthBar();
        bossHealthBarAnimator.SetTrigger("BossDeath");
    }
    #endregion

    [Header("SpawnPoints")]
    [SerializeField] TeleportController[] allSavePoints;
    public Vector2 GetTeleportPositionByName(string _zoneName)
    {
        for (int i = 0; i < allSavePoints.Length; i++)
        {
            if (allSavePoints[i].shopID.zoneName == _zoneName)
                return allSavePoints[i].transform.position;
        }
        return Vector2.zero;
    }public int GetTeleportIndexByName(string _zoneName)
    {
        for (int i = 0; i < allSavePoints.Length; i++)
        {
            if (allSavePoints[i].shopID.zoneName == _zoneName)
                return i;
        }
        return 0;
    }
    [field: SerializeField] public TpEntity currentSpawnPosition { get; private set; }

    [field: SerializeField] public List<TpEntity> activeSavePoints { get; private set; } = new List<TpEntity>();
    [SerializeField] Door[] doors;
    [SerializeField] string currentSceneName;
    [field: SerializeField] public Transform playerSavePosition { get; private set; }
    public void HideInfoMinimap(TpEntity _entity)
    {
        MinimapUIManager.instance.ShowInfo(_entity);
    }
    
    public void ActivePauseHolder()
    { 
        pauseHolder.SetActive(true);
        ChangeState(GameState.UI);
    }
    public void  TeleportPlayerTo(string _sceneName)
    { 
        
        //SaveManager.SaveZoneSpawnName(target.sceneName);
        SceneManager.LoadScene(_sceneName);
       // SceneManager.LoadScene("Main");
    }
    public void ResetLevel()
    { 
       // SceneManager.LoadScene("TransitionScene");
        SceneManager.LoadScene(currentSceneName);
    }
    public Vector2 GetSavePositionBySavedZoneName()
    {
        if (PlayerPrefs.HasKey("DoorName"))
        {
            for (int i = 0; i < doors.Length; i++)
            {
                if (doors[i].doorName != PlayerPrefs.GetString("DoorName")) 
                    continue;
                PlayerPrefs.DeleteKey("DoorName");
                return (Vector2)doors[i].pos.position;
            }
        }
        TeleportPlayerAnimController.instance.PlayTeleportAnim();
        if (playerSavePosition != null)
            return playerSavePosition.position;

        /*
        string _zoneName = SaveManager.GetZoneSpawnName();
        for (int i = 0; i < allSavePoints.Length; i++)
        {
            if (allSavePoints[i].shopID.zoneName == _zoneName)
                return allSavePoints[i].shopID.pos.position;
        }
        */
        return newGameStartPosition.position;
    }
    public void AddSavePoint(TpEntity _point)
    {
        SaveManager.SaveSceneSpawnName(_point.sceneName);
        //currentSpawnPosition = _point;

        for (int i = 0; i < activeSavePoints.Count; i++)
        {
            if (activeSavePoints[i].zoneName == _point.zoneName)
                return;
        }
        //if (activeSavePoints.Contains(_point)) return;
        SaveManager.SaveZoneUnlocked(_point.zoneName);
        activeSavePoints.Add(_point);   
    }
    void CheckForSavePoints()
    {
        for (int i = 0; i < allSavePoints.Length; i++)
        {
            if (SaveManager.IsZoneUnlocked(allSavePoints[i].shopID.zoneName))
                activeSavePoints.Add(allSavePoints[i].shopID);
        }
    }
    public WeaponDataSO GetWeaponDataAtIndex(int idx)
    {
        if (idx >= weaponUnlocked.Count || idx < 0)
            return null;
        return weaponUnlocked[idx];
    }
    public int GetTotalWeapons()
    {
        return weaponUnlocked.Count;
    }
    void CkeckForAvailableWepons()
    {
        foreach (WeaponDataSO weapon in weaponsData)
        {
            if (SaveManager.IsWeaponSaved(weapon))
                weaponUnlocked.Add(weapon);
        }
    }
    public void AddWeaponDataToInventory(WeaponDataSO _data)
    {
        SaveManager.SaveWeapon(_data);
        weaponUnlocked.Add(_data);
    }
    public void AddWeaponOnGame(WeaponDataSO _data)
    {
        if (!weaponsInGame.Contains(_data))
            weaponsInGame.Add(_data);
    }
    public void RemoveWeaponOnGame(WeaponDataSO _data)
    {
        if (weaponsInGame.Contains(_data))
            weaponsInGame.Remove(_data);
    }
    public bool IsWeaponInGame(WeaponDataSO _data)
    {
        return weaponsInGame.Contains(_data);
    }

    // Devuelve el arma inicial que el jugador tenía guardada, o null si no hay ninguna
    public WeaponDataSO GetFirstInitWeapon()
    {
        string waeaponName = SaveManager.GetFirstWeaponName();

        foreach (WeaponDataSO weapon in weaponsData)
        {
            if (weapon.Name != waeaponName) continue;
            AddWeaponOnGame(weapon);
            return weapon;
        }
        return null;
    }
    public WeaponDataSO GetSecondInitWeapon()
    {
        string waeaponName = SaveManager.GetSecondWeaponName();

        foreach (WeaponDataSO weapon in weaponsData)
        {
            if (weapon.Name != waeaponName) continue;
            AddWeaponOnGame(weapon);
            return weapon;
        }
        return null;
    }

    [Header("Abilities Unlock Holders")]
    [SerializeField] GameObject hookHolder;
    [SerializeField] GameObject dashHolder;
    [SerializeField] GameObject climbHolder;
    public void ActiveAbilityHolder(string _holdername)
    {
        switch (_holdername)
        {
            case "hook":
                hookHolder.SetActive(true);
                break;
            case "climb": 
                climbHolder.SetActive(true);
                break;
            case "dash": 
                dashHolder.SetActive(true);
                break;
        }
    }

    public void HideHolders()
    {
        ChangeState(GameState.Gameplay);
        pausePrincipalHolder.SetActive(true);
        foreach (GameObject holder in holderToHideWithEsc)
        {
            holder.SetActive(false);
        }
    }
    public void HealthBuff()
    {
        if (healthLevel >= 3) return;
        healthLevel++;
        SaveManager.SaveHealthLevel(healthLevel);
        stats.UpdateHealthLevel(healthLevel - 1);
        healthAnimator.SetInteger("Level", healthLevel);
        if (statsBackgroundAnimator.GetInteger("Level") < healthLevel)
            statsBackgroundAnimator.SetInteger("Level", healthLevel);
    }
    public void StaminaBuff()
    {
        if (staminaLevel >= 3) return;
        staminaLevel++;
        SaveManager.SaveStaminaLevel(staminaLevel);
        stats.UpdateStaminaLevel(staminaLevel - 1);
        staminaAnimator.SetInteger("Level", staminaLevel);
        if (statsBackgroundAnimator.GetInteger("Level") < staminaLevel)
            statsBackgroundAnimator.SetInteger("Level", staminaLevel);
    }
    public void InitStatsWithPlayerPrefs()
    {
        healthLevel = SaveManager.GetHealthLevel();
        staminaLevel = SaveManager.GetStaminaLevel();
        stats.UpdateHealthLevel(healthLevel - 1);
        stats.UpdateStaminaLevel(staminaLevel - 1);
        healthAnimator.SetInteger("Level", healthLevel);
        staminaAnimator.SetInteger("Level", staminaLevel);
        statsBackgroundAnimator.SetInteger("Level", Math.Max(healthLevel, staminaLevel));
    }
    public void UpdateHealthBar(float _value)
    {
        healthBar.fillAmount = 1 - _value;
    }
    public void UpdateStaminaBar(float _value)
    {
        staminaBar.fillAmount = 1 - _value;
    }



    public void ActiveShop(TpEntity _spawn)
    {
        AddSavePoint(_spawn);
        currentSpawnPosition = _spawn;
        shopHolder.SetActive(true);
        windowSelector.SetActive(true);
        SetInfoMenuComputer("Shop");
        ChangeState(GameState.UI);
    }
    [Header("Title Colors")]
    [SerializeField] Image titleBackgroundImage;
    [SerializeField] Image titleBorderImage01, titleBorderImage02;
    [SerializeField]TMPro.TMP_Text titleText;

    [SerializeField] Color shopBackgroundColor, inventoryBackgroundColor, statsBackgroundColor;
    [SerializeField] Color shopBorderColor, inventoryBorderColor, statsBorderColor;
    public void SetInfoMenuComputer(string _menuTitle)
    {
        titleText.SetText(_menuTitle);
        switch (_menuTitle)
        {
            case "Shop":
                titleBorderImage01.color = shopBorderColor;
                titleBorderImage02.color = shopBorderColor;
                titleBackgroundImage.color = shopBackgroundColor;

                return;
            case "Stats":
                titleBorderImage01.color = statsBorderColor;
                titleBorderImage02.color = statsBorderColor;
                titleBackgroundImage.color = statsBackgroundColor;

                return;
            case "Inventory":
                titleBorderImage01.color = inventoryBorderColor;
                titleBorderImage02.color = inventoryBorderColor;
                titleBackgroundImage.color = inventoryBackgroundColor;

                return;
            default:
                return;
        }
    }

    [Header("Minimap")]
    [SerializeField] GameObject minimapHolder;
    public void ToggleMinimap(bool _active)
    {
        ChangeState(GameState.UI);
        minimapHolder.SetActive(_active);
    }

    OnEnableFirstButton[] onEnableFirstButtons; 
    public void OnSelectedButtonDisabled()
    {
        onEnableFirstButtons = FindObjectsOfType<OnEnableFirstButton>();
        foreach (OnEnableFirstButton button in onEnableFirstButtons)
        {
            if (button.gameObject.activeSelf)
                button.SelectButton();
        }
    }

    #region Dialogue
    [Header("Dialogue UI")]
    [SerializeField] GameObject dialogueHolder;
    public void ActiveDialogueWindow( )
    {
        dialogueHolder.SetActive(true);
    }


    #endregion





    #endregion


    public void LoadNextLevel()
    {
        SceneManager.LoadScene("SecondLevel");
    }

    

    #endregion

    #region Prone UI Weapons Manager
    // Evento que se dispara cuando cambia el estado del juego
    public event Action<GameState> OnGameStateChanged;

    // Estado actual del juego, inicializado en Gameplay
    private GameState currentGameState = GameState.Gameplay;
    public GameState GetCurrentGameState=> currentGameState;
    // Método para cambiar de estado
    public void ChangeState(GameState state)
    {
        // Si ya estamos en el estado solicitado, no hacemos nada
        if (state == currentGameState)
            return;

        // Dependiendo del nuevo estado, ejecutamos acciones específicas
        switch (state)
        {
            case GameState.UI:
                //EnterUIState();
                break;
            case GameState.Gameplay:
               // EnterGameplayState();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

        // Actualizamos el estado actual y notificamos a los suscriptores
        currentGameState = state;
        OnGameStateChanged?.Invoke(currentGameState);
    }

    // Acciones específicas al entrar en el estado de UI
    private void EnterUIState()
    {
        // Pausa el juego
        Time.timeScale = 0f;
    }

    // Acciones específicas al entrar en el estado de Gameplay
    private void EnterGameplayState()
    {
        // Reanuda el juego
        Time.timeScale = 1f;
    }

    // Enumeración que define los diferentes estados posibles del juego
    public enum GameState
    {
        UI,
        Gameplay
    }
    #endregion

    #region Items
    public List<GameObject> itemList;
    public GameObject GetItemByIndex(int idx) { return itemList[idx] != null ? itemList[idx] : null; }
    //[Range(1, 10)]
    [field: SerializeField, Range(1, 10f)] public float itemTimeRespawn { get; private set; }

    #endregion


    #region Online

    LevelData levelData;
    [SerializeField] Image loadImage;
    [SerializeField] TMP_Text loadText;
    [SerializeField] string[] startCount;
    [SerializeField] UnityEvent StartGameEvent;
    [SerializeField] GameObject[] items;
    public Sprite[] itemsSprites;
    public Transform cursor;
    public static GameManager instance;

    public PlayerObjectController localPlayerController;
    public GameObject localPlayerObject;
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
        else { Destroy(gameObject); }
       // DontDestroyOnLoad(gameObject);
    }
    public void Start()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible=false;
        //  SaveManager.DeleteSaved();
        Invoke("ShowFps", 2);
        stats = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Stats>();
        HideHolders();
        CoinsInPlayerPrefs();
        InitStatsWithPlayerPrefs();
        CkeckForAvailableWepons();
        CheckForSavePoints();

        foreach (GameObject obj in objectsToDelayActivation)
        {
            obj.SetActive(true);
        }

        //levelData= Helpers.GetCurrentLevel();
        //AudioManager.instance.PlayMusic(levelData.levelMusic);
        //StartCoroutine(StartGame());
    }
    public void SetPlayerSpawns(Transform[] _spawns)
    {
        localPlayerObject.transform.position = _spawns[manager.gamePlayers.IndexOf(localPlayerController)].position;
    }
    public void FindLocalPlayer()
    {
        localPlayerObject = GameObject.Find("LocalGamePlayer");
        localPlayerController = localPlayerObject.GetComponent<PlayerObjectController>();

    }
    /*public void ChangeWeaponSprite(int weaponIdx)
    { 
        localPlayerController.ChangeWeaponIndex(weaponIdx);
    }
    public void ChangeAttacActive(bool attackActive)
    { 
        localPlayerController.ChangeAttackActive(attackActive);
    }
    public void UpdatePlayers()
    {
        foreach (PlayerObjectController player in Manager.gamePlayers)
        {
            player.gameObject.GetComponentInChildren<WeaponComponent>().SetIsAttackActive(player.attackActive);
            player.gameObject.GetComponentInChildren<WeaponSprite>().SetCurrentWeaponSpriteIndex(player.weaponIndex);
        }
    }
    */
    public GameObject RequestRandomItem()
    {
        return Instantiate(items[UnityEngine.Random.Range(0, items.Length)]);
    }
    //Para reaparecer el item en el mapa
    public IEnumerator ReEnableItem(GameObject item)
    {
        item.SetActive(false);
        yield return Helpers.GetWait(5);
        item.SetActive(true);
        yield break;
    }
    IEnumerator StartGame()
    {
        bool isFilled;
        float value;
        float targetFillAmount;
        //Temporizador en UI
        foreach (var count in startCount)
        {
            loadText.text = count;
            if (count == "GO")
            {
                yield return Helpers.GetWait(0.2f);
                StartGameEvent.Invoke();
                yield break;
            }
            isFilled = (loadImage.fillAmount == 1);
            value = isFilled ? -1.5f : 1.5f;
            targetFillAmount = isFilled ? 0 : 1;
            while (loadImage.fillAmount != targetFillAmount)
            {
                loadImage.fillAmount += Time.deltaTime * value;

                yield return Helpers.GetWait(Time.deltaTime);
            }

            loadImage.fillAmount = (loadImage.fillAmount < 0.5f) ? 0 : 1;
            loadImage.fillClockwise = !loadImage.fillClockwise;
        }
    }
#endregion

}
[Serializable]
public struct Door
{
    public string doorName;
    public Transform pos;
}
#region Interfaces
public interface ItemAction
{
    public void Action(PlayerItemManager _playerItemManager);

}
public interface ICollidable
{
    void OnCollision();
}
#endregion
#region enums

public enum STAT
{
    Health,
    Stamina
}
#endregion