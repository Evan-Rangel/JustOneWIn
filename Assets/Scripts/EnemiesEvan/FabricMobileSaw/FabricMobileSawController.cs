using Avocado.Combat.Damage;
using Avocado.Combat.KnockBack;
using Avocado.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class FabricMobileSawController : MonoBehaviour, IDamageable
    {
        [SerializeField] Vector2[] movePoints;
        List<Vector2> targetPoints;
        Vector2 currentPoint;
        Animator anim;
        [SerializeField] D_MeleeAttack attackData;
        [SerializeField]float minVelocity, maxVelocity;
       [SerializeField] float currentVelocity;
        [SerializeField] Transform attackPoint;
        FabricEnemyCollision fabricCollision;

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
            fabricCollision.transform.position = transform.position;    
            fabricCollision.transform.parent = transform.parent;
            currentHealth = maxtHealth;
            fabricCollision.OnPlayerEnter += MaxVelocity;
            fabricCollision.OnPlayerExit += MinVelocity;
            targetPoints= new List<Vector2>();
            for (int i = 0; i < movePoints.Length; i++)
            {
                targetPoints.Add(movePoints[i] + (Vector2)transform.position);
            }
            MinVelocity(null);
            currentPoint =targetPoints[0];
        }
        private void OnDisable()
        {
            fabricCollision.OnPlayerEnter -= MaxVelocity;
            fabricCollision.OnPlayerExit -= MinVelocity;
        }
        private void Update()
        {
            Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackData.attackRadius, attackData.whatIsPlayer);

            foreach (Collider2D collider in detectedObjects)
            {
                IDamageable damageable = collider.GetComponent<IDamageable>();

                if (damageable != null)
                {
                    damageable.Damage(new DamageData(attackData.attackDamage, gameObject));
                }

                IKnockBackable knockBackable = collider.GetComponent<IKnockBackable>();

                if (knockBackable != null)
                {
                    int direction = (transform.position.x - collider.transform.position.x > 0) ? -1 : 1;

                    knockBackable.KnockBack(new KnockBackData(attackData.knockbackAngle, attackData.knockbackStrength, direction, gameObject));
                }
            }


            if (currentPoint == Vector2.zero) return;

            transform.position = Vector2.MoveTowards(transform.position, currentPoint, currentVelocity * Time.deltaTime);
            if (Vector2.Distance(transform.position, currentPoint) < 0.1f)
            {
                currentPoint = (currentPoint == targetPoints[0]) ? targetPoints[1]  : targetPoints[0];
            }
        }


        public void MaxVelocity(Collider2D coll)
        {
            anim.SetBool("isFast",true);
            anim.SetBool("isNormal",false);
            currentVelocity = maxVelocity;
        }
        public void MinVelocity(Collider2D coll)
        {
            anim.SetBool("isFast", false);
            anim.SetBool("isNormal", true);
            currentVelocity = minVelocity;
        }
        void IDamageable.Damage(DamageData data)
        {
            currentHealth -= data.Amount;
            if (currentHealth <= 0)
            {
                anim.SetBool("isNormal", false);
                anim.SetBool("isFast", false);
                anim.SetBool("startDeath", true);
                currentPoint = Vector2.zero;
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
            for (int i = 0; i < 5; i++)
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
            Gizmos.DrawWireSphere(attackPoint.position, attackData.attackRadius);

            if (targetPoints != null && targetPoints.Count > 0)
            {
                for (int i = 0; i < targetPoints.Count; i++)
                {
                    Gizmos.DrawLine(transform.position, targetPoints[i]);
                    Gizmos.DrawWireSphere(targetPoints[i], 0.1f);
                }
            }
            else
            {
                for (int i = 0; i < movePoints.Length; i++)
                {
                    Gizmos.DrawLine(transform.position, movePoints[i] + (Vector2)transform.position);
                    Gizmos.DrawWireSphere(movePoints[i] + (Vector2)transform.position,0.1f);
                }
            }
        }
    }
}
