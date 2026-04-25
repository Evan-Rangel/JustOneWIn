using Avocado.Combat.Damage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Avocado
{
    public class EnergyGeneratorController : MonoBehaviour, IDamageable
    {
        Animator anim;
        event Action<string, Transform> OnPlaySound;
        [SerializeField] string hitSound, breakingSound, eventName;
        [SerializeField] UnityEvent OnBreak;
        [SerializeField] int maxHealth = 3;
        Collider2D col;
        int currentHealth;

        private void Awake()
        {
            col = GetComponent<Collider2D>();
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
        void IDamageable.Damage(DamageData data)
        {
            OnPlaySound?.Invoke(hitSound, transform);
            currentHealth--;
            anim.SetTrigger("Hit");
        }

        void CheckForHealth()
        {
            if (currentHealth > 0) return;
            OnPlaySound?.Invoke(breakingSound, transform);
            OnBreak?.Invoke();
            anim.SetTrigger("Break");
            col.enabled = false;
        }
    }
}
