using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script es el estado JumpState del jugador. 
Se encarga de:
Aplicar una velocidad vertical al jugador cuando salta.
Reducir la cantidad de saltos disponibles (para controlar saltos múltiples, como un doble salto).
Determinar si el jugador aún puede saltar (CanJump).
Interactuar con otros estados como InAirState para marcar que el jugador está en el aire y en 
pleno salto.
Restablecer o reducir manualmente los saltos disponibles cuando se requiera (por ejemplo, al 
tocar el suelo).
---------------------------------------------------------------------------------------------*/

public class PlayerJumpState : PlayerAbilityState
{
    private int amountOfJumpsLeft;

    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        // Inicializa la cantidad de saltos restantes con el valor configurado en los datos del jugador
        amountOfJumpsLeft = playerData.amountOfJumps;
    }

    // Método que se llama al entrar en el estado de salto
    public override void Enter()
    {
        base.Enter();

        // Se consume la entrada de salto para evitar múltiples usos con la misma pulsación
        player.InputHandler.UseJumpInput();

        // Establece la velocidad vertical del jugador según el valor de salto configurado
        Movement?.SetVelocityY(playerData.jumpVelocity);

        // Marca que esta habilidad ya ha sido usada
        isAbilityDone = true;

        // Resta uno a la cantidad de saltos disponibles
        amountOfJumpsLeft--;

        // Indica al estado "InAir" que el jugador está en pleno salto
        player.InAirState.SetIsJumping();
    }

    // Retorna si el jugador aún puede saltar (si tiene saltos restantes)
    public bool CanJump()
    {
        if (amountOfJumpsLeft > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Restaura la cantidad de saltos disponibles al máximo
    public void ResetAmountOfJumpsLeft() => amountOfJumpsLeft = playerData.amountOfJumps;

    // Reduce manualmente la cantidad de saltos disponibles
    public void DecreaseAmountOfJumpsLeft() => amountOfJumpsLeft--;
}
