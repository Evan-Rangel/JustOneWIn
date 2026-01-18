using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.GlobalIllumination;

public class GameManager : MonoBehaviour
{



    #region Local Game
    #region Coins 
    [Header("Coins")]
    public int coins = 0;
    [SerializeField] TMP_Text coinsText;
    public void AddCoin(int _value)
    {
        if (coins == 999)
            return;
        coins+=_value;
        coinsText.text = coins.ToString();
    }
    public void SubstractCoin(int _value)
    { 
        coins -= _value;
        coinsText.text = coins.ToString();
    }



    [SerializeField] GameObject coinPrefab;
    List<GameObject> coinsPool=new List<GameObject>();
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

    [Header("User Interface")]
    [SerializeField] GameObject statsHolder;
    [SerializeField] Animator healthAnimator, staminaAnimator, statsBackgroundAnimator;
    int healthLevel = 1, staminaLevel = 1;
    public void HealthBuff()
    {
        healthLevel++;
        healthAnimator.SetInteger("Level", healthLevel);
        if (statsBackgroundAnimator.GetInteger("Level") < healthLevel)
            statsBackgroundAnimator.SetInteger("Level", healthLevel);

    }
    public void StaminaBuff()
    {
        staminaLevel++;
        staminaAnimator.SetInteger("Level", staminaLevel);
        if (statsBackgroundAnimator.GetInteger("Level") < staminaLevel)
            statsBackgroundAnimator.SetInteger("Level", staminaLevel);
    }





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
                EnterUIState();
                break;
            case GameState.Gameplay:
                EnterGameplayState();
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
        DontDestroyOnLoad(gameObject);
    }
    public void Start()
    {
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
}
public interface ItemAction
{
    public void Action(PlayerItemManager _playerItemManager);

}
public interface ICollidable
{
    void OnCollision();
}
public enum CHARACTERS
{
    Blue
}