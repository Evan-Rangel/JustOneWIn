using System;
using System.Collections;
using System.Collections.Generic;
using Avocado.CoreSystem;
using Avocado.FSM;
using Avocado.Weapons;
using UnityEngine;
using UnityEngine.Rendering;

/*---------------------------------------------------------------------------------------------
Este script define el comportamiento base del jugador y gestiona su lógica mediante una máquina 
de estados finita (FSM). Cada estado representa una acción o condición diferente (como correr, 
saltar, atacar, escalar paredes, etc.).
El script también contiene referencias a múltiples componentes clave como físicas, animaciones, 
manejo de inputs y estadísticas del jugador. A través de eventos y funciones auxiliares, 
permite controlar la interacción, la adaptación del collider, y reaccionar a eventos como 
quedarse sin poise.
En resumen, este script es el centro de control del jugador, coordinando su lógica, estados, 
físicas y animaciones en una estructura limpia y extensible.
---------------------------------------------------------------------------------------------*/

public class Player : MonoBehaviour
{
    #region State Variables
    public PlayerStateMachine StateMachine { get; private set; }

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

    public PlayerStunState PlayerStunState { get; private set; }

    public GrappleHandler GrappleHandler { get; private set; }


    [SerializeField]
    private PlayerData playerData;
    #endregion

    #region Components
    public Core Core { get; private set; }
    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public Transform DashDirectionIndicator { get; private set; }
    public BoxCollider2D MovementCollider { get; private set; }

    public Stats Stats { get; private set; }

    public InteractableDetector InteractableDetector { get; private set; }
    #endregion

    #region Weapons Variables         

    private Vector2 workspace;

    private Weapon primaryWeapon;
    private Weapon secondaryWeapon;

    #endregion

    #region Unity Callback Functions
    private void Awake()
    {
        Core = GetComponentInChildren<Core>();

        primaryWeapon = transform.Find("PrimaryWeapon").GetComponent<Weapon>();
        secondaryWeapon = transform.Find("SecondaryWeapon").GetComponent<Weapon>();

        primaryWeapon.SetCore(Core);
        secondaryWeapon.SetCore(Core);

        Stats = Core.GetCoreComponent<Stats>();
        InteractableDetector = Core.GetCoreComponent<InteractableDetector>();

        GrappleHandler = GetComponent<GrappleHandler>();

        StateMachine = new PlayerStateMachine();

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
        PlayerStunState = new PlayerStunState(this, StateMachine, playerData, "stun");
    }

    private void Start()
    {
        // Más referencias necesarias
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();

        // Evento cuando se intenta interactuar con algo
        InputHandler.OnInteractInputChanged += InteractableDetector.TryInteract;

        RB = GetComponent<Rigidbody2D>();
        DashDirectionIndicator = transform.Find("DashDirectionIndicator");
        MovementCollider = GetComponent<BoxCollider2D>();

        // Evento que escucha cuando la estadística de poise llega a 0
        Stats.Poise.OnCurrentValueZero += HandlePoiseCurrentValueZero;

        // Iniciar en el estado de reposo
        StateMachine.Initialize(IdleState);
    }

    private void HandlePoiseCurrentValueZero()
    {
        // Cambia al estado de stun cuando poise llega a 0
        StateMachine.ChangeState(PlayerStunState);
    }

    private void Update()
    {
        // Actualización lógica del sistema central y del estado actual
        Core.LogicUpdate();
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        // Actualización física del estado actual
        StateMachine.CurrentState.PhysicsUpdate();
    }

    private void OnDestroy()
    {
        // Limpia el evento al destruir el jugador
        Stats.Poise.OnCurrentValueZero -= HandlePoiseCurrentValueZero;
    }

    #endregion

    #region Set Functions

    // Ajusta la altura del collider del jugador dinámicamente
    public void SetColliderHeight(float height)
    {
        Vector2 center = MovementCollider.offset;
        workspace.Set(MovementCollider.size.x, height);

        center.y += (height - MovementCollider.size.y) / 2;

        MovementCollider.size = workspace;
        MovementCollider.offset = center;
    }

    // Llamados desde eventos de animación para transiciones
    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimtionFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    public void CancelGrapple()
    {
        GrappleHandler?.ForceStopGrapple(); // Método que ahora crearemos
    }


    #endregion
}
