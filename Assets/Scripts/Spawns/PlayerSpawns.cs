using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSpawns : MonoBehaviour
{
    public Transform[] spawns { get; private set; }

    private void Start()
    {
        spawns = GetComponentsInChildren<Transform>();
        GameManager.instance.SetPlayerSpawns(spawns);
    }

}
