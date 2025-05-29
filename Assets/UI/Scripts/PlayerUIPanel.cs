using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Steamworks;

public class PlayerUIPanel : MonoBehaviour
{
    [SerializeField] TMP_Text nameTxt;
    [SerializeField] GameObject charImage;
    [SerializeField] GameObject powerUpImage;
    [SerializeField] GameObject playerObject;
    [SerializeField] CharacterData[] characters;
    [SerializeField] RawImage playerIcon; 
    PlayerObjectController player;

    private void Awake()
    {
        if (transform.parent!=null)
        {
            DontDestroyOnLoad(transform.parent);
            SetPlayerObject();
            powerUpImage.SetActive(false);
        }
    }
    
   
    public void SetPlayerObject()
    {
        playerObject = GameObject.Find("LocalGamePlayer");
        player= playerObject.GetComponent<PlayerObjectController>();
        nameTxt.text = player.playerName;
        //playerIcon = player.playerIcon;
        playerIcon.texture = player.iconText;
        player.grabItem.AddListener(delegate { GrabItem(); });
        player.useItem.AddListener(delegate { UseItem(); });
        //charImage.GetComponent<Image>().sprite = characters[player.character].skins[player.skinIdx];
    }
    void GrabItem()
    { 
        powerUpImage.SetActive(true);
        powerUpImage.GetComponent<Image>().sprite=player.currentItem.GetComponent<SpriteRenderer>().sprite;
    }
    void UseItem()
    {
        powerUpImage.GetComponent<Image>().sprite=null;
        powerUpImage.SetActive(false);

    }

}
