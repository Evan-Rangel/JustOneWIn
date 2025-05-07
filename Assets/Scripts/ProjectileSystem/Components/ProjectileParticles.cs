using UnityEngine;

/*---------------------------------------------------------------------------------------------
ProjectileParticles es un componente encargado de instanciar efectos visuales (partículas) 
cuando un proyectil impacta algo. Tiene sobrecargas para manejar tanto un solo RaycastHit2D 
como múltiples, y también permite especificar directamente una posición y rotación. Calcula la 
orientación correcta de las partículas para que coincidan con la dirección del impacto, mejorando 
así la fidelidad visual.
---------------------------------------------------------------------------------------------*/

namespace Avocado.ProjectileSystem.Components
{
    public class ProjectileParticles : MonoBehaviour
    {
        // Referencia al sistema de partículas que se usará cuando el proyectil impacte
        [SerializeField] private ParticleSystem impactParticles;

        // Instancia el sistema de partículas en una posición y rotación específicas
        public void SpawnImpactParticles(Vector3 position, Quaternion rotation)
        {
            Instantiate(impactParticles, position, rotation);
        }

        // Variante que recibe un RaycastHit2D para determinar la posición y orientación del impacto
        public void SpawnImpactParticles(RaycastHit2D hit)
        {
            // Calcula la rotación para que las partículas se orienten correctamente según la normal del impacto
            var rotation = Quaternion.FromToRotation(transform.right, hit.normal);

            SpawnImpactParticles(hit.point, rotation);
        }

        // Variante que recibe un array de impactos y genera partículas en el primer impacto detectado
        public void SpawnImpactParticles(RaycastHit2D[] hits)
        {
            // Si no hay impactos, no se hace nada
            if (hits.Length <= 0)
                return;

            // Reutiliza la función anterior con el primer impacto
            SpawnImpactParticles(hits[0]);
        }
    }
}
