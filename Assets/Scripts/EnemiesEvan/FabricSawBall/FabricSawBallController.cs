using Avocado.Combat.Damage;
using Avocado.Combat.KnockBack;
using System.Collections;
using UnityEngine;

namespace Avocado
{
    public class FabricSawBallController : MonoBehaviour, IDamageable
    {
        Animator anim;
        Rigidbody2D rb;
        SpriteRenderer sprite;
        [SerializeField] float speed;
        [SerializeField] float health;
        [SerializeField] float force;
        [SerializeField] float jumpForce;
        IEnumerator damageEffect;
        Vector2 direction;
        [SerializeField] D_MeleeAttack attackData;
        [SerializeField] Transform attackPoint;
        void IDamageable.Damage(DamageData data)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(force * direction.x, force), ForceMode2D.Impulse);
            health -= data.Amount;
            if (health <= 0)
            {
                rb.velocity = Vector2.zero;
                anim.SetBool("Idle", false);
                anim.SetBool("StartDeath", true);
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
        private void Awake()
        {
            sprite = GetComponentInChildren<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
        }
        private void OnEnable()
        {
            anim.SetBool("Idle", true);
            rb.velocity = direction * speed;
        }
        public void SetValue(int dir)
        {
            transform.localScale = new Vector3(dir, 1, 1);
            if (transform.localScale.x > 0)
                direction = Vector2.left;
            else
                direction = Vector2.right;

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


            if (rb.velocity == direction * speed)
                return;

            rb.velocity = Vector2.Lerp(rb.velocity, direction * speed, 5f * Time.deltaTime);
            if (Vector2.Distance(rb.velocity, direction * speed) < 0.01f)
                rb.velocity = direction * speed;
        }
        public void PlayerIsClose()
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(direction.y, jumpForce), ForceMode2D.Impulse);
        }
        public void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(attackPoint.position, attackData.attackRadius);
        }
    }
}
