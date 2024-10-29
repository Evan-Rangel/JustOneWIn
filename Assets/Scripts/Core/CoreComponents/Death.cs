using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.CoreSystem
{
    public class Death : CoreComponent
    {
        #region References
        private ParticleManager ParticleManager => particleManager ? particleManager : core.GetCoreComponent(ref particleManager);
        private ParticleManager particleManager;

        private Stats Stats => stats ? stats : core.GetCoreComponent(ref stats);
        private Stats stats;
        #endregion

        #region GameObjects
        [SerializeField] private GameObject[] deathParticles;
        #endregion

        #region Unity CallBack Functions Override
        protected override void Awake()
        {
            base.Awake();
        }
        #endregion

        #region Own Functions
        public void Die()
        {
            foreach (var particle in deathParticles)
            {
                ParticleManager.StartParticles(particle);
            }

            core.transform.parent.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            Stats.Health.OnCurrentValueZero += Die;
        }

        private void OnDisable()
        {
            Stats.Health.OnCurrentValueZero -= Die;
        }
        #endregion

        #region Other Functions
        #endregion
    }
}
