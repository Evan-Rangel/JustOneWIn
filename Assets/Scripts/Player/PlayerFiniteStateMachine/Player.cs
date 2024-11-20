using Avocado.Weapons;
using Avocado.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Player : NetworkBehaviour
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
    public PlayerCrouchIdleState CrouchIdleState { get; private set; }
    public PlayerCrouchMoveState CrouchMoveState { get; private set; }

    public PlayerAttackState PrimaryAttackState { get; private set; }
    public PlayerAttackState SecondaryAttackState { get; private set; }

    //Player Data
    [Header("Player Base Data")]
    [SerializeField]
    private PlayerData playerData;
    #endregion

    #region Player Components
    //Core
    public Core Core { get; private set; }
    //Animator
    public Animator Animator { get; private set; }
    //PlayerInputs
    public PlayerInputHandler InputHandler { get; private set; }
    //RigidBody 2D
    public Rigidbody2D RB { get; private set; }
    //DashDirection
    public Transform DashDirectionIndicator { get; private set; }
    //Collider 2D
    public BoxCollider2D MovementCollider { get; private set; }
    #endregion

    #region Player Other Variables
    //Vecotr WorkSpace
    private Vector2 workSpace;//With this we stop using to much memory, avoiding to create a "new Vector2" evry time we need one.

    private Weapon primaryWeapon;
    private Weapon secondaryWeapon;
    #endregion
    //-----------------//

    //---Player Functions Basics---//
    #region Unity CallBack Functions
    private void Awake()
    {
        //Get Core
        Core = GetComponentInChildren<Core>();

        primaryWeapon = transform.Find("PrimaryWeapon").GetComponent<Weapon>();
        secondaryWeapon = transform.Find("SecondaryWeapon").GetComponent<Weapon>();

        primaryWeapon.SetCore(Core);
        secondaryWeapon.SetCore(Core);

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
        CrouchIdleState = new PlayerCrouchIdleState(this, StateMachine, playerData, "crouchIdle");
        CrouchMoveState = new PlayerCrouchMoveState(this, StateMachine, playerData, "crouchMove");
        PrimaryAttackState = new PlayerAttackState(this, StateMachine, playerData, "attack", primaryWeapon, CombatInputs.primary);
        SecondaryAttackState = new PlayerAttackState(this, StateMachine, playerData, "attack", secondaryWeapon, CombatInputs.secondary);
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
        //Initialize BoxCollider2D
        MovementCollider = GetComponent<BoxCollider2D>();
        //Initialize First StateMachine
        StateMachine.Initialize(IdleState);
        GameManager.instance.FindLocalPlayer();

    }

    private void Update()
    {
        if (gameObject.name== "LocalGamePlayer")
        { 
            //Core Update
            Core.LogicUpdate();
            //Update Logics
            StateMachine.CurrentState.LogicUpdate();
        }
    }

    private void FixedUpdate()
    {
        //Fixed Update Physics
        if (gameObject.name == "LocalGamePlayer")

            StateMachine.CurrentState.PhysicsUpdate();
    }
    #endregion

    //---Player Animation Functions---//
    #region Player Animation Functions
    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
    #endregion

    //---Player Other Functions ---//
    #region Other Player Functions
    public void SetColliderHeight(float height)
    {
        Vector2 center = MovementCollider.offset;
        workSpace.Set(MovementCollider.size.x, height);

        center.y += (height - MovementCollider.size.y) / 2;

        MovementCollider.size = workSpace;
        MovementCollider.offset = center;
    }
    #endregion
}
