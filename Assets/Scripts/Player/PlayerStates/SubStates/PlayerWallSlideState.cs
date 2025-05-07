using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script es el estado PlayerWallSlideState representa el momento en que el jugador está en 
contacto con una pared y se desliza hacia abajo por ella (por ejemplo, al saltar y chocar con 
una pared sin sostenerse).
Durante este estado:
Se aplica una velocidad descendente constante (simulando fricción con la pared).
Si el jugador mantiene el botón de agarre y no se mueve hacia arriba o abajo, entra al estado 
WallGrabState, donde queda pegado sin deslizarse.
---------------------------------------------------------------------------------------------*/

public class PlayerWallSlideState : PlayerTouchingWallState
{
    public PlayerWallSlideState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Solo realiza la lógica si el estado no está siendo abandonado
        if (!isExitingState)
        {
            // Aplica una velocidad hacia abajo constante (deslizamiento por la pared)
            Movement?.SetVelocityY(-playerData.wallSlideVelocity);

            // Si el jugador mantiene el botón de agarre y no está moviéndose verticalmente (yInput == 0),
            // entonces cambia al estado de agarre en pared
            if (grabInput && yInput == 0)
            {
                stateMachine.ChangeState(player.WallGrabState);
            }
        }
    }
}
