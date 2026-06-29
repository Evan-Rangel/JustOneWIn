using Avocado.Combat.Damage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class FabricOctopusController : MonoBehaviour, IDamageable
    {


        FabricEnemySoundReproductor soundReproductor;
        [SerializeField] float minRandomTimeLapseIdleSound, maxRandomTimeLapseIdleSound;
        void IdleSound()
        {
            soundReproductor.PlayIdleSound();
            Invoke(nameof(IdleSound), Random.Range(minRandomTimeLapseIdleSound, maxRandomTimeLapseIdleSound));
        }
        public void WalkSound()
        {
            soundReproductor.PlayWalkSound();
        }
        FabricEnemyCollision fabricCollision;
        Animator animator;
        [SerializeField] int maxShoots;
        int currentShoots;
        [SerializeField] Transform bulletSpawn;
        [SerializeField]List<Vector2> movePositions;
        public void SetMovePositions(List<Vector2> _newMovePositions) 
        
        {
            movePositions = _newMovePositions;
            targetPos = new List<Vector2>();
            for (int i = 0; i < movePositions.Count; i++)
            {
                targetPos.Add(movePositions[i] + (Vector2)transform.position);
            }
        }

        List<Vector2> targetPos;
        int currentIndexPoint;
        [SerializeField] float moveSpeed;
        [SerializeField] float idleTime;
        bool isIdle;
        bool shooting;
        [SerializeField] D_RangedAttackState rangedAttackData;

         float currentHealth;
        [SerializeField] float maxtHealth;
        SpriteRenderer sprite;
        IEnumerator damageEffect;
        private void Awake()
        {
            soundReproductor = GetComponent<FabricEnemySoundReproductor>();
            sprite = GetComponentInChildren<SpriteRenderer>();
            fabricCollision= GetComponentInChildren<FabricEnemyCollision>();
            animator = GetComponent<Animator>();
            targetPos = new List<Vector2>();
            for (int i = 0; i < movePositions.Count; i++)
            {
                targetPos.Add(movePositions[i] + (Vector2)transform.position);
            }

        }

        private void OnEnable()
        {
            isIdle = false;
            currentShoots = 0;
            currentHealth = maxtHealth;
            
            currentIndexPoint = 0;

            transform.position = targetPos[currentIndexPoint];
            StartCoroutine(NextPoint());

            fabricCollision.OnPlayerEnter += TargetFinded;
            fabricCollision.OnPlayerExit += TargetLost;
        }
        private void OnDisable()
        {
            CancelInvoke(nameof(IdleSound));
            fabricCollision.OnPlayerEnter -= TargetFinded;
            fabricCollision.OnPlayerExit -= TargetLost;
        }
        private void Update()
        {
            if (animator.GetBool("Death"))
                return;
            IdleMovement();

        }
        void IdleMovement()
        {
            if (isIdle || shooting) return;

            if (Vector2.Distance(transform.position, targetPos[currentIndexPoint]) > 0.01f)
                transform.position = Vector2.MoveTowards(transform.position, targetPos[currentIndexPoint], moveSpeed * Time.deltaTime);
            else
                StartCoroutine(NextPoint());

        }
        IEnumerator NextPoint()
        {
            isIdle = true;
            animator.SetBool("Idle", true);
            animator.SetBool("Move", false);

            yield return Helpers.GetWait(idleTime/2);

            currentIndexPoint = (currentIndexPoint < targetPos.Count-1) ? currentIndexPoint + 1: 0 ;
            FlipSprite(targetPos[currentIndexPoint].x);
            yield return Helpers.GetWait(idleTime/2);

            isIdle = false;
            animator.SetBool("Idle", false);
            animator.SetBool("Move", true);
        }
        public void SpawnProjectile()
        { 
            GameObject bullet= FabricEnemiesPool.Instance.GetOctopusBullet();
            bullet.transform.position = bulletSpawn.position;
            bullet.transform.localScale = transform.localScale;
            bullet.transform.rotation=Quaternion.identity;
            int direction = (transform.lossyScale.x > 0) ? 1 : -1;
            bullet.GetComponent<Avocado.Projectiles.Projectile>().FireProjectileWithDirection(rangedAttackData.projectileSpeed*direction, rangedAttackData.projectileTravelDistance, rangedAttackData.projectileDamage, rangedAttackData.initSound, rangedAttackData.hitSound);

        }
        void FlipSprite(float _xPos)
        {
            if (_xPos - transform.position.x > 0 && transform.lossyScale.x < 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            if (_xPos - transform.position.x < 0 && transform.lossyScale.x > 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        public void Shooting()
        {
            if (currentShoots < maxShoots)
            {
                currentShoots++;
            }
            else
            {
                currentShoots = 0;
                StartCoroutine(ShootingWait());
            }
        }
        IEnumerator ShootingWait()
        {
            animator.SetBool("Idle", true);
            animator.SetBool("Attack", false);

            yield return Helpers.GetWait(3f);
            if (shooting)
            {
                animator.SetBool("Idle", false);
                animator.SetBool("Attack", true);

                Shooting();
            }
        }
        public void TargetFinded(Collider2D coll)
        {
            soundReproductor.PlayTargetFindSound();

            shooting = true;
            animator.SetBool("Idle", false);
            animator.SetBool("Move", false);
            animator.SetBool("Attack", true);
            Shooting();
        }

        public void TargetLost(Collider2D coll)
        {
            shooting = false;
            animator.SetBool("Idle", false);
            animator.SetBool("Move", true);
            animator.SetBool("Attack", false);

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
            currentHealth -= data.Amount;
            if (currentHealth <= 0)
            {
                soundReproductor.PlayDeathSound();

                animator.SetBool("Death", true);
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
            gameObject.SetActive(false);
        }
        private void OnDrawGizmos()

        {
            Gizmos.color = Color.red;

            if (targetPos!=null &&targetPos.Count >0)
            {
                for (int i = 0; i < targetPos.Count; i++)
                {
                    Gizmos.DrawSphere(targetPos[i], 0.1f);
                    Gizmos.DrawLine(transform.position, targetPos[i]);
                }
            }
            else
            {

                for (int i = 0; i < movePositions.Count; i++)
                {
                    Vector2 tPos = (Vector2)transform.position + movePositions[i];

                    Gizmos.DrawSphere(tPos, 0.1f);
                    Gizmos.DrawLine(transform.position, tPos);
                }
            }
        
        }
    }
}
