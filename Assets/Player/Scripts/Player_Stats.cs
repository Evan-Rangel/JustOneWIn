using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Stats : MonoBehaviour
{
    //Player Stats
    [Header("Stats")]
    [SerializeField]
    private float maxHealth;
    private float currentHealth;

    //Player Particles
    [SerializeField]
    private GameObject deathChunkParticle;
    [SerializeField]
    private GameObject deathBloodParticle;

    //GameManager Reference
    private GameManager gameManager;

    //Start
    private void Start()
    {
        //Initialize Vars
        currentHealth = maxHealth;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    //Awake
    private void Awake()
    {
        
    }

    //Function DecreaseHealth
    public void DecreaseHealth(float amount)
    {
        currentHealth -= amount;

        //Condition that is health is below zero, then kill the player
        if (currentHealth <= 0.0f)
        {
            Die();
        }
    }

    //Funtion Die
    private void Die()
    {
        Instantiate(deathChunkParticle, transform.position, deathChunkParticle.transform.rotation);//Pieces particle
        Instantiate(deathBloodParticle, transform.position, deathBloodParticle.transform.rotation);//Blood particle
        gameManager.Respawn();
        Destroy(gameObject);
    }
}
