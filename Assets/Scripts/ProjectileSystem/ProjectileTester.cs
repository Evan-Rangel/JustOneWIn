using System;
using Avocado.ObjectPoolSystem;
using Avocado.ProjectileSystem.DataPackages;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
ProjectileTester es una clase de prueba diseñada para verificar proyectiles sin necesidad de 
tener un arma implementada. Su función es:
-Instanciar o sacar un proyectil del pool (ObjectPools).
-Enviarle datos como daño, posición inicial, y rotación.
-Llamar a Init() para que sus componentes se activen.
-Disparar automáticamente cada cierto intervalo (ShotCooldown).
-Esto permite testear el comportamiento de proyectiles (como daño, rotación, seguimiento de 
objetivos, etc.) de forma aislada.
---------------------------------------------------------------------------------------------*/

namespace Avocado.ProjectileSystem
{
    public class ProjectileTester : MonoBehaviour
    {
        // Prefab del proyectil que se va a probar
        public Projectile ProjectilePrefab;

        // Datos de daño que se enviarán al proyectil al dispararlo
        public DamageDataPackage DamageDataPackage;

        // Tiempo de espera entre cada disparo
        public float ShotCooldown;

        // Sistema de pooling para evitar instanciaciones/destrucciones repetidas
        private ObjectPools objectPools = new ObjectPools();

        // Tiempo del último disparo
        private float lastFireTime;

        private void Start()
        {
            // Verifica si se asignó un prefab de proyectil
            if (!ProjectilePrefab)
            {
                Debug.LogWarning("No Projectile To Test");
                return;
            }

            // Dispara un proyectil al inicio para comenzar la prueba
            FireProjectile();
        }

        // Lógica para disparar un proyectil
        private void FireProjectile()
        {
            Debug.Log("Fire");
            // Obtiene un proyectil del pool correspondiente
            var projectile = objectPools.GetPool(ProjectilePrefab).GetObject();

            // Reinicia el estado interno del proyectil (como eventos o flags)
            projectile.Reset();

            // Lo posiciona y rota en la posición actual del tester
            projectile.transform.position = transform.position;
            projectile.transform.rotation = transform.rotation;

            // Le envía los datos necesarios para su funcionamiento (daño, dirección, etc.)
            projectile.SendDataPackage(DamageDataPackage);

            // Llama al evento Init para que los componentes del proyectil se inicialicen
            projectile.Init();

            // Registra el tiempo del disparo
            lastFireTime = Time.time;
        }

        private void Update()
        {
            // Dispara otro proyectil cuando se cumple el cooldown
            if (Time.time >= lastFireTime + ShotCooldown)
            {
                FireProjectile();
            }
        }

        // Método para destruir manualmente los pools desde el inspector
        [ContextMenu("Destroy Pools")]
        private void DestroyPool()
        {
            // Detiene los disparos y libera los objetos en los pools
            lastFireTime = Mathf.Infinity;
            objectPools.Release();
        }
    }
}
