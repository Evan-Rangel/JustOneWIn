using Avocado.Combat.Damage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class FabricBossController : MonoBehaviour, IDamageable
    {
        [SerializeField] D_RangedAttackState rangedAttackData;

        [SerializeField] Transform attackPoint;
        FabricEnemyCollision fabricCollision;
        Animator anim;
        float currentHealth;
        [SerializeField] float maxtHealth;
        SpriteRenderer sprite;
        IEnumerator damageEffect;



        void IDamageable.Damage(DamageData data)
        {
            currentHealth -= data.Amount;
            if (currentHealth <= 0)
            {
                anim.SetBool("startDeath", true);

                return;
            }
            if (damageEffect != null)
                StopCoroutine(damageEffect);
            damageEffect = DamageEffect();
            StartCoroutine(damageEffect);
        }
        IEnumerator DamageEffect()
        {
            sprite.enabled = false;

            yield return Helpers.GetWait(0.1f);

            sprite.enabled = true;

            yield return Helpers.GetWait(0.1f);

            sprite.enabled = false;

            yield return Helpers.GetWait(0.1f);

            sprite.enabled = true;
        }
        void DestroyEnemy()
        {
            for (int i = 0; i < 50; i++)
            {
                GameObject coin = GameManager.instance.RequestCoin();
                coin.transform.position = transform.position;
                coin.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(1f, 3f)), ForceMode2D.Impulse);
            }
            anim.SetBool("startDeath", false);
            gameObject.SetActive(false);
        }

    }
}
