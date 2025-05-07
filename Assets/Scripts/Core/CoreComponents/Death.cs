using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script es el encargado de manejar la muerte del jugador, lanzando las particulas y
desactivando todo de la "root".
---------------------------------------------------------------------------------------------*/

namespace Avocado.CoreSystem
{
    public class Death : CoreComponent
    {
        [SerializeField] private GameObject[] deathParticles;

        private ParticleManager ParticleManager => particleManager ? particleManager : core.GetCoreComponent(ref particleManager);

        private ParticleManager particleManager;

        private Stats Stats => stats ? stats : core.GetCoreComponent(ref stats);

        private Stats stats;

        // Método llamado para "matar" el objeto.
        // Lanza partículas de muerte y desactiva el GameObject raíz.
        public void Die()
        {
            // Iniciar partículas de muerte
            foreach (var particle in deathParticles)
            {
                ParticleManager.StartParticles(particle);
            }

            // Desactivar el objeto completo
            core.transform.parent.gameObject.SetActive(false);
        }

        // Suscribe el método Die al evento de vida en cero.
        private void OnEnable()
        {
            Stats.Health.OnCurrentValueZero += Die;
        }

        // Desuscribe el método Die para evitar problemas cuando el objeto se desactive.
        private void OnDisable()
        {
            Stats.Health.OnCurrentValueZero -= Die;
        }
    }
}
