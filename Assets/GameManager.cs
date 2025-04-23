using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using Avocado.Weapons.Components;
using Unity.VisualScripting;
using System;

public class GameManager: MonoBehaviour
{
    #region Prone UI Weapons Manager
    public event Action<GameState> OnGameStateChanged;
    private GameState currentGameState = GameState.Gameplay;

    public void ChangeState(GameState state)
    {
        if (state == currentGameState)
            return;

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

        currentGameState = state;
        OnGameStateChanged?.Invoke(currentGameState);
    }

    private void EnterUIState()
    {
        Time.timeScale = 0f;
    }

    private void EnterGameplayState()
    {
        Time.timeScale = 1f;
    }


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
    [field:SerializeField,Range(1, 10f)] public float itemTimeRespawn { get; private set; }
    
    
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
        return Instantiate( items[UnityEngine.Random.Range(0, items.Length)]);
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
                loadImage.fillAmount += Time.deltaTime * value ;

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