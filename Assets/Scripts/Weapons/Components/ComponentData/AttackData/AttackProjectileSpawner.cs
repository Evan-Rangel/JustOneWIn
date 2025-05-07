using System;
using Avocado.ProjectileSystem;
using Avocado.ProjectileSystem.DataPackages;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este sistema permite a un ataque generar uno o varios proyectiles personalizados. Cada ProjectileSpawnInfo especifica:
-Dónde aparecerá el proyectil (Offset),
-Hacia dónde se moverá (Direction),
-Qué prefab de Projectile se usará,
-Qué datos de daño, retroceso, poise y sprite se le deben pasar.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    [Serializable]
    public class AttackProjectileSpawner : AttackData
    {
        // Cada ataque puede generar múltiples proyectiles, por eso es un arreglo.
        [field: SerializeField] public ProjectileSpawnInfo[] SpawnInfos { get; private set; }
    }

    [Serializable]
    public struct ProjectileSpawnInfo
    {
        // Posición desde donde se generará el proyectil, relativa al personaje.
        [field: SerializeField] public Vector2 Offset { get; private set; }

        // Dirección en la que el proyectil será lanzado, relativa a la orientación del personaje.
        [field: SerializeField] public Vector2 Direction { get; private set; }

        // Prefab del proyectil que se va a instanciar. Es del tipo Projectile, no GameObject.
        [field: SerializeField] public Projectile ProjectilePrefab { get; private set; }

        // Datos de daño que se le pasarán al proyectil al generarse.
        [field: SerializeField] public DamageDataPackage DamageData { get; private set; }

        // Datos de retroceso (knockback) que se aplicarán cuando el proyectil impacte.
        [field: SerializeField] public KnockBackDataPackage KnockBackData { get; private set; }

        // Datos de daño a la poise (estabilidad) que se aplicarán al impactar.
        [field: SerializeField] public PoiseDamageDataPackage PoiseDamageData { get; private set; }

        // Datos relacionados al sprite que usará el proyectil.
        [field: SerializeField] public SpriteDataPackage SpriteDataPackage { get; private set; }
    }
}
