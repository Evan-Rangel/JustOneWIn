using Avocado.Combat.Damage;
using Avocado.Combat.KnockBack;
using System.Collections;
using UnityEngine;

namespace Avocado
{
    public class FabricSearcherController : MonoBehaviour, IDamageable
    {


        FabricEnemySoundReproductor soundReproductor;
        [SerializeField] float minRandomTimeLapseIdleSound, maxRandomTimeLapseIdleSound;
        void IdleSound()
        {
            soundReproductor.PlayIdleSound();
            Invoke(nameof(IdleSound), Random.Range(minRandomTimeLapseIdleSound, maxRandomTimeLapseIdleSound));
        }

        public void PlayWalkSound()
        {
            soundReproductor.PlayWalkSound();

        }
        Animator anim;
        Rigidbody2D rb;
        SpriteRenderer sprite;
        [SerializeField] float speed;
        float currentHealth;
        [SerializeField] float maxHealth;
        [SerializeField] float force;
        [SerializeField] float jumpForce;
        IEnumerator damageEffect;
        Vector2 direction;
        [SerializeField] D_MeleeAttack attackData;
        [SerializeField] Transform attackPoint;
        void IDamageable.Damage(DamageData data)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(force* direction.x, force), ForceMode2D.Impulse);
            currentHealth -= data.Amount;
            if (currentHealth <= 0)
            {
                soundReproductor.PlayDeathSound();
                rb.velocity = Vector2.zero;
                anim.SetBool("run", false);
                anim.SetBool("StartDeath", true);
                return;
            }
            soundReproductor.PlayDamageSound();
            if (damageEffect != null)
                StopCoroutine(damageEffect);
            damageEffect = DamageEffect();
            StartCoroutine(damageEffect);
        }
        public void SetValue(int dir)
        {
            transform.localScale = new Vector3(dir, 1, 1);
            if (transform.localScale.x > 0)
                direction = Vector2.left;
            else
                direction = Vector2.right;
            rb.velocity = direction * speed;


        }
        private void Awake()
        {
            soundReproductor = GetComponent<FabricEnemySoundReproductor>(); 
            sprite = GetComponentInChildren<SpriteRenderer>();
            anim = GetComponent<Animator>();
             rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            currentHealth = maxHealth;
            anim.SetBool("run", true);
            rb.velocity = direction * speed;
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
                    knockBackable.KnockBack(new KnockBackData(attackData.knockbackAngle, attackData.knockbackStrength, -(int)transform.localScale.x, gameObject));
                }
            }


            if (   rb.velocity == direction * speed)
                return;

            rb.velocity= Vector2.Lerp(rb.velocity, direction * speed, 5f * Time.deltaTime);
            if (Vector2.Distance(rb.velocity, direction *speed) < 0.01f)
                rb.velocity = direction * speed;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("EnemyJump"))
            {
                soundReproductor.PlayJumpSound();
                rb.velocity = Vector2.zero;
                rb.AddForce(new Vector2(direction.x, jumpForce), ForceMode2D.Impulse);
            } 
            if (collision.CompareTag("SearcherDespawner"))
            {
                rb.velocity = Vector2.zero;
                gameObject.SetActive(false);
            }
        }
        void DestroyEnemy()
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject coin = GameManager.instance.RequestCoin();
                coin.transform.position = transform.position;
                coin.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(1f, 3f)), ForceMode2D.Impulse);
            }
            anim.SetBool("StartDeath", false);

            gameObject.SetActive(false);
        }
        public void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(attackPoint.position, attackData.attackRadius);
        }
    }
}
