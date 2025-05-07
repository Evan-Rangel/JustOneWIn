using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script es el estado PlayerCrouchMoveState controla al jugador cuando está agachado y caminando.
Al entrar al estado, ajusta el tamaño del collider a la altura de agachado.
Aplica una velocidad de movimiento reducida y chequea si el personaje debe voltear.
Si el jugador deja de moverse (xInput == 0), pasa a CrouchIdleState.
Si deja de agacharse (yInput != -1) y no hay techo encima, vuelve a caminar normalmente (MoveState).
---------------------------------------------------------------------------------------------*/

public class PlayerCrouchMoveState : PlayerGroundedState
{
    public PlayerCrouchMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    // Al entrar al estado:
    public override void Enter()
    {
        base.Enter();
        player.SetColliderHeight(playerData.crouchColliderHeight); // Cambia el tamaño del collider al de agachado
    }

    // Al salir del estado:
    public override void Exit()
    {
        base.Exit();
        player.SetColliderHeight(playerData.standColliderHeight); // Restaura la altura original del collider
    }

    // Se ejecuta en cada frame
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            Movement?.SetVelocityX(playerData.crouchMovementVelocity * Movement.FacingDirection); // Aplica velocidad de movimiento agachado
            Movement?.CheckIfShouldFlip(xInput); // Revisa si debe cambiar de dirección según la entrada horizontal

            // Si deja de moverse, cambia al estado de agachado en reposo
            if (xInput == 0)
            {
                stateMachine.ChangeState(player.CrouchIdleState);
            }
            // Si suelta el botón de agacharse y no hay techo encima, cambia a estado de caminar normal
            else if (yInput != -1 && !isTouchingCeiling)
            {
                stateMachine.ChangeState(player.MoveState);
            }
        }
    }
}
