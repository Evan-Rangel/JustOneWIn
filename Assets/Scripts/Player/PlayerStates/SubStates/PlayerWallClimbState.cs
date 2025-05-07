using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script es el estado PlayerWallClimbState representa el estado del jugador cuando está 
tocando una pared y mantiene presionada la dirección hacia arriba, lo que indica que quiere 
escalar la pared:
El jugador se mueve hacia arriba con una velocidad definida por playerData.wallClimbVelocity.
Si deja de presionar hacia arriba, el jugador cambia automáticamente al estado 
de WallGrabState, es decir, sólo se agarra a la pared pero no sube.
Este estado se suele usar para plataformas verticales, muros altos o situaciones donde el 
jugador puede trepar superficies.
---------------------------------------------------------------------------------------------*/

public class PlayerWallClimbState : PlayerTouchingWallState
{
    public PlayerWallClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    // Se ejecuta cada frame mientras el jugador está en este estado
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState) // Asegura que no se ejecute si ya se está saliendo del estado
        {
            // Establece la velocidad vertical hacia arriba para simular que el jugador escala
            Movement?.SetVelocityY(playerData.wallClimbVelocity);

            // Si el jugador ya no mantiene la dirección hacia arriba, cambia al estado de agarrarse a la pared
            if (yInput != 1)
            {
                stateMachine.ChangeState(player.WallGrabState);
            }
        }
    }
}
