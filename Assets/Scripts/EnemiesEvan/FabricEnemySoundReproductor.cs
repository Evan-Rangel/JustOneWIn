using System;
using UnityEngine;

namespace Avocado
{
    public class FabricEnemySoundReproductor : MonoBehaviour
    {
        [SerializeField] string fabricEnemyName;
        event Action<string, Vector3> playSound;
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
            playSound?.Invoke(fabricEnemyName + "Attack", transform.position);
        }
        public void PlayTargetFindSound()
        {

            playSound?.Invoke("FabricTargetFind", transform.position);
        }
        public void PlayTargetLostSound()
        {
            playSound?.Invoke("FabricTargetLost", transform.position);
        }
        public void PlayIdleSound()
        {
            playSound?.Invoke(fabricEnemyName + "Idle", transform.position);
        }
        public void PlayWalkSound()
        {
            playSound?.Invoke(fabricEnemyName + "Walk", transform.position);
        }
        public void PlayJumpSound()
        {
            Debug.Log("PlayJumpSound called");
            playSound?.Invoke(fabricEnemyName + "Jump", transform.position);
        }
        public void PlaylandSound()
        {
            playSound?.Invoke(fabricEnemyName + "Land", transform.position);
        }
        public void PlayDamageSound()
        {
            playSound?.Invoke("FabricDamage", transform.position);
        }
        public void PlayDeathSound()
        {
            playSound?.Invoke("FabricDeath", transform.position);
        }
    }
}
