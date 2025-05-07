using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script es el estado PlayerWallGrabState se activa cuando el jugador se agarra a una pared 
y no se mueve verticalmente. Durante este estado:
-El jugador queda congelado en su posición actual, simulando que está suspendido en la pared.
-Si el jugador presiona hacia arriba, pasa al estado WallClimbState para escalar.
-Si presiona hacia abajo o suelta el botón de agarre (grabInput), pasa al estado WallSlideState 
y comienza a deslizarse.
---------------------------------------------------------------------------------------------*/

public class PlayerWallGrabState : PlayerTouchingWallState
{
    // Guarda la posición exacta donde el jugador se queda "pegado" a la pared
    private Vector2 holdPosition;

    public PlayerWallGrabState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    // Disparadores de animación, disponibles si se usan eventos de animación (en este estado están vacíos)
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    // Verificaciones específicas del estado (colisiones, etc.)
    public override void DoChecks()
    {
        base.DoChecks();
    }

    // Al entrar en este estado:
    public override void Enter()
    {
        base.Enter();

        // Guarda la posición actual del jugador
        holdPosition = player.transform.position;

        // Fija al jugador en esa posición
        HoldPosition();
    }

    // Al salir del estado (no se necesita lógica adicional aquí)
    public override void Exit()
    {
        base.Exit();
    }

    // Actualización lógica que ocurre cada frame
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            // Mantiene al jugador fijo en la pared sin movimiento
            HoldPosition();

            // Si el jugador presiona hacia arriba, pasa al estado de escalar
            if (yInput > 0)
            {
                stateMachine.ChangeState(player.WallClimbState);
            }
            // Si presiona hacia abajo o suelta el botón de agarre, comienza a deslizarse
            else if (yInput < 0 || !grabInput)
            {
                stateMachine.ChangeState(player.WallSlideState);
            }
        }
    }

    // Mantiene al jugador congelado en la pared
    private void HoldPosition()
    {
        player.transform.position = holdPosition;

        Movement?.SetVelocityX(0f);
        Movement?.SetVelocityY(0f);
    }

    // Actualizaciones físicas (nada especial en este estado)
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
