using Avocado.Combat.Damage;
using System.Collections;
using UnityEngine;

namespace Avocado
{
    public class FabricMinibossController : MonoBehaviour,IDamageable
    {
        [SerializeField] D_RangedAttackState rangedAttackData;

        [SerializeField] Transform attackPoint;
        FabricEnemyCollision fabricCollision;
        Animator anim;
        float currentHealth;
        [SerializeField] float maxtHealth;
        SpriteRenderer sprite;
        IEnumerator damageEffect;
        private void Awake()
        {
            sprite = GetComponent<SpriteRenderer>();
            fabricCollision = GetComponentInChildren<FabricEnemyCollision>();
            anim = GetComponent<Animator>();
        }
        private void OnEnable()
        {
            anim.SetBool("idle", true);
            currentHealth = maxtHealth;
            fabricCollision.OnPlayerEnter += OnPlayerEnterCollision;
            fabricCollision.OnPlayerExit += OnPlayerExitCollision;

        }
        private void OnDisable()
        {
            anim.SetBool("startDeath", false);

            fabricCollision.OnPlayerEnter -= OnPlayerEnterCollision;
            fabricCollision.OnPlayerExit -= OnPlayerExitCollision;
        }
        void OnPlayerEnterCollision(Collider2D coll)
        { 
        
            anim.SetBool("attack", true);
            anim.SetBool("idle", false);
        }
        void OnPlayerExitCollision(Collider2D coll)
        { 
            anim.SetBool("attack", false);
            anim.SetBool("idle", true);
        }
        void SpawnProjectile()
        {
            GameObject bullet = FabricEnemiesPool.Instance.GetMiniBossBullet();
            bullet.transform.position = attackPoint.position;
            bullet.transform.localScale = transform.localScale*6;
            int direction = (transform.lossyScale.x > 0) ? 1 : -1;
            bullet.GetComponent<Avocado.Projectiles.Projectile>().FireProjectileWithDirection(rangedAttackData.projectileSpeed * direction, rangedAttackData.projectileTravelDistance, rangedAttackData.projectileDamage);
        }
        void IDamageable.Damage(DamageData data)
        {
            currentHealth -= data.Amount;
            if (currentHealth <= 0)
            {
                anim.SetBool("startDeath", true);
                anim.SetBool("attack", false);
                anim.SetBool("idle", false);

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
