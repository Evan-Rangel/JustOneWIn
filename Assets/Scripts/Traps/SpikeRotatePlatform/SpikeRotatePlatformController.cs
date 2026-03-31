using Avocado.Combat.Damage;
using Avocado.Combat.KnockBack;
using System;
using UnityEngine;

namespace Avocado
{
    public class SpikeRotatePlatformController : MonoBehaviour
    {
        Animator anim;
        [SerializeField] D_MeleeAttack attackData;
        [SerializeField] float timeToActive, ActiveTime;
        FabricEnemyCollision fabricCollision;
        bool activePlatform;
        [SerializeField] string collisionSound, activeSound;
        event Action<string, Transform> OnPlaySound;

        private void Awake()
        {
            anim = GetComponentInChildren<Animator>();
            fabricCollision = GetComponentInChildren<FabricEnemyCollision>();
        }
      /*  private void Update()
        {
            Collider2D[] detectedObjects = Physics2D.OverlapBoxAll(currentActivePoint.position, new Vector2(1, 0.2f)* attackData.attackRadius,0, attackData.whatIsPlayer);

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
                    if (currentActivePoint==damagePointUp)
                        knockBackable.KnockBack(new KnockBackData(attackData.knockbackAngle, attackData.knockbackStrength,direction, gameObject));
                    else
                        knockBackable.KnockBack(new KnockBackData(new Vector2(attackData.knockbackAngle.x, 0), attackData.knockbackStrength, direction, gameObject));
                }
            }
        }*/
        private void OnEnable()
        {
            OnPlaySound += AudioManager.instance.PlaySFXSound;
            activePlatform = false;
            anim.SetBool("Active", false);
            fabricCollision.OnPlayerEnter += OnPlayerEnter;
            fabricCollision.OnPlayerExit += OnPlayerExit;
        }
        void OnDisable() 
        {
            OnPlaySound -= AudioManager.instance.PlaySFXSound;
            fabricCollision.OnPlayerEnter -= OnPlayerEnter;
            fabricCollision.OnPlayerExit -= OnPlayerExit;
            
        }
        
        void OnPlayerEnter(Collider2D collider)
        {
            if (activePlatform) return;
            activePlatform = true;
            Invoke(nameof(ActivatePlatform), timeToActive);
        }
        void OnPlayerExit(Collider2D collision)
        {
        }
        void RotationSound()
        {
            OnPlaySound?.Invoke(activeSound, transform);
        }
        void CollisionSound()
        { 
            OnPlaySound?.Invoke(collisionSound, transform);
        }
        void ActivatePlatform()
        {
            anim.SetBool("Active", true);
            Invoke(nameof(InactivePlatform), ActiveTime);
        }
        void InactivePlatform()
        {
            activePlatform = false;
            anim.SetBool("Active", false);
        }
       
    }
}
