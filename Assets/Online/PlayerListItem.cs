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


    public void SetPlayerValues()
    {
        playerNameText.text = playerName;
        charImage.sprite =(data!=null)?data.skins[skinIdx]:null;
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
    [SerializeField] Image charImage;
    [SerializeField] TMP_Text charName;
    [SerializeField] TMP_Text selectButtonText;
    [SerializeField] Button selectButton;
    //backButton is starting in false in the Start function
    //[SerializeField] Button backButton;
    [SerializeField] Button nextButton;
    [SerializeField] Button prevButton;
    [SerializeField] AudioClip clickSound;
    public CharacterData data;
    public int skinIdx = 0;
    int characterIdx = 0;

    public void IsInteractuable(bool value)
    {
        nextButton.interactable = value;
        prevButton.interactable= value;
        selectButton.interactable= value;
    }
    public void StartListeners()
    {
        prevButton.onClick.AddListener(delegate { AudioManager.instance.PlayOneShotSFX(clickSound); });
        prevButton.onClick.AddListener(delegate { GetPrevCharacter(); });
        nextButton.onClick.AddListener(delegate { AudioManager.instance.PlayOneShotSFX(clickSound); }); prevButton.onClick.AddListener(delegate { AudioManager.instance.PlayOneShotSFX(clickSound); });
        nextButton.onClick.AddListener(delegate { GetNextCharacter(); });
        SetCharacterData(LobbyController.instance.RequestCharacterData(characterIdx));
    }
    public void SetCharacterData(CharacterData _data)
    {
        data = _data;
        charName.text = data.cName;
        charImage.sprite = data.skins[0];
        skinIdx = 0;
        //Setting the selector character
        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(delegate { AudioManager.instance.PlayOneShotSFX(clickSound); });
        selectButton.onClick.AddListener(delegate { SelectCharacter(); });
        //Checa con el lobby si el personaje ha sido seleccionado, en dado caso, el boton se bloquea
        selectButton.interactable = !LobbyController.instance.IsCharacterLocked(_data.cCharacter);
    }
    public void SelectCharacter()
    {
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
        //Setting the prev button
        prevButton.onClick.RemoveAllListeners();
        prevButton.onClick.AddListener(delegate { AudioManager.instance.PlayOneShotSFX(clickSound); });
        prevButton.onClick.AddListener(delegate { GetPrevCharacter(); });

        //Setting the next button
        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(delegate { AudioManager.instance.PlayOneShotSFX(clickSound); });
        nextButton.onClick.AddListener(delegate { GetNextCharacter(); });

        //Reset the character data
        SetCharacterData(data);

        //SelectButton
        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(delegate { AudioManager.instance.PlayOneShotSFX(clickSound); });
        selectButton.onClick.AddListener(delegate { SelectCharacter(); });
        selectButtonText.text = "Select";
    }
    public void GetNextCharacter()
    {

        characterIdx = (characterIdx >= LobbyController.instance.GetMaxCharacters-1) ? 0 : characterIdx + 1;
        SetCharacterData(LobbyController.instance.RequestCharacterData(characterIdx));
    }
    public void GetPrevCharacter()
    {
        characterIdx = (characterIdx <= 0) ? LobbyController.instance.GetMaxCharacters - 1 : characterIdx-1;
        SetCharacterData(LobbyController.instance.RequestCharacterData(characterIdx));
    }
    public void GetNextCharacterSkin()
    {
        skinIdx = (skinIdx >= data.skins.Length-1) ? 0 : skinIdx+1;
        charImage.sprite = data.skins[skinIdx];
    }
    public void GetPrevCharacterSkin()
    {
        skinIdx = (skinIdx <= 0) ? data.skins.Length - 1 : skinIdx-1;
        charImage.sprite = data.skins[skinIdx];
    }
}
