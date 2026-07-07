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
    //[SerializeField] TMPro.TMP_Text fpsText;
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

        CanvasManager.instance.SetFpsText(Mathf.RoundToInt(fps).ToString());
        //fpsText.text = Mathf.RoundToInt(fps).ToString();
        Invoke("ShowFps", 0.1f);

    }
    #endregion
    #region Local Game
    Stats stats;
    public Transform newGameStartPosition;
    #region Coins 
    [Header("Coins")]
    public int coins;
    //[SerializeField] TMP_Text coinsText;
    public void AddCoin(int _value)
    {
        if (coins == 999)
            return;
        coins += _value;
        SaveManager.SaveCoins(coins);
        CanvasManager.instance.SetCoinsText(coins.ToString());
       // coinsText.text = coins.ToString();
    }
    public bool SubstractCoin(int _value)
    {
        if (coins - _value < 0)
            return false;
        coins -= _value;
        SaveManager.SaveCoins(coins);
        CanvasManager.instance.SetCoinsText(coins.ToString());

        //coinsText.text = coins.ToString();
        return true;
    }
    void CoinsInPlayerPrefs()
    {
        coins = SaveManager.GetCoins();
        CanvasManager.instance.SetCoinsText(coins.ToString());

        //coinsText.text = coins.ToString();
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
   // [SerializeField] GameObject statsHolder;
    //[SerializeField] GameObject pauseHolder;
    //[SerializeField] GameObject[] holderToHideWithEsc;
    //[field: SerializeField] public GameObject shopHolder { get; private set; }
    //[SerializeField] Animator healthAnimator, staminaAnimator, statsBackgroundAnimator;
   // [SerializeField] Image healthBar, staminaBar;
    //[SerializeField] GameObject[] healtBarRecoveryImages;
    int healthLevel = 1, staminaLevel = 1, healthRecoverLevel=1;
    //[SerializeField] ShopItemHolder[] shopItemHolders;
    [field: SerializeField] public WeaponDataSO[] weaponsData { get; private set; }
    [SerializeField] List<WeaponDataSO> weaponUnlocked = new List<WeaponDataSO>();
    [SerializeField] List<WeaponDataSO> weaponsInGame = new List<WeaponDataSO>();
    //public GameObject windowSelector;

    /* #region Boss Health Bar
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
     #endregion*/

    [Header("SpawnPoints")]
    public TeleportManager currentTeleportManager;
    //[SerializeField] TeleportController[] allSavePoints;
    public Vector2 GetTeleportPositionByName(string _zoneName)
    {
        for (int i = 0; i < currentTeleportManager.allSavePoints.Length; i++)
        {
            if (currentTeleportManager.allSavePoints[i].shopID.zoneName == _zoneName)
                return currentTeleportManager.allSavePoints[i].transform.position;
        }
        return Vector2.zero;
    }public int GetTeleportIndexByName(string _zoneName)
    {
        for (int i = 0; i < currentTeleportManager.allSavePoints.Length; i++)
        {
            if (currentTeleportManager.allSavePoints[i].shopID.zoneName == _zoneName)
                return i;
        }
        return 0;
    }
    [field: SerializeField] public TpEntity currentSpawnPosition { get; private set; }

    [field: SerializeField] public List<TpEntity> activeSavePoints { get; private set; } = new List<TpEntity>();
    public DoorsManager currentDoorManager;
    [SerializeField] string currentSceneName;
    [field: SerializeField] public Transform playerSavePosition { get; private set; }
    public void SetPlayerSavePosition(Transform _newPosition)
    { 
        playerSavePosition = _newPosition;
    }
    public void HideInfoMinimap(TpEntity _entity)
    {
        MinimapUIManager.instance.ShowInfo(_entity);
    }
    
    public void ActivePauseHolder()
    { 
        CanvasManager.instance.TogglePauseHolder(true);
        ChangeState(GameState.UI);
    }
    public void  TeleportPlayerTo(string _sceneName)
    { 
        //Debug.Log("Teleporting to scene: " + _sceneName);
        //SaveManager.SaveZoneSpawnName(target.sceneName);
        SceneManager.LoadScene(_sceneName);
    }
    public void ResetLevel()
    { 
       // SceneManager.LoadScene("TransitionScene");
        SceneManager.LoadScene(currentSceneName);
    }
    #region Save Points Logic
    
    public Vector2 GetSavePositionBySavedZoneName()
    {
        if (PlayerPrefs.HasKey("DoorName"))
        {
            for (int i = 0; i < currentDoorManager.doors.Length; i++)
            {
                if (currentDoorManager.doors[i].doorName != PlayerPrefs.GetString("DoorName")) 
                    continue;
                PlayerPrefs.DeleteKey("DoorName");
                return (Vector2)currentDoorManager.doors[i].pos.position;
            }
        }
        Debug.Log("a");
        if (playerSavePosition != null)
        { 
            TeleportPlayerAnimController.instance.PlayTeleportAnim();
            return playerSavePosition.position;
        }
        Debug.Log("b");

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
    public void CheckForSavePoints(TeleportManager _newTeleportManager)
    {
        currentTeleportManager = _newTeleportManager;
        CanvasManager.instance.SetMinimapTeleports(_newTeleportManager);
        for (int i = 0; i < currentTeleportManager.allSavePoints.Length; i++)
        {
            if (SaveManager.IsZoneUnlocked(currentTeleportManager.allSavePoints[i].shopID.zoneName) && !activeSavePoints.Contains(currentTeleportManager.allSavePoints[i].shopID))
                activeSavePoints.Add(currentTeleportManager.allSavePoints[i].shopID);
        }
    }
    #endregion


    #region Weapon Logic
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
    #endregion



    public void HideHolders()
    {
        ChangeState(GameState.Gameplay);
        CanvasManager.instance.HideHolders();
    }

    #region Buffs Logic
    public void HealthBuff()
    {
        if (healthLevel >= 3) return;
        healthLevel++;
        SaveManager.SaveHealthLevel(healthLevel);
        stats.UpdateHealthLevel(healthLevel - 1);
        CanvasManager.instance.SetHealthAnimationLevel(healthLevel);

        /*healthAnimator.SetInteger("Level", healthLevel);
        if (statsBackgroundAnimator.GetInteger("Level") < healthLevel)
            statsBackgroundAnimator.SetInteger("Level", healthLevel);
        */
    }
    public void StaminaBuff()
    {
        if (staminaLevel >= 3) return;
        staminaLevel++;
        SaveManager.SaveStaminaLevel(staminaLevel);
        stats.UpdateStaminaLevel(staminaLevel - 1);
        CanvasManager.instance.SetStaminaAnimationLevel(staminaLevel);
       /* staminaAnimator.SetInteger("Level", staminaLevel);
        if (statsBackgroundAnimator.GetInteger("Level") < staminaLevel)
            statsBackgroundAnimator.SetInteger("Level", staminaLevel);
    */
    }
    public void HealthRecoverBuff()
    {
        if (healthRecoverLevel >= 4) return;
    
        healthRecoverLevel++;
        SaveManager.SaveHealthRecoverLevel(healthRecoverLevel);
        stats.UpdateHealthRecoveryLevel(healthRecoverLevel );
    }
    public void InitStatsWithPlayerPrefs()
    {
        healthLevel = SaveManager.GetHealthLevel();
        staminaLevel = SaveManager.GetStaminaLevel();
        healthRecoverLevel = SaveManager.GetHealthRecoverLevel();
        stats.UpdateHealthLevel(healthLevel - 1);
        stats.UpdateStaminaLevel(staminaLevel - 1);
        stats.UpdateHealthRecoveryLevel(healthRecoverLevel);
        CanvasManager.instance.SetHealthAnimationLevel(healthLevel);
        CanvasManager.instance.SetStaminaAnimationLevel(staminaLevel);

        //healthAnimator.SetInteger("Level", healthLevel);
        //staminaAnimator.SetInteger("Level", staminaLevel);
        //statsBackgroundAnimator.SetInteger("Level", Math.Max(healthLevel, staminaLevel));
    }
    /*
    public void UpdateHealthBar(float _value)
    {
        healthBar.fillAmount = 1 - _value;
    }
    public void UpdateStaminaBar(float _value)
    {
        staminaBar.fillAmount = 1 - _value;
    }
    public void UpdateHealthRecoveryImages(float _Value)
    {
        for (int i = 0; i < healtBarRecoveryImages.Length; i++)
        {
            if (i < (int)_Value)
            {
                healtBarRecoveryImages[i].SetActive(true);
                continue ;
            }
            healtBarRecoveryImages[i].SetActive(false);
        }
    }*/



    public void ActiveShop(TpEntity _spawn)
    {
        AddSavePoint(_spawn);
        currentSpawnPosition = _spawn;
        CanvasManager.instance.ActiveShop();
        
        ChangeState(GameState.UI);
    }
    #endregion
    
    
    /*
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
    }*/


    #endregion



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

    [SerializeField] GameObject[] dontDestroyOnLoadObjects;
    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
        for (int i = 0; i < dontDestroyOnLoadObjects.Length; i++)
        {
            DontDestroyOnLoad(dontDestroyOnLoadObjects[i]);
        }
        DontDestroyOnLoad(gameObject);
    }
    public void Start()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        Invoke("ShowFps", 2);
        stats = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Stats>();
        HideHolders();
        CoinsInPlayerPrefs();
        InitStatsWithPlayerPrefs();
        CkeckForAvailableWepons();
        //CheckForSavePoints();

    }
   
    //desuso
    #region Online

    LevelData levelData;
    [Space]
    [Space]
    [Space]

    [Header("Desuso")]
    [Space]
    [SerializeField] GameObject[] items;
    public Sprite[] itemsSprites;
    public static GameManager instance;


 
   
  
    public GameObject RequestRandomItem()
    {
        return Instantiate(items[UnityEngine.Random.Range(0, items.Length)]);
    }

    #endregion
    #region Items
    public List<GameObject> itemList;
    public GameObject GetItemByIndex(int idx) { return itemList[idx] != null ? itemList[idx] : null; }
    //[Range(1, 10)]
    [field: SerializeField, Range(1, 10f)] public float itemTimeRespawn { get; private set; }

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
    Stamina,
    HealthRecover
}
#endregion