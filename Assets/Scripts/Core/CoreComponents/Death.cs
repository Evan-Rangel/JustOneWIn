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

        // M�todo llamado para "matar" el objeto.
        // Lanza part�culas de muerte y desactiva el GameObject ra�z.
        public void Die()
        {
            // Iniciar part�culas de muerte
            foreach (var particle in deathParticles)
            {
                ParticleManager.StartParticles(particle);
            }

            // Desactivar el objeto completo
            core.transform.parent.gameObject.SetActive(false);
        }

        // Suscribe el m�todo Die al evento de vida en cero.
        private void OnEnable()
        {
            Stats.Health.OnCurrentValueZero += Die;
        }

        // Desuscribe el m�todo Die para evitar problemas cuando el objeto se desactive.
        private void OnDisable()
        {
            Stats.Health.OnCurrentValueZero -= Die;
        }
    }
}
