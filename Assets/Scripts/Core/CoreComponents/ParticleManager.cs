using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.CoreSystem
{
    public class ParticleManager : CoreComponent
    {
        #region References
        #endregion

        #region Integers
        #endregion

        #region Floats
        #endregion

        #region Flags
        #endregion

        #region Components
        #endregion

        #region Transforms
        private Transform particleContainer;
        #endregion

        #region Vectors
        #endregion

        #region Unity CallBack Functions Override
        protected override void Awake()
        {
            base.Awake();

            particleContainer = GameObject.FindGameObjectWithTag("ParticleContainer").transform;
        }
        #endregion

        #region Own Functions
        public GameObject StartParticles(GameObject particlePrefab, Vector2 position, Quaternion rotation)
        {
            return Instantiate(particlePrefab, position, rotation, particleContainer);
        }

        public GameObject StartParticles(GameObject particlePrefab)
        {
            return StartParticles(particlePrefab, transform.position, Quaternion.identity);
        }

        public GameObject StartParticlesWithRandomRotation(GameObject particlePrefab)
        {
            var randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
            return StartParticles(particlePrefab, transform.position, randomRotation);
        }
        #endregion

        #region Check Functions
        #endregion

        #region Other Functions
        #endregion
    }
}
