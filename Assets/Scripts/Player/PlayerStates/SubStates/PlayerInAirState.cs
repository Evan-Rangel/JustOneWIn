using System.Collections;
using System.Collections.Generic;
using Avocado.CoreSystem;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script es el estado PlayerInAirState maneja todo el comportamiento del jugador cuando está 
en el aire:
Detecta colisiones con el suelo y paredes.
Permite saltar con coyote time (saltos justo después de salir del suelo o de una pared).
Gestiona transiciones a estados como atacar, saltar en pared, resbalar por pared, trepar bordes, 
aterrizar, dash, etc.
Aplica lógica de salto variable (salto más bajo si se suelta el botón).
Controla el movimiento horizontal mientras está en el aire.
---------------------------------------------------------------------------------------------*/

// Estado del jugador cuando está en el aire (saltando, cayendo, etc.)
public class PlayerInAirState : PlayerState
{
    protected Movement Movement => movement ?? core.GetCoreComponent(ref movement);
    private Movement movement;

    private CollisionSenses CollisionSenses => collisionSenses ?? core.GetCoreComponent(ref collisionSenses);
    private CollisionSenses collisionSenses;

    private int xInput;
    private bool jumpInput;
    private bool jumpInputStop;
    private bool grabInput;
    private bool dashInput;

    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingWallBack;
    private bool oldIsTouchingWall;
    private bool oldIsTouchingWallBack;
    private bool isTouchingLedge;

    // Estados especiales
    private bool coyoteTime;                // Permite saltar por un pequeño tiempo tras dejar el suelo
    private bool wallJumpCoyoteTime;        // Igual que arriba, pero para saltos en pared
    private bool isJumping;

    private float startWallJumpCoyoteTime;

    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        // Guardar estados anteriores de contacto con la pared
        oldIsTouchingWall = isTouchingWall;
        oldIsTouchingWallBack = isTouchingWallBack;

        // Verificar colisiones si el componente existe
        if (CollisionSenses)
        {
            isGrounded = CollisionSenses.Ground;
            isTouchingWall = CollisionSenses.WallFront;
            isTouchingWallBack = CollisionSenses.WallBack;
            isTouchingLedge = CollisionSenses.LedgeHorizontal;
        }

        // Detectar si se puede iniciar una trepada
        if (isTouchingWall && !isTouchingLedge)
        {
            player.LedgeClimbState.SetDetectedPosition(player.transform.position);
        }

        // Iniciar coyote time de salto en pared si se dejó de tocar la pared recientemente
        if (!wallJumpCoyoteTime && !isTouchingWall && !isTouchingWallBack &&
            (oldIsTouchingWall || oldIsTouchingWallBack))
        {
            StartWallJumpCoyoteTime();
        }
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        // Reset de variables relacionadas con contacto en pared
        oldIsTouchingWall = false;
        oldIsTouchingWallBack = false;
        isTouchingWall = false;
        isTouchingWallBack = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Verificar si aún se puede saltar (coyote time)
        CheckCoyoteTime();
        CheckWallJumpCoyoteTime();

        // Leer entradas del jugador
        xInput = player.InputHandler.NormInputX;
        jumpInput = player.InputHandler.JumpInput;
        jumpInputStop = player.InputHandler.JumpInputStop;
        grabInput = player.InputHandler.GrabInput;
        dashInput = player.InputHandler.DashInput;

        // Aplicar multiplicador de salto variable si se suelta el botón de salto
        CheckJumpMultiplier();
        // Transiciones de estado por prioridad
        if (player.InputHandler.AttackInputs[(int)CombatInputs.primary] && player.PrimaryAttackState.CanTransitionToAttackState())
        {
            if (player.TryGetComponent<PlayerObjectController>(out var poc) && poc.authority)
                poc.NetworkStartAttack(0);
            else
                stateMachine.ChangeState(player.PrimaryAttackState);
        }
        else if (player.InputHandler.AttackInputs[(int)CombatInputs.secondary] && player.SecondaryAttackState.CanTransitionToAttackState())
        {
            if (player.TryGetComponent<PlayerObjectController>(out var poc) && poc.authority)
                poc.NetworkStartAttack(1);
            else
                stateMachine.ChangeState(player.SecondaryAttackState);
        }
        else if (isGrounded && Movement?.CurrentVelocity.y < 0.01f)
        {
            stateMachine.ChangeState(player.LandState);
        }
        else if (isTouchingWall && !isTouchingLedge && !isGrounded)
        {
            stateMachine.ChangeState(player.LedgeClimbState);
        }
        else if (jumpInput && (isTouchingWall || isTouchingWallBack || wallJumpCoyoteTime))
        {
            StopWallJumpCoyoteTime();
            isTouchingWall = CollisionSenses.WallFront;
            player.WallJumpState.DetermineWallJumpDirection(isTouchingWall);
            stateMachine.ChangeState(player.WallJumpState);
        }
        else if (jumpInput && player.JumpState.CanJump())
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if (isTouchingWall && grabInput && isTouchingLedge)
        {
            stateMachine.ChangeState(player.WallGrabState);
        }
        else if (isTouchingWall && xInput == Movement?.FacingDirection && Movement?.CurrentVelocity.y <= 0)
        {
            stateMachine.ChangeState(player.WallSlideState);
        }
        else if (dashInput && player.DashState.CheckIfCanDash())
        {
            stateMachine.ChangeState(player.DashState);
        }
        else
        {
            // Movimiento horizontal en el aire
            Movement?.CheckIfShouldFlip(xInput);
            Movement?.SetVelocityX(playerData.movementVelocity * xInput);

            // Actualización de animaciones
            player.Anim.SetFloat("yVelocity", Movement.CurrentVelocity.y);
            player.Anim.SetFloat("xVelocity", Mathf.Abs(Movement.CurrentVelocity.x));
        }
    }

    // Controla si debe aplicarse el multiplicador para reducir salto al soltar el botón
    private void CheckJumpMultiplier()
    {
        if (isJumping)
        {
            if (jumpInputStop)
            {
                Movement?.SetVelocityY(Movement.CurrentVelocity.y * playerData.variableJumpHeightMultiplier);
                isJumping = false;
            }
            else if (Movement.CurrentVelocity.y <= 0f)
            {
                isJumping = false;
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    // Verifica si se acabó el coyote time para salto normal
    private void CheckCoyoteTime()
    {
        if (coyoteTime && Time.time > startTime + playerData.coyoteTime)
        {
            coyoteTime = false;
            player.JumpState.DecreaseAmountOfJumpsLeft();
        }
    }

    // Verifica si se acabó el coyote time para salto en pared
    private void CheckWallJumpCoyoteTime()
    {
        if (wallJumpCoyoteTime && Time.time > startWallJumpCoyoteTime + playerData.coyoteTime)
        {
            wallJumpCoyoteTime = false;
        }
    }

    // Inicia coyote time para salto normal
    public void StartCoyoteTime() => coyoteTime = true;

    // Inicia coyote time para salto en pared
    public void StartWallJumpCoyoteTime()
    {
        wallJumpCoyoteTime = true;
        startWallJumpCoyoteTime = Time.time;
    }

    // Detiene el coyote time para salto en pared
    public void StopWallJumpCoyoteTime() => wallJumpCoyoteTime = false;

    // Indica que se está en un salto activo
    public void SetIsJumping() => isJumping = true;
}
