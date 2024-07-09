using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //---Player Vars---//
    #region Player States Variables
    //StateMachine
    public PlayerStateMachine StateMachine { get; private set; }
    //Player States
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallGrabState WallGrabState { get; private set; }
    public PlayerWallClimbState WallClimbState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerLedgeClimbState LedgeClimbState { get; private set; }
    public PlayerDashState DashState { get; private set; }

    //Player Data
    [Header("Player Base Data")]
    [SerializeField]
    private PlayerData playerData;
    #endregion

    #region Player Components
    //Animator
    public Animator Animator { get; private set; }
    //PlayerInputs
    public PlayerInputHandler InputHandler { get; private set; }
    //RigidBody 2D
    public Rigidbody2D RB { get; private set; }
    //DashDirection
    public Transform DashDirectionIndicator { get; private set; }
    #endregion

    #region Player Check Transforms
    [Header("Player Detectors")]
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private Transform wallCheck;
    [SerializeField]
    private Transform ledgeCheck;
    #endregion

    #region Player Other Variables
    //Vecotr WorkSpace
    private Vector2 workSpace;//With this we stop using to much memory, avoiding to create a "new Vector2" evry time we need one.
    //Velocity
    public Vector2 CurrentVelocity { get; private set; }//With this we stop using to much memory, avoiding calling the RigidBody2D and asking for the "Y" or "X" velocity.
    //Facing Direction
    public int FacingDirection { get; private set; }
    #endregion
    //-----------------//

    //---Player Functions Basics---//
    #region Unity CallBack Functions
    private void Awake()
    {
        //Create a new StateMachine when awake
        StateMachine = new PlayerStateMachine();

        //Create States
        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
        LandState = new PlayerLandState(this, StateMachine, playerData, "land");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "wallSlide");
        WallGrabState = new PlayerWallGrabState(this, StateMachine, playerData, "wallGrab");
        WallClimbState = new PlayerWallClimbState(this, StateMachine, playerData, "wallClimb");
        WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, "inAir");
        LedgeClimbState = new PlayerLedgeClimbState(this, StateMachine, playerData, "ledgeClimbState");
        DashState = new PlayerDashState(this, StateMachine, playerData, "inAir");

    }

    private void Start()
    {
        //Initialize Aniamtor
        Animator = GetComponent<Animator>();
        //Initialize Inputs
        InputHandler = GetComponent<PlayerInputHandler>();
        //Initialize RigidBody2D
        RB = GetComponent<Rigidbody2D>();
        //Initialize DashIndicator
        DashDirectionIndicator = transform.Find("DashDirectionIndicator");
        //Initialize StateMachine
        StateMachine.Initialize(IdleState);
        //Initialize the direction to right
        FacingDirection = 1;
    }

    private void Update()
    {
        //Current Velocity
        CurrentVelocity = RB.velocity;
        //Update Logics
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        //Fixed Update Physics
        StateMachine.CurrentState.PhysicsUpdate();
    }
    #endregion

    //---Player Functions---//
    #region Player Set Funtions
    public void SetVelocityZero()
    {
        RB.velocity = Vector2.zero;
        CurrentVelocity = Vector2.zero;
    }
    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        workSpace.Set(angle.x * velocity * direction, angle.y * velocity);
        RB.velocity = workSpace;
        CurrentVelocity = workSpace;
    }

    public void SetVelocity(float velocity, Vector2 direction)
    {
        workSpace = direction * velocity;
        RB.velocity = workSpace;
        CurrentVelocity = workSpace;
    }

    public void SetVelocityX(float velocity)
    {
        workSpace.Set(velocity, CurrentVelocity.y);
        RB.velocity = workSpace;
        CurrentVelocity = workSpace;    
    }

    public void SetVelocityY(float velocity)
    {
        workSpace.Set(CurrentVelocity.x, velocity);
        RB.velocity = workSpace;
        CurrentVelocity = workSpace;
    }
    #endregion

    //---Player Check Functions---//
    #region Player Check Functions
    public bool CheckIfGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }

    public bool CheckIfTouchingWall()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }

    public bool CheckIfTouchingLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }

    public bool CheckIfTouchingWallBack()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * -FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }

    public void CheckIfShouldFlip(int xInput)
    {
        //Condition that check th valu of th axe "X" to determinate if should Flip the facedirection
        if(xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }
    #endregion

    //---Player Animation Functions---//
    #region Player Animation Functions
    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
    #endregion

    //---Player Other Functions ---//
    #region Other Player Functions
    public Vector2 DetermineCornerPosition()
    {
        RaycastHit2D xHit = Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
        float xDist = xHit.distance;
        workSpace.Set((xDist + 0.015f) * FacingDirection, 0f);
        //workSpace.Set(xDist * FacingDirection, 0f);
        RaycastHit2D yHit = Physics2D.Raycast(ledgeCheck.position + (Vector3)(workSpace), Vector2.down, ledgeCheck.position.y - wallCheck.position.y, playerData.whatIsGround);
        float yDist = yHit.distance;

        workSpace.Set(wallCheck.position.x + (xDist * FacingDirection), ledgeCheck.position.y - yDist);

        return workSpace;
    }
    private void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
    #endregion
}
