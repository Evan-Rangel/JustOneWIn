using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Entity : MonoBehaviour
{
    //Finite State Machine
    public FiniteStateMachine stateMachine;

    //D_Entity 
    public D_Entity entityData;

    //Components
    public Rigidbody2D rb {  get; private set; }
    public Animator animator { get; private set; }
    public GameObject aliveGo {  get; private set; }

    //Movement
    public int facingDirection {  get; private set; }

    private Vector2 velocityWorkspace;

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

    //Virtual Start
    public virtual void Start()//"Virtual" means that this can be redefind in the derived classes
    {
        //Start looking to the right
        facingDirection = 1;

        //Initialice Reference
        aliveGo = transform.Find("Alive").gameObject;
        rb = aliveGo.GetComponent<Rigidbody2D>();
        animator = aliveGo.GetComponent<Animator>();    

        stateMachine = new FiniteStateMachine();//Every entity have his own state machine, that a instance of finite state machine.
    }

    //Virtual Update
    public virtual void Update()
    {
        stateMachine.currentState.LogicUpdate();
    }

    //Virtual FixedUpdate
    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();//Physics in FixedUpdate to avoid frame errors
    }

    //Virtual SetVelocity
    public virtual void SetVelocity(float velocity)
    {
        velocityWorkspace.Set(facingDirection * velocity, rb.velocity.y);
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

    //Virtual Flip
    public virtual void Flip()
    {
        facingDirection *= -1;
        aliveGo.transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    //Virtual OnDrawGizmos
    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.wallCheckDistance));
        Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * entityData.ledgeCheckDistance));
    }
}
