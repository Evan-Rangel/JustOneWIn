using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Entity : MonoBehaviour
{
    //Finite State Machine
    public FiniteStateMachine stateMachine;

    //D_Entity 
    [Header("Base Enemy Data")]
    public D_Entity entityData;

    //Components
    public Rigidbody2D rb {  get; private set; }
    public Animator animator { get; private set; }
    public GameObject aliveGo {  get; private set; }
    public AnimationToStateMachine atsm { get; private set; }

    //Movement
    public int facingDirection {  get; private set; }
    public int lastDamageDirection { get; private set; }

    //Stats
    private float currentHealth;
    private float currentStunResistance;
    private float lastDamageTime;

    //Vectors
    private Vector2 velocityWorkspace;

    //Flags
    protected bool isStunned;
    protected bool isDead;

    //Detectors
    [Header("Detectors")]
    [SerializeField]
    [Tooltip("This will detect a wall to enemy turn around")]
    private Transform wallCheck;
    [SerializeField]
    [Tooltip("This will detect a ledge to enemy avoid fall")]
    private Transform ledgeCheck;
    [SerializeField]
    [Tooltip("This will detect a GameObject with the tag and layer -Player-")]
    private Transform playerCheck;
    [SerializeField]
    [Tooltip("This will detect the ground to do thinks like friction, and stop the enemy")]
    private Transform groundCheck;


    //Virtual Start
    public virtual void Start()//"Virtual" means that this can be redefind in the derived classes
    {
        //Start looking to the right
        facingDirection = 1;
        currentHealth = entityData.maxHealth;
        currentStunResistance = entityData.stunResistance;

        //Initialice Reference
        aliveGo = transform.Find("Alive").gameObject;
        rb = aliveGo.GetComponent<Rigidbody2D>();
        animator = aliveGo.GetComponent<Animator>();    
        atsm = aliveGo.GetComponent<AnimationToStateMachine>();

        stateMachine = new FiniteStateMachine();//Every entity have his own state machine, that a instance of finite state machine.
    }

    //Virtual Update
    public virtual void Update()
    {
        stateMachine.currentState.LogicUpdate();

        animator.SetFloat("yVelocity", rb.velocity.y);

        //Condition that take track of the stunRecovery to indicate that stun is over
        if(Time.time >= lastDamageTime + entityData.stunRecoveryTime)
        {
            ResetStunResistance();
        }
    }

    //Virtual FixedUpdate
    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();//Physics in FixedUpdate to avoid frame errors
    }

    //Virtual SetVelocity Normal
    public virtual void SetVelocity(float velocity)
    {
        velocityWorkspace.Set(facingDirection * velocity, rb.velocity.y);
        rb.velocity = velocityWorkspace;
    }

    //Virtual SetVelocity Angle
    public virtual void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        velocityWorkspace.Set(angle.x * velocity * direction, angle.y * velocity);
        rb.velocity = velocityWorkspace;    
    }

    //Virtual CheckWall
    public virtual bool CheckWall()
    {
        return Physics2D.Raycast(wallCheck.position, aliveGo.transform.right, entityData.wallCheckDistance, entityData.whatIsGround);
    }

    //Virtual CheckLedge
    public virtual bool CheckLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.down, entityData.ledgeCheckDistance, entityData.whatIsGround);
    }

    //Virtual CheckGround
    public virtual bool CheckGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, entityData.groundCheckRadius, entityData.whatIsGround);
    }

    //Virtual CheckPlayerInMinAgroRange
    public virtual bool CheckPlayerInMinAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, aliveGo.transform.right, entityData.minAgroDistance, entityData.whatIsPlayer);
    }

    //Virtual CheckPlayerInMaxAgroRange
    public virtual bool CheckPlayerInMaxAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, aliveGo.transform.right, entityData.maxAgroDistance, entityData.whatIsPlayer);
    }

    //Virtual CheckPlayerInCloseRangeAction
    public virtual bool CheckPlayerInCloseRangeAction()
    {
        return Physics2D.Raycast(playerCheck.position, aliveGo.transform.right, entityData.closeRangeActionDistance, entityData.whatIsPlayer);
    }

    //Virtual DamageHop
    public virtual void DamageHop(float velocity)
    {
        velocityWorkspace.Set(rb.velocity.x, velocity);
        rb.velocity = velocityWorkspace;
    }
    //Virtual ResetStunResistance
    public virtual void ResetStunResistance()
    {
        isStunned = false;
        currentStunResistance = entityData.stunResistance;
    }

    //Virtual Damage
    public virtual void Damage(AttackDetails attackDetails)
    {
        lastDamageTime = Time.time;

        //Damage
        currentHealth -= attackDetails.damageAmount;
        currentStunResistance -= attackDetails.stunDamageAmount;

        DamageHop(entityData.damageHopSpeed);

        Instantiate(entityData.hitParticle, aliveGo.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));

        //--Stun Resistance--//
        //Condition that knoww the place whre recibe the player attack
        if(attackDetails.position.x > aliveGo.transform.position.x)
        {
            lastDamageDirection = -1;
        }
        else
        {
            lastDamageDirection = 1;
        }

        //Condition that check the stun resistance, if is below 0 then is stunned
        if(currentStunResistance <= 0)
        {
            isStunned = true;
        }
        //--End Stun Resistance--//

        //--Dead--//
        if(currentHealth <= 0)
        {
            isDead = true;
        }
        //--End Dead--//
    }

    //Virtual Flip
    public virtual void Flip()
    {
        facingDirection *= -1;
        aliveGo.transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    //Virtual OnDrawGizmos
    public virtual void OnDrawGizmos()
    {
        //Wall Detector
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.wallCheckDistance));
        //Ledge Detector
        Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * facingDirection * entityData.ledgeCheckDistance));
        //Detect Player to Melee Attack Detector
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.minAgroDistance), 0.2f);
        //Detect Player Detector
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.minAgroDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.maxAgroDistance), 0.2f);

    }
}
