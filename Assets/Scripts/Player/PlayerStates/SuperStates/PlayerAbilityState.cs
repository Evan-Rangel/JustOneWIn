using System.Collections;
using System.Collections.Generic;
using Avocado.CoreSystem;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script es el estado PlayerAbilityState es una clase base abstracta para estados de 
habilidades especiales (como el dash, salto en pared, ataque especial, etc.).
Su propósito principal es:
-Permitir ejecutar una habilidad temporal.
-Una vez terminada (isAbilityDone = true), transicionar automáticamente al estado adecuado:
-Si está en el suelo → IdleState.
-Si está en el aire → InAirState.
---------------------------------------------------------------------------------------------*/

public class PlayerAbilityState : PlayerState
{
    protected bool isAbilityDone;

    protected Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }

    private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }

    private Movement movement;
    private CollisionSenses collisionSenses;

    private bool isGrounded;

    public PlayerAbilityState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    // Revisión de condiciones necesarias (por ejemplo, si está en el suelo)
    public override void DoChecks()
    {
        base.DoChecks();

        if (CollisionSenses)
        {
            isGrounded = CollisionSenses.Ground;
        }
    }

    // Se ejecuta al entrar al estado: resetea la bandera de habilidad terminada
    public override void Enter()
    {
        base.Enter();
        isAbilityDone = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    // Lógica de transición basada en si la habilidad terminó
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Si la habilidad ya se completó
        if (isAbilityDone)
        {
            // Si el jugador está en el suelo y no se está moviendo hacia arriba
            if (isGrounded && Movement?.CurrentVelocity.y < 0.01f)
            {
                // Cambia al estado de reposo
                stateMachine.ChangeState(player.IdleState);
            }
            else
            {
                // Si no está en el suelo, cambia al estado en el aire
                stateMachine.ChangeState(player.InAirState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
