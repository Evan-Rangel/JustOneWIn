using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
El componente Targeter permite detectar automáticamente enemigos (u otros objetos en una capa 
específica) dentro de un área de ataque mientras el ataque está activo. Su propósito es apoyar 
a otros sistemas, como aplicar daño, parálisis, etc., proporcionando una lista actualizada de 
objetivos.
La detección se hace usando Physics2D.OverlapBoxAll.
Se puede obtener la lista actual de objetivos con GetTargets().
Funciona en tiempo real durante FixedUpdate, lo cual es útil si necesitas saber qué hay en el 
área en cada frame físico.
Se muestra visualmente en la escena usando OnDrawGizmosSelected.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    // Este componente detecta colliders dentro de un área específica (OverlapBox)
    public class Targeter : WeaponComponent<TargeterData, AttackTargeter>
    {
        // Lista de objetivos detectados en el área definida
        private List<Transform> targets = new List<Transform>();

        // Componente de movimiento usado para determinar dirección
        private CoreSystem.Movement movement;

        // Flag que indica si el ataque (y por tanto la detección) está activo
        private bool isActive;

        // Se llama cuando inicia el ataque. Activa la detección de objetivos.
        protected override void HandleEnter()
        {
            base.HandleEnter();
            isActive = true;
        }

        // Se llama cuando termina el ataque. Detiene la detección.
        protected override void HandleExit()
        {
            base.HandleExit();
            isActive = false;
        }

        // Devuelve la lista de objetivos detectados más recientemente.
        public List<Transform> GetTargets()
        {
            return targets;
        }

        // Revisa el área definida para ver qué colliders hay presentes.
        private void CheckForTargets()
        {
            // Calcula la posición del área de detección en base a la dirección
            var pos = transform.position + new Vector3(currentAttackData.Area.center.x * movement.FacingDirection, currentAttackData.Area.center.y);

            // Detecta todos los colliders dentro del área y capa especificadas
            var targetColliders = Physics2D.OverlapBoxAll(pos, currentAttackData.Area.size, 0f, currentAttackData.DamageableLayer);

            // Almacena las transformaciones de los colliders detectados
            targets = targetColliders.Select(item => item.transform).ToList();
        }

        protected override void Start()
        {
            base.Start();
            movement = Core.GetCoreComponent<CoreSystem.Movement>();
        }

        // Se llama en cada FixedUpdate si el ataque está activo, para detectar objetivos.
        private void FixedUpdate()
        {
            if (!isActive)
                return;

            CheckForTargets();
        }

        // Muestra visualmente el área de detección y los objetivos en el editor de Unity.
        private void OnDrawGizmosSelected()
        {
            if (data == null)
                return;

            foreach (var attackTargeter in data.GetAllAttackData())
            {
                Gizmos.DrawWireCube(
                    transform.position + (Vector3)attackTargeter.Area.center,
                    attackTargeter.Area.size
                );
            }

            Gizmos.color = Color.red;
            foreach (var target in targets)
            {
                Gizmos.DrawWireSphere(target.position, 0.25f);
            }
        }
    }
}
