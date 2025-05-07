using System;
using Avocado.ObjectPoolSystem;
using Avocado.ProjectileSystem;
using Avocado.Weapons.Components;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script define una estrategia avanzada para lanzar proyectiles en Unity. En lugar de lanzar 
solo uno, esta clase permite lanzar múltiples proyectiles en abanico, usando una variación de 
ángulo (AngleVariation) para dispersarlos alrededor de la dirección base.
Es ideal para armas con un sistema de carga o disparos múltiples (como escopetas, hechizos, etc.). 
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons
{
    [Serializable]
    public class ChargeProjectileSpawnerStrategy : ProjectileSpawnerStrategy
    {
        public float AngleVariation; 
        public int ChargeAmount;     

        private Vector2 currentDirection;

        // Método principal que ejecuta la estrategia de spawn
        public override void ExecuteSpawnStrategy(ProjectileSpawnInfo projectileSpawnInfo, Vector3 spawnerPos, int facingDirection, ObjectPools objectPools, Action<Projectile> OnSpawnProjectile     )
        {
            // No hay cargas, no se lanza nada
            if (ChargeAmount <= 0)
                return;

            // Si solo hay una carga, no se hace rotación: se lanza en la dirección original
            if (ChargeAmount == 1)
            {
                currentDirection = projectileSpawnInfo.Direction;
            }
            else
            {
                // Calcula la rotación inicial para que el primer proyectil se lance desde un ángulo negativo de forma que todos los proyectiles queden simétricos respecto a la dirección original.
                var initialRotationQuaternion = Quaternion.Euler(0f, 0f, -((ChargeAmount - 1f) * AngleVariation / 2f));
                currentDirection = initialRotationQuaternion * projectileSpawnInfo.Direction;
            }

            // Rotación para aplicar en cada paso del bucle
            var rotationQuaternion = Quaternion.Euler(0f, 0f, AngleVariation);

            // Bucle que lanza un proyectil por carga
            for (var i = 0; i < ChargeAmount; i++)
            {
                // Método heredado que instancia el proyectil en el mundo
                SpawnProjectile(projectileSpawnInfo, currentDirection, spawnerPos, facingDirection, objectPools, OnSpawnProjectile);

                // Actualiza la dirección rotándola para el siguiente proyectil
                currentDirection = rotationQuaternion * currentDirection;
            }
        }
    }
}
