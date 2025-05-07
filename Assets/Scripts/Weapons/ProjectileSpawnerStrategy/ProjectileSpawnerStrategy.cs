using System;
using Avocado.ObjectPoolSystem;
using Avocado.ProjectileSystem;
using Avocado.Weapons.Components;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Esta clase ProjectileSpawnerStrategy representa la estrategia por defecto para generar proyectiles. 
Su lógica es simple: genera un solo proyectil en una dirección determinada. Usa el patrón Strategy, 
lo que permite crear subclases con otras formas de disparar (múltiples proyectiles, ángulos 
aleatorios, proyectiles guiados, etc.). Sus pasos clave son:
-Calcular dirección y posición del disparo.
-Obtener un proyectil del pool.
-Posicionarlo y rotarlo.
-Inicializarlo con los datos de daño y visuales.
-Notificar a otros sistemas que un proyectil ha sido generado.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons
{
    public class ProjectileSpawnerStrategy : IProjectileSpawnerStrategy
    {
        private Vector2 spawnPos;    
        private Vector2 spawnDir;     
        private Projectile currentProjectile;  

        public virtual void ExecuteSpawnStrategy
        (ProjectileSpawnInfo projectileSpawnInfo, Vector3 spawnerPos, int facingDirection, ObjectPools objectPools, Action<Projectile> OnSpawnProjectile)
        {
            SpawnProjectile(projectileSpawnInfo, projectileSpawnInfo.Direction, spawnerPos, facingDirection, objectPools, OnSpawnProjectile);
        }

        // Método que encapsula toda la lógica para disparar un proyectil
        protected virtual void SpawnProjectile(ProjectileSpawnInfo projectileSpawnInfo, Vector2 spawnDirection, Vector3 spawnerPos, int facingDirection, ObjectPools objectPools, Action<Projectile> OnSpawnProjectile)
        {
            // Calcula posición y dirección final del disparo
            SetSpawnPosition(spawnerPos, projectileSpawnInfo.Offset, facingDirection);
            SetSpawnDirection(spawnDirection, facingDirection);

            // Obtiene un proyectil del pool y lo posiciona
            GetProjectileAndSetPositionAndRotation(objectPools, projectileSpawnInfo.ProjectilePrefab);

            // Inicializa los datos y comportamientos del proyectil
            InitializeProjectile(projectileSpawnInfo, OnSpawnProjectile);
        }

        // Toma un proyectil del pool y ajusta su posición y rotación
        protected virtual void GetProjectileAndSetPositionAndRotation(ObjectPools objectPools, Projectile prefab)
        {
            currentProjectile = objectPools.GetObject(prefab);  // Obtiene del pool

            // Asigna posición
            currentProjectile.transform.position = spawnPos;

            // Calcula el ángulo a partir de la dirección y lo aplica como rotación
            var angle = Mathf.Atan2(spawnDir.y, spawnDir.x) * Mathf.Rad2Deg;
            currentProjectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        // Inicializa y configura los datos del proyectil (daño, empuje, apariencia, etc.)
        protected virtual void InitializeProjectile(ProjectileSpawnInfo projectileSpawnInfo, Action<Projectile> OnSpawnProjectile)
        {
            currentProjectile.Reset();  // Resetea estado previo si existe

            // Asigna todos los paquetes de datos relevantes
            currentProjectile.SendDataPackage(projectileSpawnInfo.DamageData);
            currentProjectile.SendDataPackage(projectileSpawnInfo.KnockBackData);
            currentProjectile.SendDataPackage(projectileSpawnInfo.PoiseDamageData);
            currentProjectile.SendDataPackage(projectileSpawnInfo.SpriteDataPackage);

            // Permite que otros sistemas agreguen más lógica si lo desean
            OnSpawnProjectile?.Invoke(currentProjectile);

            currentProjectile.Init();  // Inicializa su comportamiento
        }

        // Ajusta la dirección del proyectil de acuerdo al lado al que está mirando el personaje
        protected virtual void SetSpawnDirection(Vector2 direction, int facingDirection)
        {
            spawnDir.Set(direction.x * facingDirection, direction.y);
        }

        // Calcula la posición del proyectil sumando el offset, considerando la dirección del personaje
        protected virtual void SetSpawnPosition(Vector3 referencePosition, Vector2 offset, int facingDirection)
        {
            spawnPos = referencePosition;
            spawnPos.Set(spawnPos.x + offset.x * facingDirection, spawnPos.y + offset.y);
        }
    }
}
