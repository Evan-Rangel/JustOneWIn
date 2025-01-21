using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
public class LevelButton : MonoBehaviour
{
    [SerializeField] Image mapImage;
    [SerializeField] TMP_Text levelLabel;
    [SerializeField] Button levelButton;
    [SerializeField] AudioClip buttonSound;
    [SerializeField] List<Image> playerImages;
    [SerializeField] GameObject playerGrid;
    [SerializeField] GameObject imagePrefab;
    public List<int> playersID= new List<int>();
    //Characters
    [SerializeField] CharacterData[] characters;
    public int levelId;
    public string levelName;
    private void Start()
    {
        levelButton.onClick.AddListener(delegate { AudioManager.instance.PlayOneShotSFX(buttonSound); });
    }
    public void DisableButton()
    { 
        levelButton.interactable = false;
    }
    public void SetVotes(int charIdx, int skinIdx, int playerID)
    {
        foreach (Image image in playerImages)
        {
            if (playersID.Contains(playerID))
                break;
            if (image.sprite != null)
                continue;
            //image.sprite = characters[charIdx].skins[skinIdx];
            playersID.Add(playerID); ;
            break;
        }
    }
    public void SetLevelButton(LevelData _data)
    {
        mapImage.sprite = _data.levelSprite;
        levelLabel.text = _data.levelName;
        levelName= _data.levelName;
        levelId = _data.levelId;
        foreach (PlayerObjectController player in FindObjectsOfType<PlayerObjectController>())
        {
            GameObject image = Instantiate(imagePrefab, parent: playerGrid.transform);
            playerImages.Add(image.GetComponent<Image>());
        }
        levelButton.onClick.AddListener(delegate { LevelSelectorController.instance.ChoiceLevel(levelId); });
    }
}
