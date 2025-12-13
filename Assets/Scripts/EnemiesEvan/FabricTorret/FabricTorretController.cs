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
        [SerializeField] GameObject explosiveEffect;
        [SerializeField] GameObject coll;
        IEnumerator damageEffect;
        [SerializeField]SpriteRenderer[] sprites;
        [SerializeField] float health;
        private void Awake()
        {
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
            Destroy(canon);
            Destroy(gameObject);
            //gameObject.SetActive(false);
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
            health-=data.Amount;
            if (health <= 0)
            {
                explosiveEffect.SetActive(true);
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
