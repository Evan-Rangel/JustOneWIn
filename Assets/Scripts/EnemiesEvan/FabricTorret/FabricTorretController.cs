using Avocado.Combat.Damage;
using Avocado.ProjectileSystem.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{

    public class FabricTorretController : MonoBehaviour, IDamageable
    {
        Animator anim;
        [SerializeField] GameObject canon;
        [SerializeField] GameObject coll;
        IEnumerator damageEffect;
        [SerializeField]SpriteRenderer[] sprites;
        [SerializeField] float maxHealth;
        float currentHealth;
        private void Awake()
        {
            currentHealth = maxHealth;
            anim = GetComponent<Animator>();
        }
         void DestroyTorret()
        { 
            canon.SetActive(false);
            coll.SetActive(false);
            anim.enabled = true;
        }

        public void DisableTorret()
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject coin = GameManager.instance.RequestCoin() ;
                coin.transform.position = transform.position;
                coin.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(1f, 3f)), ForceMode2D.Impulse);
            }
            currentHealth = maxHealth;
            anim.enabled = false;
            canon.SetActive(true);
            coll.SetActive(true);
            canon.transform.parent = transform;
            gameObject.SetActive(false);
        }

      
        IEnumerator DamageEffect()
        {
            foreach (SpriteRenderer sprite in sprites)
            {
                sprite.enabled = false;
            }
            yield return Helpers.GetWait(0.1f);

            foreach (SpriteRenderer sprite in sprites)
            {
                sprite.enabled = true;
            }
            yield return Helpers.GetWait(0.1f);

            foreach (SpriteRenderer sprite in sprites)
            {
                sprite.enabled = false;
            }
            yield return Helpers.GetWait(0.1f);

            foreach (SpriteRenderer sprite in sprites)
            {
                sprite.enabled = true;
            }
        }
        void IDamageable.Damage(DamageData data)
        {
            currentHealth -= data.Amount;
            if (currentHealth <= 0)
            {
                DestroyTorret();
                return;
            }
            if (damageEffect != null)
                StopCoroutine(damageEffect);
            damageEffect = DamageEffect();
            StartCoroutine(damageEffect);
        }
    }

    
}
