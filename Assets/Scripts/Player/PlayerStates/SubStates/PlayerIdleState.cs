using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script es el estado PlayerIdleState representa el estado en que el jugador está parado en el 
suelo sin moverse.
Al entrar, se asegura de detener el movimiento horizontal.
En la lógica, reacciona al input:
Si se mueve a los lados, cambia al estado de movimiento.
Si presiona hacia abajo, entra en estado de agachado.
Es un estado simple que sirve como punto de transición entre moverse, agacharse o quedar quieto.
---------------------------------------------------------------------------------------------*/

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    // Revisión de condiciones o colisiones al entrar en este estado
    public override void DoChecks()
    {
        base.DoChecks();
    }

    // Al entrar al estado, se detiene el movimiento horizontal
    public override void Enter()
    {
        base.Enter();
        Movement?.SetVelocityX(0f);
    }

    // Al salir del estado, no se hace nada extra
    public override void Exit()
    {
        base.Exit();
    }

    // Lógica principal del estado que se actualiza en cada frame
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            // Si hay entrada horizontal, cambiar al estado de movimiento
            if (xInput != 0)
            {
                stateMachine.ChangeState(player.MoveState);
            }
            // Si se presiona hacia abajo, cambiar al estado de agachado
            else if (yInput == -1)
            {
                stateMachine.ChangeState(player.CrouchIdleState);
            }
        }
    }

    // Actualización física si fuera necesaria (en este caso se hereda tal cual)
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
