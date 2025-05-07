using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script es el estado PlayerWallJumpState se activa cuando el jugador salta desde una pared. 
Este estado:
-Calcula la dirección del salto basándose en si el jugador está tocando una pared o no.
-Aplica una fuerza de salto con un ángulo y velocidad configurables (usualmente hacia arriba y 
alejándose de la pared).
-Ajusta la dirección del personaje para que mire hacia donde salta.
-Gestiona los saltos restantes, útil si el juego permite doble salto o más.
---------------------------------------------------------------------------------------------*/

public class PlayerWallJumpState : PlayerAbilityState
{
    // Dirección del salto en el eje X (puede ser opuesta a la dirección actual del jugador)
    private int wallJumpDirection;

    public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    // Al entrar en el estado (cuando se ejecuta el wall jump)
    public override void Enter()
    {
        base.Enter();

        // Consume el input de salto para evitar múltiples registros del mismo salto
        player.InputHandler.UseJumpInput();

        // Restaura la cantidad de saltos disponibles (para permitir un salto doble después, por ejemplo)
        player.JumpState.ResetAmountOfJumpsLeft();

        // Aplica la velocidad del wall jump con la dirección calculada y el ángulo predefinido
        Movement?.SetVelocity(playerData.wallJumpVelocity, playerData.wallJumpAngle, wallJumpDirection);

        // Asegura que el personaje esté mirando en la dirección del salto
        Movement?.CheckIfShouldFlip(wallJumpDirection);

        // Reduce en 1 la cantidad de saltos restantes
        player.JumpState.DecreaseAmountOfJumpsLeft();
    }

    // Actualización lógica por frame
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Actualiza parámetros de animación con las velocidades actuales
        player.Anim.SetFloat("yVelocity", Movement.CurrentVelocity.y);
        player.Anim.SetFloat("xVelocity", Mathf.Abs(Movement.CurrentVelocity.x));

        // Termina la habilidad después de cierto tiempo definido en los datos del jugador
        if (Time.time >= startTime + playerData.wallJumpTime)
        {
            isAbilityDone = true;
        }
    }

    // Determina la dirección en la que se debe realizar el salto (opuesta a la pared si está tocando una)
    public void DetermineWallJumpDirection(bool isTouchingWall)
    {
        if (isTouchingWall)
        {
            wallJumpDirection = -Movement.FacingDirection;
        }
        else
        {
            wallJumpDirection = Movement.FacingDirection;
        }
    }
}
