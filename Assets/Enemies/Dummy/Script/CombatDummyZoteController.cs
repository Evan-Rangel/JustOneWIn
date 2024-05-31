using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatDummyZoteController : MonoBehaviour
{
    //DummyZote
    [Header("DummyZote Stats")]
    [SerializeField]
    private float maxHealth;
    private float currentHealth;
    [Header("KnockBack")]
    [SerializeField]
    private bool applyKnockBack;
    private bool knockBack;
    private float knockBackStart;
    [SerializeField]
    private float knockBackSpeedX;
    [SerializeField]
    private float knockBackSpeedY;
    [SerializeField]
    private float knockBackDuration;
    [SerializeField]
    private float knockBackDeathSpeedX;
    [SerializeField]
    private float knockBackDeathSpeedY;
    [SerializeField]
    private float deathTorque;

    //DummyZote Reference Parts
    private GameObject aliveGO;
    private GameObject brokenTopGO;
    private GameObject brokenBottomGO;
    private Rigidbody2D rbAlive;
    private Rigidbody2D rbBrokenTop;
    private Rigidbody2D rbBrokenBottom;

    //DummyZote Animator
    private Animator aliveAnim;

    //Player Refrence
    private Player_Controller player_pc;
    private int playerFacingDirection;
    private bool playerOnLeft;
    [SerializeField]
    private GameObject hitParticle;


    //Start
    private void Start()
    {
        //Initialize Vars
        currentHealth = maxHealth;

        //DummyZoteParts Find
        aliveGO = transform.Find("Alive").gameObject;
        brokenTopGO = transform.Find("Broken Top").gameObject;
        brokenBottomGO = transform.Find("Broken Bottom").gameObject;

        //DummyZote Animator
        aliveAnim = aliveGO.GetComponent<Animator>();

        //DummyZoteParts RigidBody2D
        rbAlive = aliveGO.GetComponent<Rigidbody2D>();
        rbBrokenTop = brokenTopGO.GetComponent<Rigidbody2D>();
        rbBrokenBottom = brokenBottomGO.GetComponent<Rigidbody2D>();

        //DummyZoteParts Active
        aliveGO.SetActive(true);
        brokenTopGO.SetActive(false);
        brokenBottomGO.SetActive(false);
    }

    //Awake
    private void Awake()
    {
        //Initialize Vars
        player_pc = GameObject.Find("Player").GetComponent<Player_Controller>();
    }

    //Update
    private void Update()
    {
        //Check the KnockBack
        CheckKnockBack();
    }

    //Function Damage
    private void Damage(float amount)
    {
        currentHealth -= amount;
        playerFacingDirection = player_pc.GetFacingDirection();

        Instantiate(hitParticle, aliveGO.transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));

        //Condition that know the side the player is facing
        if (playerFacingDirection == 1)
        {
            playerOnLeft = true;
        }
        else
        {
            playerOnLeft = false;
        }

        aliveAnim.SetBool("playerOnLeft", playerOnLeft);
        aliveAnim.SetTrigger("damage");

        //Condition that apply the knockback if the Dummy is still alive
        if (applyKnockBack && currentHealth > 0.0f)
        {
            //KnockBack
            KnockBack();
        }

        //Conditions thats know when the dummy is dead
        if (currentHealth <= 0.0f)
        {
            //Dead Zote
            Die();
        }
    }

    //Function KnockBack
    private void KnockBack()
    {
        knockBack = true;  
        knockBackStart = Time.time;
        rbAlive.velocity = new Vector2(knockBackSpeedX * playerFacingDirection, knockBackSpeedY);
    }

    //Function CheckKnockBack
    private void CheckKnockBack()
    {
        //Condition that stop the knockback once certain amount of time has passed
        if (Time.time >= knockBackStart + knockBackDuration && knockBack)
        {
            knockBack = false;
            rbAlive.velocity = new Vector2(0.0f, rbAlive.velocity.y);
        }
    }

    //Function Die
    private void Die()
    {
        //Set values to prepare for the effect
        aliveGO.SetActive(false);
        brokenTopGO.SetActive(true);
        brokenBottomGO.SetActive(true);
        brokenTopGO.transform.position = aliveGO.transform.position;
        brokenBottomGO.transform.position = aliveGO.transform.position;

        //Knockback effects
        rbBrokenBottom.velocity = new Vector2(knockBackSpeedX * playerFacingDirection, knockBackSpeedY);//Same as the Alive part
        rbBrokenTop.velocity = new Vector2(knockBackDeathSpeedX * playerFacingDirection, knockBackDeathSpeedY);//Diferent amount of knockback
        rbBrokenTop.AddTorque(deathTorque * -playerFacingDirection, ForceMode2D.Impulse);
    }
}
