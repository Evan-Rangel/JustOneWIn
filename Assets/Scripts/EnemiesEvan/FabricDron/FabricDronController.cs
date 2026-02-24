using Avocado.Combat.Damage;
using System.Collections;
using UnityEngine;

namespace Avocado
{
    public class FabricDronController : MonoBehaviour, IDamageable
    {
        FabricEnemySoundReproductor soundReproductor;
        [SerializeField] float minRandomTimeLapseIdleSound, maxRandomTimeLapseIdleSound;
        void IdleSound()
        { 
            soundReproductor.PlayIdleSound();
            Invoke(nameof(IdleSound), Random.Range(minRandomTimeLapseIdleSound, maxRandomTimeLapseIdleSound));
        }




        [SerializeField] float shootingTime;   

        FabricEnemyCollision fabricCollision;
        Rigidbody2D rb;
        [SerializeField] LayerMask playerLayer;
        [SerializeField] LayerMask groundLayer;
        Vector3 startPos;
        Transform player;
        bool canFollowPlayer;
         float currentHealht;
        [SerializeField] float maxHealth;
        SpriteRenderer sprite;
        IEnumerator damageEffect;
        [SerializeField] GameObject explosionEffect;
        [SerializeField] float maxVerticalDistance;
        [SerializeField] float maxHorizontalDistance;
        [SerializeField] float verticalSpeed;
        [SerializeField] float horizontalSpeed;
        int verticalDirection;
        int horizontalDirection;
        float currentVerticalVelocity;
        float currentHorizontalVelocity;
        bool verticalCheck;
        bool horizontalCheck;
        [SerializeField] float followToPlayerSpeed;
        Vector2 randomTargetPosition ;


        [SerializeField] Transform bulletSpawn;
        [SerializeField, Range(0, 10)] int numberOfBullets;
        [SerializeField, Range(0,90)] float angleOfBullets;
        [SerializeField] D_RangedAttackState rangedAttackData;
        Animator anim;
        private void Awake()
        {
            soundReproductor=GetComponent<FabricEnemySoundReproductor>();
            fabricCollision =GetComponentInChildren<FabricEnemyCollision>();
            sprite = GetComponent<SpriteRenderer>();
            anim =GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }
        
        private void OnEnable()
        {
            Invoke(nameof(IdleSound), Random.Range(minRandomTimeLapseIdleSound, maxRandomTimeLapseIdleSound));
            verticalDirection = 1;
            horizontalDirection = 1;
            currentHealht = maxHealth;
            fabricCollision.OnPlayerEnter +=TargetFinded;
            fabricCollision.OnPlayerExit +=TargetLost;
            startPos = transform.position;
            verticalCheck = true;
            horizontalCheck = true;
            currentHorizontalVelocity = horizontalDirection * horizontalSpeed;
            currentVerticalVelocity = verticalDirection * verticalSpeed;
            ChangeDirection();
        }
        private void OnDisable()
        {
            CancelInvoke(nameof(IdleSound));
            player = null;
            canFollowPlayer = false;
            fabricCollision.OnPlayerEnter -= TargetFinded;
            fabricCollision.OnPlayerExit -= TargetLost;
        }
        private void Update()
        {
            if (anim.GetBool("Death"))
                return;
            TargetPlayer();
            MoveToPlayer();
            IdleMovement();

        }
        void IdleMovement()
        { 
            if (canFollowPlayer) return;

            if (Mathf.Abs(transform.position.y - startPos.y) >= maxVerticalDistance && verticalCheck)
            {
                verticalCheck = false;
                verticalDirection *= -1;
                currentVerticalVelocity = verticalDirection * verticalSpeed;
                StartCoroutine(DelayVerticalCheck());
                ChangeDirection();

            }
            if (Mathf.Abs(transform.position.x - startPos.x) >= maxHorizontalDistance && horizontalCheck)
            {
                horizontalCheck = false;
                horizontalDirection *= -1;

                currentHorizontalVelocity = 0;
                ChangeDirection();
                StartCoroutine(DelayHorizontalCheck());
            }
        }
        
        void ChangeDirection()
        { 
            rb.velocity = new Vector2(currentHorizontalVelocity, currentVerticalVelocity);
        } 
        IEnumerator DelayVerticalCheck()
        {
            yield return Helpers.GetWait(0.5f);
            verticalCheck = true;
        }
        IEnumerator DelayHorizontalCheck()
        {
            yield return Helpers.GetWait(2);
            transform.localScale = new Vector3(horizontalDirection, 1, 1);
            yield return Helpers.GetWait(2);

            currentHorizontalVelocity = horizontalDirection * horizontalSpeed;
            ChangeDirection();
            yield return Helpers.GetWait(0.5f);

            horizontalCheck = true;
        }

