using System;
using Avocado.ObjectPoolSystem;
using Avocado.ProjectileSystem;
using Avocado.Weapons.Components;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este archivo define una interfaz para estrategias de generación de proyectiles (IProjectileSpawnerStrategy). 
Su propósito es permitir que diferentes clases implementen diversas formas de lanzar proyectiles, 
facilitando un diseño flexible y extensible. Por ejemplo:
-Una estrategia puede lanzar un solo proyectil (básica).
-Otra puede lanzar múltiples en abanico (como viste en ChargeProjectileSpawnerStrategy).
-Otra más puede generar proyectiles con retardo, en forma circular, etc.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons
{
    // El método 'ExecuteSpawnStrategy' se llama cuando se quiere disparar uno o más proyectiles, y proporciona toda la información necesaria
    public interface IProjectileSpawnerStrategy
    {
        void ExecuteSpawnStrategy(ProjectileSpawnInfo projectileSpawnInfo, Vector3 spawnerPos, int facingDirection, ObjectPools objectPools, Action<Projectile> OnSpawnProjectile);
    }
}
