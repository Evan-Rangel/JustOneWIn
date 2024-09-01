using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyController : MonoBehaviour
{
    //Note: The Enemy will work with State Machine, using a buncho of prdefind state that we use to determinate what to do
    //Enemy States
    private enum State
    {
        Moving,
        Knockback,
        Dead
    }
    private State currentState;

    //Enemy Movement
    [Header("Movement")]
    [SerializeField]
    private float movementSpeed;
    private int facingDirection;
    private Rigidbody2D aliveRb;
    private Vector2 movement;

    //Enemy Stats
    [Header("Stats")]
    [SerializeField]
    private float maxHealth;
    private float currentHealth;

    //Enemy Attack/Damage
    [Header("Attack & Damage")]  
    [SerializeField]
    private float touchDamage;   
    [SerializeField]
    private float touchDamageCooldown;
    [SerializeField]
    private float touchDamageWidth;
    [SerializeField]
    private float touchDamageHeight;
    private Vector2 touchDamageBotLeft;
    private Vector2 touchDamageTopRight;
    private float lastTouchDamageTime;
    private float[] attackDetails = new float[2];
    [SerializeField]
    private Transform touchDamageCheck;
    [SerializeField]
    private LayerMask whatIsPlayer;
    private int damageDirection;

    //Enemy Knockback
    [Header("Knockback")]
    [SerializeField]
    private float KnockbackDuration;
    [SerializeField]
    private Vector2 KnockbackSpeed;
    private float knockbackStartTime;

    //Enemy Particles
    [Header("Particles")]
    [SerializeField]
    private GameObject hitParticle;
    [SerializeField]
    private GameObject deathChunkParticle;
    [SerializeField]
    private GameObject deathBloodParticle;

    //Enemy Detectors
    [Header("Detectors")]
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private float groundCheckDistance;
    [SerializeField]
    private Transform wallCheck;
    [SerializeField]
    private float wallCheckDistance;
    [SerializeField]
    private LayerMask whatIsGround;
    private bool groundDetected;
    private bool wallDetected;

    //Enemy Reference
    private GameObject alive;
    private Animator aliveAnim;

    //Start
    private void Start()
    {
        //Initialice Vars
        alive = transform.Find("Alive").gameObject;
        aliveRb = alive.GetComponent<Rigidbody2D>();
        facingDirection = 1;
        currentHealth = maxHealth;
        aliveAnim = alive.GetComponent<Animator>();
    }

    //Awake
    private void Awake()
    {
        //Initialice Vars
        
    }

    //Update
    private void Update()
    {
        switch (currentState)//We are going to determinate what State is Active in this Switch
        {
            case State.Moving:
                UpdateMovingState(); 
                break;
            case State.Knockback:
                UpdateKnockbackState();
                break;
            case State.Dead: 
                UpdateDeadState(); 
                break;       
        }
    }

    //--WALKING STATE--//
    //Function EnterMovingState
    private void EnterMovingState()
    {

    }
    //Function UpdateMovingState
    private void UpdateMovingState()
    {
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);//Ground RayCast
        wallDetected = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);//Wall RayCast

        //Check Damage
        CheckTouchDamage();

        //Condition that is not detecting any ground or wall, turn around
        if(!groundDetected || wallDetected)
        {
            //Flip Enemy
            Flip();
        }
        else
        {
            //Move Enemy
            movement.Set(movementSpeed * facingDirection, aliveRb.velocity.y);
            aliveRb.velocity = movement;
        }
    }
    //Function ExitMovingState
    private void ExitMovingState()
    {

    }

    //--KNOCKBACK STATE--//
    //Function EnterKnockbackState
    private void EnterKnockbackState()
    {
        knockbackStartTime = Time.time;
        movement.Set(KnockbackSpeed.x * damageDirection, KnockbackSpeed.y);
        aliveRb.velocity = movement;
        aliveAnim.SetBool("Knockback", true);
    }
    //Function UpdateKnockbackState
    private void UpdateKnockbackState()
    {
        //Condition that is the time of the Knockback was enough, then change to the State of Moving
        if(Time.time >= knockbackStartTime + KnockbackDuration)
        {
            SwitchState(State.Moving); 
        }
    }
    //Function ExitKnockbackState
    private void ExitKnockbackState()
    {
        aliveAnim.SetBool("Knockback", false);
    }

    //--DEAD STATE--//
    //Function EnterDeadState
    private void EnterDeadState()
    {
        Instantiate(deathChunkParticle, alive.transform.position, deathChunkParticle.transform.rotation);
        Instantiate(deathBloodParticle, alive.transform.position, deathBloodParticle.transform.rotation);
        Destroy(gameObject);
    }
    //Function UpdateDeadState
    private void UpdateDeadState()
    {

    }
    //Function ExitDeadState
    private void ExitDeadState()
    {

    }

    //--OTHER FUNCTIONS--//
    //Function Damage (This is used in "SendMessege")
    private void Damage(float[] attackDetails)
    {
        currentHealth -= attackDetails[0];//0 index of the array is de damage created, de 1 index will be the x postion of the player

        //Hit Particle
        Instantiate(hitParticle, alive.transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));

        //Condition that determinate the facing direction of the player
        if(attackDetails[1] > alive.transform.position.x)
        {
            damageDirection = -1;
        }
        else
        {
            damageDirection = 1;
        }

        //Hit Particles

        //Condition that got the eneny into the Knockback state
        if(currentHealth > 0.0f)//Alive
        {
            SwitchState(State.Knockback);
        }
        else if(currentHealth <= 0.0f)//Dead
        {
            SwitchState(State.Dead);
        }

    }
    //Function CheckTouchDamage
    private void CheckTouchDamage()
    {
        //Conditionthat check the cooldown to know if can damage again
        if(Time.time >= lastTouchDamageTime + touchDamageCooldown)
        {
            //Determining the two corners of the area
            touchDamageBotLeft.Set(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
            touchDamageTopRight.Set(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));
            //Overlap area
            Collider2D hit = Physics2D.OverlapArea(touchDamageBotLeft, touchDamageTopRight, whatIsPlayer);
            //Condition thats detect in the area a player
            if (hit != null)
            {
                lastTouchDamageTime = Time.time;
                attackDetails[0] = touchDamage;
                attackDetails[1] = alive.transform.position.x;
                hit.SendMessage("Damage", attackDetails);
            }
        }

    }

    //Function Flip
    private void Flip()
    {
        facingDirection *= -1;
        alive.transform.Rotate(0.0f, 180.0f, 0.0f);
    }
    //Function SwitchState
    private void SwitchState(State state)
    {
        //Exit Switch
        switch(currentState)//Take care of calling the "Exit" function for the current State
        {
            case State.Moving:
                ExitMovingState(); 
                break;
            case State.Knockback:
                ExitKnockbackState();
                break;
            case State.Dead:
                ExitDeadState();
                break;
        }
        //Enter Switch
        switch (state)//Take care of calling the "Enter" function for the current State
        {
            case State.Moving:
                EnterMovingState();
                break;
            case State.Knockback:
                EnterKnockbackState();
                break;
            case State.Dead:
                EnterDeadState();
                break;
        }

        currentState = state;
    }

    //Function OnDrawGizmos
    private void OnDrawGizmos()
    {
        //GroundCheck
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        //WallCheck
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));

        //Box to check the damage to the player
        Vector2 botLeft = new Vector2(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
        Vector2 botRight = new Vector2(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
        Vector2 topRight = new Vector2(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));
        Vector2 topLeft = new Vector2(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));
        
        //Damage Zone
        Gizmos.DrawLine(botLeft, botRight);
        Gizmos.DrawLine(botRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, botLeft);
    }
}