        void Shoot()
        {
            int dir = (transform.lossyScale.x > 0) ? 1 : -1;
            Vector2 direction = (player.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion quaternion = Quaternion.Euler(0, 0, angle-angleOfBullets);
            soundReproductor.PlayAttackSound();
            for (int i = 0; i < numberOfBullets; i++)
            {
                //GameObject bullet = Instantiate(rangedAttackData.projectile, bulletSpawn.position, quaternion);
                GameObject bullet = FabricEnemiesPool.Instance.GetOctopusBullet();
                bullet.transform.position = bulletSpawn.position;
                bullet.transform.rotation = quaternion;
                bullet.GetComponent<Avocado.Projectiles.Projectile>().FireProjectileWithAngle(rangedAttackData.projectileSpeed, rangedAttackData.projectileTravelDistance, rangedAttackData.projectileDamage);
                quaternion *= Quaternion.Euler(0, 0, angleOfBullets);
            }
        }

      
        void MoveToPlayer()
        {
            if (!canFollowPlayer ) return;

             if(randomTargetPosition==Vector2.zero)
             {
                randomTargetPosition = Random.insideUnitCircle;
                if (randomTargetPosition.x < .4f) randomTargetPosition.x =0.4f;

                if (randomTargetPosition.x < .6f) randomTargetPosition.x *= 6;
                else randomTargetPosition.x *= 3;


                if (randomTargetPosition.y < .4f) randomTargetPosition.y =0.4f;

                if (randomTargetPosition.y < .6f) randomTargetPosition.y *= 3;
                else randomTargetPosition.y *= 1.5f;


                randomTargetPosition.y= Mathf.Abs(randomTargetPosition.y);
                randomTargetPosition.x =Mathf.Abs(randomTargetPosition.x);
                if (player.position.x-transform.position.x>0)
                randomTargetPosition.x *= -1;

                randomTargetPosition += (Vector2)player.position;
             }

            Vector2 dir = ((Vector2)randomTargetPosition - (Vector2)transform.position).normalized;

            if (Vector2.Distance(transform.position, randomTargetPosition)<0.1f)
                rb.velocity = Vector2.zero;
            else
                rb.velocity = dir * followToPlayerSpeed;

            FlipSprite(player.transform.position.x- transform.position.x);
        }
        IEnumerator Shooting() 
        {
            yield return new WaitUntil(() => canFollowPlayer);
           
            while (canFollowPlayer)
            {
                Shoot();
                yield return Helpers.GetWait(shootingTime);
            }
            StartCoroutine(Shooting());
        }
        IEnumerator ResetPointAroundPlayer()
        {
            yield return new WaitUntil(() => canFollowPlayer);
           

            while (canFollowPlayer)
            {
                yield return Helpers.GetWait(1f);   
                randomTargetPosition = Vector2.zero;
            }

            StartCoroutine(ResetPointAroundPlayer());
        }

        void TargetPlayer()
        {
            if (player != null)
            {
                Vector2 dir = player.position - transform.position;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 15f, playerLayer| groundLayer);
                RaycastHit2D hitGround = Physics2D.Raycast(transform.position, dir, hit.distance,  groundLayer);
                if (hitGround && canFollowPlayer)
                {
                    startPos = transform.position;
                    horizontalCheck = true;
                    verticalCheck = true;
                    anim.SetBool("Attack", false);
                    anim.SetBool("Idle", true);
                }
                if (hit)
                    canFollowPlayer = hit.transform.CompareTag("Player");
                if (canFollowPlayer)
                {
                    anim.SetBool("Attack", true);
                    anim.SetBool("Idle", false);
                }
              
            }
        }

        void FlipSprite(float _xPos)
        {
            if (_xPos  > 0 && transform.lossyScale.x<0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            if (_xPos  < 0 && transform.lossyScale.x > 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        public void TargetFinded(Collider2D _player)
        {
            soundReproductor.PlayTargetFindSound();
            //Shoot();
            player = _player.transform;
            StopAllCoroutines();
            randomTargetPosition = Vector2.zero;
            StartCoroutine(ResetPointAroundPlayer());
            StartCoroutine(Shooting());
           
        }
        public void TargetLost(Collider2D coll)
        {
           // soundReproductor.PlayTargetLostSound();
            StopAllCoroutines();

            anim.SetBool("Attack", false);
            anim.SetBool("Idle", true);
            currentHorizontalVelocity = horizontalDirection * horizontalSpeed;
            horizontalCheck = true;
            verticalCheck = true;

            player = null;
            canFollowPlayer = false;
            startPos = transform.position;
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
        void IDamageable.Damage(DamageData data)
        {
            currentHealht -= data.Amount;
            if (currentHealht <= 0)
            {
                anim.SetBool("Death", true);
                canFollowPlayer = false;
                player = null;
                soundReproductor.PlayDeathSound();
                StopAllCoroutines();
                return;
            
            }
            soundReproductor.PlayDamageSound();

            if (damageEffect != null)
                StopCoroutine(damageEffect);
            damageEffect = DamageEffect();
            StartCoroutine(damageEffect);
        }

        void DestroyDron()
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject coin = GameManager.instance.RequestCoin();
                coin.transform.position = transform.position;
                coin.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(1f, 3f)), ForceMode2D.Impulse);
            }
            anim.SetBool("Death", false);
            anim.SetBool("Attack", false);

            gameObject.SetActive(false);
        }
      
    }
}
