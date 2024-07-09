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
    [SerializeField] TMP_Text levelLavel;
    [SerializeField] Button levelButton;
    public void SetLevelButton(LevelData _data)
    {
        levelButton.onClick.AddListener(delegate { _data.StartLevel(); });
        mapImage.sprite = _data.levelSprite;
        levelLavel.text = _data.levelName;
    }
}
