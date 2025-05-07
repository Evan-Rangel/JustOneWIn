using System;
using Avocado.Weapons.Components;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este componente se encarga de controlar el movimiento durante los ataques, como un impulso 
hacia adelante o un retroceso. Usa datos definidos en AttackMovement para aplicar velocidad 
y dirección al personaje mediante el sistema de animaciones (AnimationEventHandler).
HandleStartMovement() y HandleStopMovement() son llamados por eventos de animación para comenzar 
o detener el movimiento del personaje durante un ataque.
FixedUpdate() asegura que mientras el ataque esté activo, el personaje mantenga su movimiento 
horizontal.
Usa el componente CoreSystem.Movement para aplicar la física del movimiento real en el personaje.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    public class Movement : WeaponComponent<MovementData, AttackMovement>
    {
        private CoreSystem.Movement coreMovement; // Referencia al componente de movimiento principal

        private float velocity;       // Velocidad actual del ataque
        private Vector2 direction;    // Dirección actual del ataque

        // Evento que inicia el movimiento (desde la animación).
        private void HandleStartMovement()
        {
            velocity = currentAttackData.Velocity;     // Obtiene la velocidad definida en el ataque
            direction = currentAttackData.Direction;   // Obtiene la dirección definida en el ataque

            SetVelocity(); // Aplica el movimiento completo
        }

        // Evento que detiene el movimiento (también desde la animación).
        private void HandleStopMovement()
        {
            velocity = 0f;
            direction = Vector2.zero;

            SetVelocity(); // Detiene el movimiento
        }

        // Se ejecuta al iniciar el ataque. Se asegura de que el movimiento esté en cero.
        protected override void HandleEnter()
        {
            base.HandleEnter();

            velocity = 0f;
            direction = Vector2.zero;
        }

        // Se llama cada FixedUpdate mientras el ataque está activo. Aplica solo movimiento horizontal.
        private void FixedUpdate()
        {
            if (!isAttackActive)
                return;

            SetVelocityX(); // Aplica la velocidad en el eje X (horizontal)
        }

        // Aplica la velocidad completa (X y Y) al personaje.
        private void SetVelocity()
        {
            coreMovement.SetVelocity(velocity, direction, coreMovement.FacingDirection);
        }

        // Aplica solo la velocidad en el eje X .
        private void SetVelocityX()
        {
            coreMovement.SetVelocityX((direction * velocity).x * coreMovement.FacingDirection);
        }

        protected override void Start()
        {
            base.Start();

            coreMovement = Core.GetCoreComponent<CoreSystem.Movement>();

            AnimationEventHandler.OnStartMovement += HandleStartMovement;
            AnimationEventHandler.OnStopMovement += HandleStopMovement;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            AnimationEventHandler.OnStartMovement -= HandleStartMovement;
            AnimationEventHandler.OnStopMovement -= HandleStopMovement;
        }
    }
}
