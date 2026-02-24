using Avocado.Combat.Damage;
using Avocado.Combat.KnockBack;
using System.Collections;
using UnityEngine;

namespace Avocado
{
    public class FabricJumperController : MonoBehaviour, IDamageable
    {
        [SerializeField] D_MeleeAttack attackData;
        [SerializeField] float playerDistanceToJump = 5f;  
        [SerializeField] Vector2 playerForceJump;
        [SerializeField] LayerMask groundLayer;
        Rigidbody2D rb;
        [SerializeField] Transform attackPoint;
        FabricEnemyCollision fabricCollision;
        Animator anim;
        float currentHealth;
        [SerializeField] float maxtHealth;
        SpriteRenderer sprite;
        IEnumerator damageEffect;
        Transform playerPos;
        bool onGround;
        bool jumping;
        [SerializeField] float jumpDuration = 2f;
        FabricEnemySoundReproductor soundReproductor;
        private void Awake()
        {
            soundReproductor = GetComponent<FabricEnemySoundReproductor>();
            sprite = GetComponent<SpriteRenderer>(); 
            fabricCollision = GetComponentInChildren<FabricEnemyCollision>();
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }
        private void OnEnable()
        {
            currentHealth = maxtHealth;
            onGround = true;
            jumping = false;
            fabricCollision.OnPlayerEnter += OnPayerEnterCollision;
            fabricCollision.OnPlayerExit += OnPayerExitCollision;
            rb.constraints = RigidbodyConstraints2D.None| RigidbodyConstraints2D.FreezeRotation;

        }
        private void OnDisable()
        {
           fabricCollision.OnPlayerEnter -= OnPayerEnterCollision;
            fabricCollision.OnPlayerExit -= OnPayerExitCollision;
            anim.SetBool("startDeath", false);
            CancelInvoke(nameof(ActivateJumping));

        }
        private void Update()
        {
            if (currentHealth <= 0)
                return;
            if (playerPos != null && Vector2.Distance(playerPos.position, transform.position)<playerDistanceToJump && !jumping)
            {
                Jump();
            }
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
                    knockBackable.KnockBack(new KnockBackData(attackData.knockbackAngle, attackData.knockbackStrength, (int)transform.localScale.x, gameObject));
                }
            }
        }
        void Jump()
        {
            if (!onGround) return;
            soundReproductor.PlayAttackSound();
            jumping = true;
            onGround = false;
            anim.SetBool("preJump", false);
            anim.SetBool("jump", true);
            int direction =(int)transform.localScale.x;//(transform.position.x-playerPos.position.x>0)? -1: 1;
            rb.AddForce(new Vector2(direction * playerForceJump.x, playerForceJump.y), ForceMode2D.Impulse);

            Invoke(nameof(ActivateJumping), jumpDuration);

        }
        void ActivateJumping()
        {
 
            jumping = false;
        }
        
        void OnPayerEnterCollision(Collider2D coll)
        {
            playerPos = coll.transform;
            anim.SetBool("preJump",true);
            anim.SetBool("idle",false);
        }
        void OnPayerExitCollision(Collider2D coll)
        {
            playerPos = null;
            anim.SetBool("preJump", false);
            anim.SetBool("idle", true);
        }
        void IDamageable.Damage(DamageData data)
        {
            rb.velocity = Vector2.zero;
            int direction= (transform.position.x - data.Source.transform.position.x > 0) ? -1 : 1;
            rb.AddForce(new Vector2(-direction*5, 5), ForceMode2D.Impulse);

            currentHealth -= data.Amount;
            if (currentHealth <= 0)
            {
                rb.velocity = Vector2.zero;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                anim.SetBool("startDeath", true);
                anim.SetBool("idle", false);
                anim.SetBool("jump", false);
                anim.SetBool("preJump", false);
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
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.name=="Platforms")
            {
                rb.velocity = Vector2.zero;
                onGround = true;
                anim.SetBool("jump", false);

            }
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.transform.name == "Platforms")
            {
                anim.SetBool("preJump", playerPos!=null);
                anim.SetBool("idle", playerPos == null);
                onGround = false;
            }
        }
        public void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(attackPoint.position, attackData.attackRadius);
        }
    }
}
