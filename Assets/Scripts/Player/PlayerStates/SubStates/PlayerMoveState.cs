using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script es el estado PlayerMoveState representa cuando el jugador camina normalmente sobre 
el suelo. Hereda de PlayerGroundedState, por lo que ya tiene lógica base como detección de 
suelo, manejo 
de animaciones o transición a salto o caída.
Comportamiento clave:
LogicUpdate(): Aplica velocidad horizontal según el input (xInput).
Gira al personaje si es necesario.
Si deja de moverse, cambia al estado IdleState.
Si presiona hacia abajo, cambia a CrouchMoveState.
---------------------------------------------------------------------------------------------*/

public class PlayerMoveState : PlayerGroundedState
{
    // Constructor del estado
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    // Revisión de condiciones físicas (raycasts, colisiones, etc.)
    public override void DoChecks()
    {
        base.DoChecks();
    }

    // Al entrar al estado
    public override void Enter()
    {
        base.Enter();
    }

    // Al salir del estado
    public override void Exit()
    {
        base.Exit();
    }

    // Actualización lógica por frame
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Gira al jugador si cambia la dirección del movimiento
        Movement?.CheckIfShouldFlip(xInput);

        // Aplica velocidad en X según la dirección del input
        Movement?.SetVelocityX(playerData.movementVelocity * xInput);

        // Cambia de estado si se suelta la dirección o se presiona hacia abajo
        if (!isExitingState)
        {
            if (xInput == 0)
            {
                stateMachine.ChangeState(player.IdleState); // Se detiene
            }
            else if (yInput == -1)
            {
                stateMachine.ChangeState(player.CrouchMoveState); // Se agacha caminando
            }
        }
    }

    // Actualización física por frame
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
