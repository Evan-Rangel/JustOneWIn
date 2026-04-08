using Avocado.Combat.Damage;
using Avocado.Weapons.Components;
using System.Collections;
using UnityEngine;

namespace Avocado
{
    public class FabricMinibossController : MonoBehaviour, IDamageable
    {


        FabricEnemySoundReproductor soundReproductor;
        [SerializeField] float minRandomTimeLapseIdleSound, maxRandomTimeLapseIdleSound;
        void IdleSound()
        {
            soundReproductor.PlayIdleSound();
            Invoke(nameof(IdleSound), Random.Range(minRandomTimeLapseIdleSound, maxRandomTimeLapseIdleSound));
        }



        [SerializeField] D_RangedAttackState rangedAttackData;
        [SerializeField] D_MeleeAttack meleeAttackData;
        [SerializeField] Transform meleeAttackPoint;

        [SerializeField] Transform attackPoint;
        FabricEnemyCollision fabricCollision;
        Animator anim;
        float currentHealth;
        [SerializeField] float maxtHealth;
        SpriteRenderer sprite;
        IEnumerator damageEffect;
        private void Awake()
        {
            soundReproductor = GetComponent<FabricEnemySoundReproductor>();
            sprite = GetComponent<SpriteRenderer>();
            fabricCollision = GetComponentInChildren<FabricEnemyCollision>();
            anim = GetComponent<Animator>();
        }
        private void OnEnable()
        {
            anim.SetBool("idle", true);
            CancelInvoke(nameof(IdleSound));
            currentHealth = maxtHealth;
            fabricCollision.OnPlayerEnter += OnPlayerEnterCollision;
            fabricCollision.OnPlayerExit += OnPlayerExitCollision;

        }
        private void Update()
        {
            Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(meleeAttackPoint.position, meleeAttackData.attackRadius, meleeAttackData.whatIsPlayer);
            foreach (Collider2D collider in detectedObjects)
            {
                IDamageable damageable = collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.Damage(new DamageData(meleeAttackData.attackDamage, gameObject));
                }
                IKnockBackable knockBackable = collider.GetComponent<IKnockBackable>();
                if (knockBackable != null)
                {
                    int direction = (transform.position.x - collider.transform.position.x > 0) ? -1 : 1;
                    knockBackable.KnockBack(new Combat.KnockBack.KnockBackData(meleeAttackData.knockbackAngle, meleeAttackData.knockbackStrength, direction, gameObject));
                }

            }
        }
        private void OnDisable()
        {
            anim.SetBool("startDeath", false);
            fabricCollision.OnPlayerEnter -= OnPlayerEnterCollision;
            fabricCollision.OnPlayerExit -= OnPlayerExitCollision;
        }
        void OnPlayerEnterCollision(Collider2D coll)
        {
            soundReproductor.PlayTargetFindSound();

            CancelInvoke(nameof(IdleSound));

            anim.SetBool("attack", true);
            anim.SetBool("idle", false);
        }
        void OnPlayerExitCollision(Collider2D coll)
        {
            Invoke(nameof(IdleSound), Random.Range(minRandomTimeLapseIdleSound, maxRandomTimeLapseIdleSound));

            anim.SetBool("attack", false);
            anim.SetBool("idle", true);
        }
        void SpawnProjectile()
        {
            GameObject bullet = FabricEnemiesPool.Instance.GetMiniBossBullet();
            bullet.transform.position = attackPoint.position;
            //bullet.transform.localScale = transform.localScale*6;
            int direction = (transform.lossyScale.x > 0) ? 1 : -1;
            bullet.GetComponent<Avocado.Projectiles.Projectile>().FireProjectileWithDirection(rangedAttackData.projectileSpeed * direction, rangedAttackData.projectileTravelDistance, rangedAttackData.projectileDamage, rangedAttackData.initSound, rangedAttackData.hitSound);
        }
        void IDamageable.Damage(DamageData data)
        {
            currentHealth -= data.Amount;
            if (currentHealth <= 0)
            {
                soundReproductor.PlayDeathSound();

                anim.SetBool("startDeath", true);
                anim.SetBool("attack", false);
                anim.SetBool("idle", false);

                return;
            }
            soundReproductor.PlayDamageSound();

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
            for (int i = 0; i <  15; i++)
            {
                GameObject coin = GameManager.instance.RequestCoin();
                coin.transform.position = transform.position;
                coin.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(1f, 3f)), ForceMode2D.Impulse);
            }
            anim.SetBool("startDeath", false);
            gameObject.SetActive(false);
        }
        public void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(meleeAttackPoint.position, meleeAttackData.attackRadius);
        }

    }
}
