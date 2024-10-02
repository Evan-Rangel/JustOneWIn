using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Steamworks;

public class PlayerListItem : MonoBehaviour
{

    //Carga de datos de Steam
    public string playerName;
    public int connectionID;
    public ulong playerSteamID;
    private bool avatarReceived;

    public TMP_Text playerNameText;
    public TMP_Text playerReadyText;
    public bool ready;
    public RawImage playerIcon;
    protected Callback<AvatarImageLoaded_t> imageLoaded;

    public void ChangeReadyStatus()
    {
        if (ready)
        {
            playerReadyText.text = "Ready";
            playerReadyText.color = Color.green;
        }
        else
        {
            playerReadyText.text = "Not Ready";
            playerReadyText.color = Color.red;
        }
    }
    private void Start()
    {
        imageLoaded = Callback<AvatarImageLoaded_t>.Create(OnImageLoaded);
    }
    private void Update()
    {
        //Intentar Optimizar esto
        if (isInteractuable && !charLocked)
        {
            selectButton.interactable = !LobbyController.instance.IsCharacterLocked(characterIdx);
            LobbyController.instance.ChangeTestText("Values: " + selectButton.interactable.ToString());
        }
    }

    public void SetPlayerValues()
    {
        playerNameText.text = playerName;
        if (characterIdx<=0)
        {
            characterIdx = characters.Length;
        }if (characterIdx>=characters.Length)
        {
            characterIdx = 0;
        }
        charImage.sprite = characters[characterIdx].skins[skinIdx];
        charName.text = characters[characterIdx].cName;

        ChangeReadyStatus();
        if (!avatarReceived)
        {
            GetPlayerIcon();
        }
    }
    void GetPlayerIcon()
    {
        int ImageID = SteamFriends.GetLargeFriendAvatar((CSteamID)playerSteamID);
        if (ImageID == -1) { return; }
        playerIcon.texture = GetSteamImageAsTexture(ImageID);
    }
    private Texture2D GetSteamImageAsTexture(int iImage)
    {
        Texture2D texture = null;
        bool isValid = SteamUtils.GetImageSize(iImage, out uint width, out uint height);
        if (isValid)
        {
            byte[] image = new byte[width * height * 4];
            isValid = SteamUtils.GetImageRGBA(iImage, image, (int)(width * height * 4));
            if (isValid)
            {
                texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false, true);
                texture.LoadRawTextureData(image);
                texture.Apply();
            }
        }
        avatarReceived = true;
        return texture;
    }
    protected void OnImageLoaded(AvatarImageLoaded_t callback)
    {
        if (callback.m_steamID.m_SteamID == playerSteamID)
        {
            playerIcon.texture = GetSteamImageAsTexture(callback.m_iImage);
        }
        else
        {
            return;
        }
    }



    //Para la seleccion de personaje y skin
    [Space]
    [Space]
    [Space]
    [Header("Seleccion de personaje")]
    [SerializeField] CharacterData[] characters;
    [SerializeField] Image charImage;
    [SerializeField] TMP_Text charName;
    [SerializeField] TMP_Text selectButtonText;
    [SerializeField] Button selectButton;
  
    [SerializeField] Button nextButton;
    [SerializeField] Button prevButton;
    [SerializeField] AudioClip clickSound; 
    public int skinIdx = 0;
    public int characterIdx = 0;
    bool isInteractuable;
    bool charLocked;
    //Desactiva los PlayerListItem que no sean del cliente
    public void IsInteractuable(bool value)
    {
        nextButton.interactable = value;
        prevButton.interactable= value;
        selectButton.interactable= value;
        isInteractuable = value;
    }
    //Agrega listener iniciales.
    public void StartListeners()
    {
        prevButton.onClick.AddListener(delegate { AudioManager.instance.PlayOneShotSFX(clickSound); });
        prevButton.onClick.AddListener(delegate { GetPrevCharacter(); });
        nextButton.onClick.AddListener(delegate { AudioManager.instance.PlayOneShotSFX(clickSound); }); 
        nextButton.onClick.AddListener(delegate { GetNextCharacter(); });

        SetCharacterData(characterIdx);
    }

    public void SetCharacterData(int _data)
    {
        //data = _data;
        charName.text = characters[_data].cName;
        charImage.sprite = characters[_data].skins[0];
        skinIdx = 0;
        //Setting the selector character
        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(delegate { AudioManager.instance.PlayOneShotSFX(clickSound); });
        selectButton.onClick.AddListener(delegate { SelectCharacter(); });
    }
    public void SelectCharacter()
    {
        charLocked = true;
        LobbyController.instance.LockCharacter(true);
        //Setting the prev button
        prevButton.onClick.RemoveAllListeners();
        prevButton.onClick.AddListener(delegate { AudioManager.instance.PlayOneShotSFX(clickSound); });
        prevButton.onClick.AddListener(delegate { GetPrevCharacterSkin(); });
        Debug.Log("Select");
        //Setting the next button
        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(delegate { AudioManager.instance.PlayOneShotSFX(clickSound); });
        nextButton.onClick.AddListener(delegate { GetNextCharacterSkin(); });

        //SelectButton
        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(delegate { AudioManager.instance.PlayOneShotSFX(clickSound); });
        selectButton.onClick.AddListener(delegate { BackToSelectCharacter(); });

        selectButtonText.text = "Cancel";
    }
    public void BackToSelectCharacter()
    {
        charLocked = false;

        LobbyController.instance.LockCharacter(false);
        //Setting the prev button
        prevButton.onClick.RemoveAllListeners();
        prevButton.onClick.AddListener(delegate { AudioManager.instance.PlayOneShotSFX(clickSound); });
        prevButton.onClick.AddListener(delegate { GetPrevCharacter(); });

        //Setting the next button
        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(delegate { AudioManager.instance.PlayOneShotSFX(clickSound); });
        nextButton.onClick.AddListener(delegate { GetNextCharacter(); });

        //SelectButton
        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(delegate { AudioManager.instance.PlayOneShotSFX(clickSound); });
        //selectButton.onClick.AddListener(delegate { LobbyController.instance.LockCharacter(characterIdx); });
        selectButton.onClick.AddListener(delegate { SelectCharacter(); });

        selectButtonText.text = "Select";
    }
    public void GetNextCharacter()
    {
        characterIdx = (characterIdx >= characters.Length - 1) ? 0 : characterIdx + 1;
        SetCharacterData(characterIdx);
        LobbyController.instance.ChangeCharacter(characterIdx);
    }
    public void GetPrevCharacter()
    {
        characterIdx = (characterIdx <= 0) ? characters.Length - 1 : characterIdx-1;
        SetCharacterData(characterIdx);
        LobbyController.instance.ChangeCharacter( characterIdx); 
    }
    public void GetNextCharacterSkin()
    {
        skinIdx = (skinIdx >= characters[characterIdx].skins.Length-1) ? 0 : skinIdx+1;
        charImage.sprite = characters[characterIdx].skins[skinIdx];
        LobbyController.instance.ChangeSkin(skinIdx);
    }
    public void GetPrevCharacterSkin()
    {
        skinIdx = (skinIdx <= 0) ? characters[characterIdx].skins.Length - 1 : skinIdx-1;
        charImage.sprite = characters[characterIdx].skins[skinIdx];
        LobbyController.instance.ChangeSkin(skinIdx);
    }
}
