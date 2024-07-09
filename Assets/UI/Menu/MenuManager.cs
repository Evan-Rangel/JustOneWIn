using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] LevelData[] levels;
    [SerializeField] GameObject levelButtonPrefab;
    [SerializeField] Transform levelsGrid;
    public static MenuManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        { 
            Destroy(this);
        }
    }
    private void Start()
    {
        foreach (LevelData levelD in levels)
        {
            LevelButton _level= Instantiate(levelButtonPrefab).GetComponent<LevelButton>();
            _level.SetLevelButton(levelD);
            _level.transform.parent = levelsGrid;
        }
    }
}
