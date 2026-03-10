using Avocado.Combat.Damage;
using System;
using UnityEngine;

namespace Avocado
{
    public class BreakingWallController : MonoBehaviour, IDamageable
    {
        Animator anim;
        [SerializeField] int maxHealth = 3;
        [SerializeField] string hitSound, breakingSound;  
        event Action<string, Transform> OnPlaySound;
        int currentHealth;
        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            OnPlaySound += AudioManager.instance.PlaySFXSound;
            currentHealth = maxHealth;
        }
        private void OnDisable()
        {
            OnPlaySound -= AudioManager.instance.PlaySFXSound;
        }
        void CheckForHealth()
        {
            if (currentHealth > 0) return;
            OnPlaySound?.Invoke(breakingSound, transform);
            anim.SetTrigger("Break");

        }
        void DisableWall()
        { 
            gameObject.SetActive(false);
        }

        void IDamageable.Damage(DamageData data)
        {
            OnPlaySound?.Invoke (hitSound, transform);
            currentHealth--;
            anim.SetTrigger("Hit");
        }
    }
}
