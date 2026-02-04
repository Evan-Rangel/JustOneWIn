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
        private SoundReproductor sound;
        // Método llamado para "matar" el objeto.
        // Lanza partículas de muerte y desactiva el GameObject raíz.
        public void Die()
        {

            if (sound == null)sound = core.Root.GetComponent<SoundReproductor>();

                sound.PlayDeathSound();

            // Iniciar partículas de muerte
            foreach (var particle in deathParticles)
            {
                ParticleManager.StartParticles(particle);
            }
            FakeLight_S.instance.ShadeEffect(); 
            Invoke("ResetLevel", 1.5f);

            // Desactivar el objeto completo
            core.transform.parent.gameObject.SetActive(false);
        }
        void ResetLevel()
        {
            GameManager.instance.ResetLevel();
        }

        // Suscribe el método Die al evento de vida en cero.
        private void OnEnable()
        {
            Invoke("DelayOnEnable", 0.1f);
        }
        void DelayOnEnable()
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
