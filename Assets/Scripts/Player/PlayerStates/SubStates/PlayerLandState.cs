using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script es el estado PlayerLandState maneja lo que sucede justo después de que el jugador 
aterriza tras estar en el aire (por ejemplo, después de un salto o caída).
Este estado permite que la animación de aterrizaje se reproduzca correctamente antes de cambiar 
a otro estado.
Si el jugador se mueve horizontalmente durante este estado, se pasa directamente al estado de 
movimiento.
Si no hay movimiento, se espera a que termine la animación para pasar al estado de reposo 
(IdleState).
---------------------------------------------------------------------------------------------*/

public class PlayerLandState : PlayerGroundedState
{
    public PlayerLandState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    // Actualización lógica del estado (se llama cada frame)
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Si no estamos saliendo del estado actual
        if (!isExitingState)
        {
            // Si hay input horizontal, cambiar al estado de movimiento
            if (xInput != 0)
            {
                stateMachine.ChangeState(player.MoveState);
            }
            // Si la animación de aterrizaje ha terminado, pasar a estado de reposo (idle)
            else if (isAnimationFinished)
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
    }
}
