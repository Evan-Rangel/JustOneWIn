using System.Collections;
using System.Collections.Generic;
using Avocado.CoreSystem;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script es el estado PlayerGroundedState es una clase base para cualquier estado donde el 
jugador está tocando el suelo. Su función principal es:
-Leer entradas del jugador (moverse, saltar, atacar, agarrarse, hacer dash).
-Realizar revisiones de colisiones como si está tocando el suelo, techo, pared o repisa.
-Transicionar a otros estados si se cumplen ciertas condiciones:
-Salto, ataques, dash, agarre de pared, o si el jugador cae.
-Resetear variables importantes como la cantidad de saltos restantes o el dash al entrar al estado.
---------------------------------------------------------------------------------------------*/

public class PlayerGroundedState : PlayerState
{
    protected int xInput;
    protected int yInput;

    protected bool isTouchingCeiling;

    protected Movement Movement => movement ?? core.GetCoreComponent(ref movement);
    private Movement movement;

    private CollisionSenses CollisionSenses => collisionSenses ?? core.GetCoreComponent(ref collisionSenses);
    private CollisionSenses collisionSenses;

    private bool jumpInput;
    private bool grabInput;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingLedge;
    private bool dashInput;

    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    // Verifica el entorno del jugador (colisiones)
    public override void DoChecks()
    {
        base.DoChecks();

        if (CollisionSenses)
        {
            isGrounded = CollisionSenses.Ground;
            isTouchingWall = CollisionSenses.WallFront;
            isTouchingLedge = CollisionSenses.LedgeHorizontal;
            isTouchingCeiling = CollisionSenses.Ceiling;
        }
    }

    // Al entrar al estado: reinicia saltos y dash
    public override void Enter()
    {
        base.Enter();
        player.JumpState.ResetAmountOfJumpsLeft();
        player.DashState.ResetCanDash();
    }

    public override void Exit()
    {
        base.Exit();
    }

    // Lógica principal de decisiones y transiciones
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (player == null || player.InputHandler == null) return;
        if (player.InputHandler.AttackInputs == null) return;
        // Captura las entradas del jugador
        xInput = player.InputHandler.NormInputX;
        yInput = player.InputHandler.NormInputY;
        jumpInput = player.InputHandler.JumpInput;
        grabInput = player.InputHandler.GrabInput;
        dashInput = player.InputHandler.DashInput;

        // Ataque primario
        if (player.InputHandler.AttackInputs[(int)CombatInputs.primary] && !isTouchingCeiling && player.PrimaryAttackState.CanTransitionToAttackState())
        {
            if (player.TryGetComponent<PlayerObjectController>(out var poc) && poc.authority)
                poc.NetworkStartAttack(0); // primario
            else
                stateMachine.ChangeState(player.PrimaryAttackState);
        }
        // Ataque secundario
        else if (player.InputHandler.AttackInputs[(int)CombatInputs.secondary] && !isTouchingCeiling && player.SecondaryAttackState.CanTransitionToAttackState())
        {
            if (player.TryGetComponent<PlayerObjectController>(out var poc) && poc.authority)
                poc.NetworkStartAttack(1); // secundario
            else
                stateMachine.ChangeState(player.SecondaryAttackState);
        }
        // Salto
        else if (jumpInput && player.JumpState.CanJump() && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.JumpState);
        }
        // Caída (ya no está en el suelo)
        else if (!isGrounded)
        {
            player.InAirState.StartCoyoteTime(); // Coyote time permite saltar justo después de dejar el suelo
            stateMachine.ChangeState(player.InAirState);
        }
        // Agarre de pared (solo si está tocando la pared y la repisa)
        else if (isTouchingWall && grabInput && isTouchingLedge)
        {
            stateMachine.ChangeState(player.WallGrabState);
        }
        // Dash
        else if (dashInput && player.DashState.CheckIfCanDash() && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.DashState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
