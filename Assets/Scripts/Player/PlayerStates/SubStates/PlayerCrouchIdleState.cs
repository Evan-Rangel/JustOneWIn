using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script es el estado PlayerCrouchIdleState representa el estado del jugador cuando está 
agachado sin moverse. Al entrar, detiene el movimiento y reduce la altura del collider, simulando 
que el jugador está agachado.
Si el jugador presiona izquierda/derecha, cambia al estado de movimiento agachado.
Si deja de presionar abajo (yInput != -1) y no hay un techo encima del personaje, cambia al 
estado Idle, permitiéndole levantarse.
---------------------------------------------------------------------------------------------*/

public class PlayerCrouchIdleState : PlayerGroundedState
{
    public PlayerCrouchIdleState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    // Al entrar en este estado:
    public override void Enter()
    {
        base.Enter();

        Movement?.SetVelocityZero();                             // Se detiene el movimiento
        player.SetColliderHeight(playerData.crouchColliderHeight); // Se cambia el tamaño del collider al de agachado
    }

    // Al salir del estado:
    public override void Exit()
    {
        base.Exit();
        player.SetColliderHeight(playerData.standColliderHeight); // Se restaura la altura normal del collider
    }

    // Se actualiza cada frame:
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            // Si se mueve mientras está agachado, cambia al estado de agachado en movimiento
            if (xInput != 0)
            {
                stateMachine.ChangeState(player.CrouchMoveState);
            }
            // Si suelta el botón de agacharse y no hay techo encima, vuelve al estado Idle
            else if (yInput != -1 && !isTouchingCeiling)
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
    }
}
