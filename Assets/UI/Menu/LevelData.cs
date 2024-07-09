using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level")]

public class LevelData : ScriptableObject
{
    [field: SerializeField] public string levelName { get; private set; }
    [field: SerializeField] public Sprite levelSprite { get; private set; }
    public UnityEvent StartLevelAction;
    
    public void StartLevel()
    {
        SceneManager.LoadScene(levelName);
    }
}
