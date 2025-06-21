using System;
using Avocado.CoreSystem;
using Mirror;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
ActionHitBox es un componente especializado para detectar colisiones de ataque en 2D cuando se 
activa una animación de ataque:
-Funciona con AnimationEventHandler, escuchando el evento OnAttackAction para saber exactamente 
cuándo debe activarse el hitbox.
-Usa Physics2D.OverlapBoxAll para detectar a los enemigos (u otros objetos) en una caja alrededor 
del arma en el momento del ataque.
-Calcula correctamente el offset del hitbox dependiendo de hacia dónde está mirando el personaje.
-Dispara el evento OnDetectedCollider2D para notificar a otros sistemas (como el de daño) que 
hubo una colisión.
-El método OnDrawGizmosSelected permite visualizar los hitboxes en el editor si el Debug está 
activado en los datos de ataque, lo cual es útil para diseñar y ajustar ataques visualmente.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    public class ActionHitBox : WeaponComponent<ActionHitBoxData, AttackActionHitBox>
    {
        // Evento que se dispara cuando se detectan colisiones dentro del área del hitbox
        public event Action<Collider2D[]> OnDetectedCollider2D;

        // Referencia al componente de movimiento para saber hacia qué dirección está mirando el personaje
        private CoreComp<CoreSystem.Movement> movement;

        // Offset calculado dinámicamente para posicionar correctamente el hitbox
        private Vector2 offset;

        // Resultado del análisis de colisiones
        private Collider2D[] detected;

        // Maneja el evento de ataque proveniente de AnimationEventHandler.
        // Calcula la posición del hitbox y detecta colisiones.
        private void HandleAttackAction()
        {
            //if (!NetworkServer.active || currentAttackData == null) return; // Solo en server
            if ( currentAttackData == null) return; // Solo en server


            // Calcula el offset del hitbox tomando en cuenta la dirección hacia donde se enfrenta el personaje
            offset.Set(
                transform.position.x + (currentAttackData.HitBox.center.x * movement.Comp.FacingDirection),
                transform.position.y + currentAttackData.HitBox.center.y
            );

            // Detecta todas las colisiones dentro del área del hitbox
            detected = Physics2D.OverlapBoxAll(offset, currentAttackData.HitBox.size, 0f, data.DetectableLayers);

            // Si no se detectaron colisiones, termina aquí
            if (detected.Length == 0)
                return;

            // Lanza el evento para comunicar que se detectaron colisiones
            OnDetectedCollider2D?.Invoke(detected);
        }

        protected override void Start()
        {
            base.Start();

            // Inicializa la referencia al componente de movimiento del núcleo
            movement = new CoreComp<CoreSystem.Movement>(Core);
            // Se suscribe al evento de ataque definido en AnimationEventHandler
            AnimationEventHandler.OnAttackAction += HandleAttackAction;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            AnimationEventHandler.OnAttackAction -= HandleAttackAction;
        }

        // Dibuja el hitbox en el editor cuando el objeto está seleccionado. Solo se activa si la opción "Debug" está marcada en los datos.
        private void OnDrawGizmosSelected()
        {
            if (data == null)
                return;

            foreach (var item in data.GetAllAttackData())
            {
                if (!item.Debug)
                    continue;

                Gizmos.DrawWireCube(transform.position + (Vector3)item.HitBox.center, item.HitBox.size);
            }
        }
    }
}
