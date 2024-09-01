using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManagerLevel : MonoBehaviour
{
    //NOTE: Simple respawn Mechanic, not the real version, just something to test things

    //Respawn
    [Header("Respawn")]
    [SerializeField]
    private Transform respawnPoint;
    [SerializeField]
    private float respawnTime;
    private float respawnStartTime;
    private bool respawn;
    [SerializeField]
    private GameObject player;

    //Cinemachine
    private CinemachineVirtualCamera CVC;

    //Start
    private void Start()
    {
        CVC = GameObject.Find("Player Camera").GetComponent<CinemachineVirtualCamera>();
    }

    //Awake
    private void Awake()
    {

    }

    //Update
    private void Update()
    {
        //Check for player respawn
        CheckRespawn();
    }

    //Function Respawn
    public void Respawn()
    {
        respawnTime = Time.time;
        respawn = true;
    }

    //Function CheckRespawn
    private void CheckRespawn()
    {
        //Condition that if the enough time has pass, then respawn
        if (Time.time <= respawnStartTime + respawnTime && respawn)
        {
            var playerTemp = Instantiate(player, respawnPoint);//"var playerTemp" It exists because when the player is destroyed, Cinemachin's camera will lose its target, so I save it in the VAR so I can access it later
            CVC.m_Follow = playerTemp.transform;//Here the camera again go with the respawn player
            respawn = false;
        }
    }
}
