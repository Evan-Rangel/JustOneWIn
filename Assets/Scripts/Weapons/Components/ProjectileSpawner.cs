using System;
using System.Collections.Generic;
using Avocado.ObjectPoolSystem;
using Avocado.ProjectileSystem;
using Mirror;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
El componente ProjectileSpawner se encarga de generar y disparar proyectiles durante una animación 
de ataque:
-Usa un sistema de estrategias de disparo (IProjectileSpawnerStrategy) para definir cómo se lanzan 
los proyectiles (por ejemplo, en línea recta, en abanico, múltiples a la vez, etc.).
-Usa object pooling para mejorar el rendimiento al reutilizar proyectiles en lugar de crear nuevos.
-Al activarse mediante un evento de animación, lanza todos los proyectiles definidos en 
currentAttackData.SpawnInfos.
-Al finalizar el ataque, se restablece la estrategia al comportamiento predeterminado.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    public class ProjectileSpawner : WeaponComponent<ProjectileSpawnerData, AttackProjectileSpawner>
    {
        // Evento que se dispara antes de inicializar cada proyectil (permite a otros sistemas modificarlo si es necesario)
        public event Action<Projectile> OnSpawnProjectile;

        // Referencia al sistema de movimiento del personaje para obtener la dirección en la que mira
        private CoreSystem.Movement movement;

        // Sistema de pool para evitar instanciar proyectiles constantemente
        private readonly ObjectPools objectPools = new ObjectPools();

        // Estrategia actual para generar proyectiles
        private IProjectileSpawnerStrategy projectileSpawnerStrategy;

        // Permite establecer una estrategia personalizada para disparar proyectiles.
        public void SetProjectileSpawnerStrategy(IProjectileSpawnerStrategy newStrategy)
        {
            projectileSpawnerStrategy = newStrategy;
        }

        // Manejador que se llama durante el ataque para generar los proyectiles.
        private void HandleAttackAction()
        {
            //if (!NetworkServer.active) return; // Solo en server
            foreach (var projectileSpawnInfo in currentAttackData.SpawnInfos)
            {
                // Ejecuta la estrategia actual con los datos de spawn
                projectileSpawnerStrategy.ExecuteSpawnStrategy(
                    projectileSpawnInfo,
                    transform.position,
                    movement.FacingDirection,
                    objectPools,
                    OnSpawnProjectile
                );
            }
        }

        // Vuelve a la estrategia por defecto (una sola instancia simple por ataque).
        private void SetDefaultProjectileSpawnStrategy()
        {
            projectileSpawnerStrategy = new ProjectileSpawnerStrategy();
        }

        // Se llama al terminar el ataque. Reinicia la estrategia de disparo.
        protected override void HandleExit()
        {
            base.HandleExit();
            SetDefaultProjectileSpawnStrategy();
        }

        protected override void Awake()
        {
            base.Awake();
            SetDefaultProjectileSpawnStrategy(); // Inicializa con la estrategia por defecto
        }

        protected override void Start()
        {
            base.Start();
            movement = Core.GetCoreComponent<CoreSystem.Movement>();
            AnimationEventHandler.OnAttackAction += HandleAttackAction; // Conecta al evento de ataque
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            AnimationEventHandler.OnAttackAction -= HandleAttackAction;
        }

        // Muestra en el editor dónde aparecerán los proyectiles.
        private void OnDrawGizmosSelected()
        {
            if (data == null || !Application.isPlaying)
                return;

            foreach (var item in data.GetAllAttackData())
            {
                foreach (var point in item.SpawnInfos)
                {
                    var pos = transform.position + (Vector3)point.Offset;

                    Gizmos.DrawWireSphere(pos, 0.2f);
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(pos, pos + (Vector3)point.Direction.normalized);
                    Gizmos.color = Color.white;
                }
            }
        }
    }
}
