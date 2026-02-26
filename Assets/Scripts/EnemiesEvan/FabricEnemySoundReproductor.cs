using System;
using UnityEngine;

namespace Avocado
{
    public class FabricEnemySoundReproductor : MonoBehaviour
    {
        [SerializeField] string fabricEnemyName;
        event Action<string, Transform> playSound;
        private void OnEnable()
        {
            playSound += AudioManager.instance.PlaySFXSound;
        }
        private void OnDisable()
        {
            playSound -= AudioManager.instance.PlaySFXSound;
        }

        public void PlayAttackSound()
        {
            playSound?.Invoke(fabricEnemyName + "Attack", transform);
        }
        public void PlayTargetFindSound()
        {

            playSound?.Invoke("FabricTargetFind", transform);
        }
        public void PlayTargetLostSound()
        {
            playSound?.Invoke("FabricTargetLost", transform);
        }
        public void PlayIdleSound()
        {
            playSound?.Invoke(fabricEnemyName + "Idle", transform);
        }
        public void PlayWalkSound()
        {
            playSound?.Invoke(fabricEnemyName + "Walk", transform);
        }
        public void PlayJumpSound()
        {
            playSound?.Invoke(fabricEnemyName + "Jump", transform);
        }
        public void PlaylandSound()
        {
            playSound?.Invoke(fabricEnemyName + "Land", transform);
        }
        public void PlayDamageSound()
        {
            playSound?.Invoke("FabricDamage", transform);
        }
        public void PlayDeathSound()
        {
            playSound?.Invoke("FabricDeath", transform);
        }
    }
}
