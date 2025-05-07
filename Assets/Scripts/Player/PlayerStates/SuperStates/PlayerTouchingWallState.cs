using System.Collections;
using System.Collections.Generic;
using Avocado.CoreSystem;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script es el estado PlayerTouchingWallState es un estado base que se activa cuando el 
jugador está tocando una pared. Desde aquí se gestionan varias transiciones posibles:
-Salto en pared: Si el jugador presiona salto mientras está en la pared.
-Volver al suelo (Idle): Si está en el suelo y deja de agarrar la pared.
-Caída libre (InAir): Si se suelta de la pared o se mueve en la dirección opuesta.
-Subir una repisa (LedgeClimbState): Si está tocando una pared pero no una repisa.
---------------------------------------------------------------------------------------------*/

public class PlayerTouchingWallState : PlayerState
{
    protected Movement Movement => movement ?? core.GetCoreComponent(ref movement);
    private Movement movement;

    private CollisionSenses CollisionSenses => collisionSenses ?? core.GetCoreComponent(ref collisionSenses);
    private CollisionSenses collisionSenses;

    protected bool isGrounded;
    protected bool isTouchingWall;
    protected bool grabInput;
    protected bool jumpInput;
    protected bool isTouchingLedge;
    protected int xInput;
    protected int yInput;

    public PlayerTouchingWallState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) 
    { 
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    // Verifica colisiones con suelo, pared y repisa
    public override void DoChecks()
    {
        base.DoChecks();

        if (CollisionSenses)
        {
            isGrounded = CollisionSenses.Ground;
            isTouchingWall = CollisionSenses.WallFront;
            isTouchingLedge = CollisionSenses.LedgeHorizontal;
        }

        // Si está tocando pared pero no una repisa, guarda la posición para subir por la repisa
        if (isTouchingWall && !isTouchingLedge)
        {
            player.LedgeClimbState.SetDetectedPosition(player.transform.position);
        }
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    // Lógica de transición entre estados al tocar una pared
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Captura las entradas del jugador
        xInput = player.InputHandler.NormInputX;
        yInput = player.InputHandler.NormInputY;
        grabInput = player.InputHandler.GrabInput;
        jumpInput = player.InputHandler.JumpInput;

        // Transición a salto en pared
        if (jumpInput)
        {
            player.WallJumpState.DetermineWallJumpDirection(isTouchingWall);
            stateMachine.ChangeState(player.WallJumpState);
        }
        // Si está en el suelo y ya no está agarrando la pared, pasa a idle
        else if (isGrounded && !grabInput)
        {
            stateMachine.ChangeState(player.IdleState);
        }
        // Si ya no está tocando la pared o se aleja de ella, va al estado en el aire
        else if (!isTouchingWall || (xInput != Movement?.FacingDirection && !grabInput))
        {
            stateMachine.ChangeState(player.InAirState);
        }
        // Si está tocando la pared pero no la repisa, cambia a trepar repisa
        else if (isTouchingWall && !isTouchingLedge)
        {
            stateMachine.ChangeState(player.LedgeClimbState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
