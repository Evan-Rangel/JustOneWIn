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
    //Characters
    [SerializeField] CharacterData[] characters;
    public int levelId;
    private void Start()
    {
        levelButton.onClick.AddListener(delegate { AudioManager.instance.PlayOneShotSFX(buttonSound); });
    }
    
    public void SetVotes(int charIdx, int skinIdx)
    {
        Debug.Log("SI");
        foreach (Image image in playerImages)
        {
            if (image.sprite != null)
                continue;
           // image.gameObject.SetActive(true);
            image.sprite= characters[charIdx].skins[skinIdx];
        }
    }
    public void SetLevelButton(LevelData _data)
    {
        mapImage.sprite = _data.levelSprite;
        levelLabel.text = _data.levelName;
        levelId= _data.levelId;
        for (int i = 0; i < FindObjectsOfType<PlayerObjectController>().Length; i++)
        {
            GameObject image=Instantiate(imagePrefab, parent:playerGrid.transform);
            playerImages.Add(image.GetComponent<Image>());
            //image.SetActive(false);
        }
        levelButton.onClick.AddListener(delegate { LevelSelectorController.instance.ChoiceLevel(levelId); });
    }
}
