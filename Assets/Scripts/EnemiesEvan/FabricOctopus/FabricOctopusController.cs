using Avocado.Combat.Damage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class FabricOctopusController : MonoBehaviour, IDamageable
    {
        Animator animator;
        [SerializeField] int maxShoots;
        int currentShoots;
        [SerializeField] Transform[] movePoints;
        [SerializeField] Transform bulletSpawn;
        List<Vector2> movePositions;
        int currentIndexPoint;
        [SerializeField] float moveSpeed;
        [SerializeField] float idleTime;
        bool isIdle;
        bool shooting;
        [SerializeField] D_RangedAttackState rangedAttackData;

        [SerializeField] float health;
        SpriteRenderer sprite;
        IEnumerator damageEffect;
        private void Awake()
        {
            sprite = GetComponentInChildren<SpriteRenderer>();

            isIdle = false;
            currentShoots = 0;
            animator = GetComponent<Animator>();
           
            movePositions=new List<Vector2>();
            for (int i=0; i<movePoints.Length; i++)
            {
                movePositions.Add(movePoints[i].position);
                movePoints[i].gameObject.SetActive(false);
            }
            currentIndexPoint = 0;

            transform.position = movePositions[currentIndexPoint];
            StartCoroutine(NextPoint());
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

            if (Vector2.Distance(transform.position, movePositions[currentIndexPoint]) > 0.01f)
                transform.position = Vector2.MoveTowards(transform.position, movePositions[currentIndexPoint], moveSpeed * Time.deltaTime);
            else
                StartCoroutine(NextPoint());

        }
        IEnumerator NextPoint()
        {
            isIdle = true;
            animator.SetBool("Idle", true);
            animator.SetBool("Move", false);

            yield return Helpers.GetWait(idleTime/2);

            currentIndexPoint = (currentIndexPoint < movePositions.Count-1) ? currentIndexPoint + 1: 0 ;
            FlipSprite(movePositions[currentIndexPoint].x);
            yield return Helpers.GetWait(idleTime/2);

            isIdle = false;
            animator.SetBool("Idle", false);
            animator.SetBool("Move", true);
        }
        public void SpawnProjectile()
        { 
            GameObject bullet = Instantiate(rangedAttackData.projectile, bulletSpawn.position, Quaternion.identity);
            bullet.transform.localScale = transform.localScale;
            int direction = (transform.lossyScale.x > 0) ? 1 : -1;
            bullet.GetComponent<Avocado.Projectiles.Projectile>().FireProjectile(rangedAttackData.projectileSpeed*direction, rangedAttackData.projectileTravelDistance, rangedAttackData.projectileDamage);

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
        public void TargetFinded()
        {
            shooting = true;
            animator.SetBool("Idle", false);
            animator.SetBool("Move", false);
            animator.SetBool("Attack", true);
            Shooting();
        }

        public void TargetLost()
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
            Debug.Log(data.Amount);
            health -= data.Amount;
            if (health <= 0)
            {
                animator.SetBool("Death", true);
                return;
            }
            if (damageEffect != null)
                StopCoroutine(damageEffect);
            damageEffect = DamageEffect();
            StartCoroutine(damageEffect);
        }
        void DestroyDron()
        {
            Destroy(gameObject);

        }
    }
}
