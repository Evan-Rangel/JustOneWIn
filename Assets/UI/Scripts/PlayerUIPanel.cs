using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerUIPanel : MonoBehaviour
{
    [SerializeField] TMP_Text nameTxt;
    [SerializeField] GameObject charImage;
    [SerializeField] GameObject powerUpImage;
    [SerializeField] GameObject playerObject;
    [SerializeField] CharacterData[] characters;

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
        PlayerObjectController player= playerObject.GetComponent<PlayerObjectController>();
        nameTxt.text = characters[player.character].cName;
        charImage.GetComponent<Image>().sprite = characters[player.character].skins[player.skinIdx];
    }
}
