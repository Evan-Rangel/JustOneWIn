using Avocado.Combat.Damage;
using UnityEngine;

namespace Avocado
{
    public class BreakingWallController : MonoBehaviour, IDamageable
    {
        Animator anim;
        [SerializeField] int maxHealth = 3;
         int currentHealth;
        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            currentHealth = maxHealth;
            
        }
        private void OnDisable()
        {
            
        }
        void CheckForHealth()
        {
            if (currentHealth<=0)
                anim.SetTrigger("Break");

        }
        void DisableWall()
        { 
            gameObject.SetActive(false);
        }

        void IDamageable.Damage(DamageData data)
        {
            currentHealth--;
            anim.SetTrigger("Hit");
        }
    }
}
