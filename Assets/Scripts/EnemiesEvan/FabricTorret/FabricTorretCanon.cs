using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Avocado
{
    public class FabricTorretCanon : MonoBehaviour
    {
        FabricEnemySoundReproductor soundReproductor;

        Transform target;
        bool aiming = false;
        bool stop=false;
        bool shooting = false;
        float shootTimer = 0f;
        Quaternion idleRotation;
        SpriteRenderer spriteRenderer;
        int rootDir;
        FabricEnemyCollision fabricCollision;

        [SerializeField] float angle;
        [SerializeField] float rotationSpeed;
        [SerializeField] Sprite[] shootSprites;
        [SerializeField] D_RangedAttackState rangedAttackData;
        [SerializeField] Transform bulletSpawn;
        [SerializeField, Range(0, 5)] float shootTime;
        private void Awake()
        {
            soundReproductor = GetComponent<FabricEnemySoundReproductor>();
            fabricCollision =transform.parent.GetComponentInChildren<FabricEnemyCollision>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        }
        private void Start()
        {
            transform.parent = null;
            idleRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            rootDir=(int) transform.root.localScale.y ;
        }
        private void OnEnable()
        {
            fabricCollision.OnPlayerEnter += Aiming;
            fabricCollision.OnPlayerExit += StopAiming;
        }
        private void OnDisable()
        {
            fabricCollision.OnPlayerEnter -= Aiming;
            fabricCollision.OnPlayerExit -= StopAiming;
        }
        private void Update()
        {
            if (target != null && aiming)
            {
                if (shootTimer > shootTime)
                {
                    shooting = true;
                    shootTimer = 0f;
                    StartCoroutine(ShootSprites());
                }
                else if (!shooting)
                {
                    AimingAtPlayer();
                    shootTimer += Time.deltaTime;
                }
            }
            else if(!stop)
            {
                IdleMovement();
            }
        }
        IEnumerator IdleWait()
        {
            stop = true;
            yield return Helpers.GetWait(0.5f);
            angle *= -1;
            idleRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            stop = false;
        }
        IEnumerator ShootSprites()
        {
            for (int i = 0; i < shootSprites.Length; i++)
            {
                spriteRenderer.sprite = shootSprites[i];
                if (i==1)
                {
                    Quaternion bulletRotation = transform.rotation * Quaternion.Euler(0, 0, (-90 * rootDir));
                    
                   // GameObject bullet = Instantiate(rangedAttackData.projectile, bulletSpawn.position, bulletRotation);
                    GameObject bullet = FabricEnemiesPool.Instance.GetFabricTorretBullet();
                    bullet.transform.position = bulletSpawn.position;
                    bullet.transform.rotation = bulletRotation; 
                    bullet.GetComponent<Avocado.Projectiles.Projectile>().FireProjectileWithAngle(rangedAttackData.projectileSpeed, rangedAttackData.projectileTravelDistance, rangedAttackData.projectileDamage, rangedAttackData.initSound, rangedAttackData.hitSound);
                }
                yield return Helpers.GetWait(0.2f);
            }
          
            yield return Helpers.GetWait(0.2f);
            shooting = false;
        }
        void IdleMovement()
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, idleRotation, rotationSpeed * Time.deltaTime);
            if (Quaternion.Angle(transform.rotation, idleRotation) < 0.1f)
            {
                StartCoroutine(IdleWait());
            }
        }
        public void Aiming(Collider2D _target)
        {
            Debug.Log("Player finded");
            soundReproductor.PlayTargetFindSound();
            target = _target.transform;
            aiming = true;
        }  
        public void StopAiming(Collider2D _target)
        {
            target = null;
            aiming = false;
        }
     
        void AimingAtPlayer()
        {
            Vector2 direction = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle + (90*rootDir), Vector3.forward);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 200 * Time.deltaTime);
        }
    }
}
