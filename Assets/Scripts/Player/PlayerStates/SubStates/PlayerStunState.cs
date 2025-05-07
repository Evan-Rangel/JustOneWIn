using Avocado.CoreSystem;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script es el estado PlayerStunState se activa cuando el jugador queda aturdido (tras recibir 
un golpe fuerte). 
Durante este estado:
Se detiene por completo el movimiento horizontal.
El jugador no puede realizar acciones.
Permanecerá aturdido durante un tiempo determinado (stunTime) definido en los datos del jugador 
(PlayerData).
Luego vuelve automáticamente al estado IdleState una vez que el tiempo se cumple.
---------------------------------------------------------------------------------------------*/

namespace Avocado.FSM
{
    // Estado que representa cuando el jugador está aturdido
    public class PlayerStunState : PlayerState
    {
        // Referencia al componente de movimiento
        private readonly Movement movement;

        // Constructor: inicializa el estado y obtiene el componente de movimiento del núcleo
        public PlayerStunState(
            Player player,
            PlayerStateMachine stateMachine,
            PlayerData playerData,
            string animBoolName
        ) : base(player, stateMachine, playerData, animBoolName)
        {
            movement = core.GetCoreComponent<Movement>();
        }

        // Se ejecuta cada frame para la lógica del estado
        public override void LogicUpdate()
        {
            base.LogicUpdate();

            // Detiene el movimiento horizontal mientras está aturdido
            movement.SetVelocityX(0f);

            // Si el tiempo de aturdimiento ha pasado, cambia al estado Idle
            if (Time.time >= startTime + playerData.stunTime)
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
    }
}
