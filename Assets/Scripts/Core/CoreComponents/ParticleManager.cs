using System;
using UnityEngine;
using Random = UnityEngine.Random; // Que random sea el de Unity y no el del sistema

/*---------------------------------------------------------------------------------------------
Este Script administra la instanciaci�n de part�culas (por ejemplo, explosiones, efectos 
visuales) de forma organizada y relativa a un contenedor especial en la escena (ParticleContainer).
Funciones principales:
-Instanciar part�culas en la posici�n del objeto o en una posici�n relativa usando offsets.
-Instanciar part�culas con rotaci�n aleatoria.
-Agrupar todas las part�culas bajo un contenedor (ParticleContainer) para mantener el 
Hierarchy limpio y ordenado.
---------------------------------------------------------------------------------------------*/

namespace Avocado.CoreSystem
{
    public class ParticleManager : CoreComponent
    {
        private Transform particleContainer; 
        private Movement movement; 

        protected override void Awake()
        {
            base.Awake();

            // Busca el contenedor de part�culas en la escena usando su tag
            particleContainer = GameObject.FindGameObjectWithTag("ParticleContainer").transform;
            DontDestroyOnLoad(particleContainer.gameObject);
        }

        private void Start()
        {
            // Obtiene la referencia al Movement
            movement = core.GetCoreComponent<Movement>();
        }

        // Instancia una part�cula en una posici�n y rotaci�n espec�ficas
        public GameObject StartParticles(GameObject particlePrefab, Vector2 position, Quaternion rotation)
        {
            return Instantiate(particlePrefab, position, rotation, particleContainer);
        }

        // Instancia una part�cula en la posici�n actual del objeto, con rotaci�n default
        public GameObject StartParticles(GameObject particlePrefab)
        {
            return StartParticles(particlePrefab, transform.position, Quaternion.identity);
        }

        // Instancia una part�cula en la posici�n actual pero con una rotaci�n aleatoria
        public GameObject StartWithRandomRotation(GameObject particlePrefab)
        {
            var randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
            return StartParticles(particlePrefab, transform.position, randomRotation);
        }

        // Instancia una part�cula con rotaci�n aleatoria en una posici�n relativa basada en un offset
        public GameObject StartWithRandomRotation(GameObject prefab, Vector2 offset)
        {
            var randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
            return StartParticles(prefab, FindRelativePoint(offset), randomRotation);
        }

        // Instancia una part�cula en una posici�n relativa y una rotaci�n espec�fica
        public GameObject StartParticlesRelative(GameObject particlePrefab, Vector2 offset, Quaternion rotation)
        {
            var pos = FindRelativePoint(offset);
            return StartParticles(particlePrefab, pos, rotation);
        }

        // Calcula una posici�n relativa al objeto teniendo en cuenta el FacingDirection
        private Vector2 FindRelativePoint(Vector2 offset) => movement.FindRelativePoint(offset);
    }
}
