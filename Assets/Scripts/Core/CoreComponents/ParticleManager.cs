using System;
using UnityEngine;
using Random = UnityEngine.Random; // Que random sea el de Unity y no el del sistema

/*---------------------------------------------------------------------------------------------
Este Script administra la instanciación de partículas (por ejemplo, explosiones, efectos 
visuales) de forma organizada y relativa a un contenedor especial en la escena (ParticleContainer).
Funciones principales:
-Instanciar partículas en la posición del objeto o en una posición relativa usando offsets.
-Instanciar partículas con rotación aleatoria.
-Agrupar todas las partículas bajo un contenedor (ParticleContainer) para mantener el 
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

            // Busca el contenedor de partículas en la escena usando su tag
            particleContainer = GameObject.FindGameObjectWithTag("ParticleContainer").transform;
            DontDestroyOnLoad(particleContainer.gameObject);
        }

        private void Start()
        {
            // Obtiene la referencia al Movement
            movement = core.GetCoreComponent<Movement>();
        }

        // Instancia una partícula en una posición y rotación específicas
        public GameObject StartParticles(GameObject particlePrefab, Vector2 position, Quaternion rotation)
        {
            return Instantiate(particlePrefab, position, rotation, particleContainer);
        }

        // Instancia una partícula en la posición actual del objeto, con rotación default
        public GameObject StartParticles(GameObject particlePrefab)
        {
            return StartParticles(particlePrefab, transform.position, Quaternion.identity);
        }

        // Instancia una partícula en la posición actual pero con una rotación aleatoria
        public GameObject StartWithRandomRotation(GameObject particlePrefab)
        {
            var randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
            return StartParticles(particlePrefab, transform.position, randomRotation);
        }

        // Instancia una partícula con rotación aleatoria en una posición relativa basada en un offset
        public GameObject StartWithRandomRotation(GameObject prefab, Vector2 offset)
        {
            var randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
            return StartParticles(prefab, FindRelativePoint(offset), randomRotation);
        }

        // Instancia una partícula en una posición relativa y una rotación específica
        public GameObject StartParticlesRelative(GameObject particlePrefab, Vector2 offset, Quaternion rotation)
        {
            var pos = FindRelativePoint(offset);
            return StartParticles(particlePrefab, pos, rotation);
        }

        // Calcula una posición relativa al objeto teniendo en cuenta el FacingDirection
        private Vector2 FindRelativePoint(Vector2 offset) => movement.FindRelativePoint(offset);
    }
}
