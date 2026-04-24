using Avocado.Combat.Damage;
using System;
using Avocado.Combat.KnockBack;
using UnityEngine;

namespace Avocado
{
    public class LaserBlockController : MonoBehaviour
    {
        [SerializeField] D_MeleeAttack damageData;

        [SerializeField] string eventName;
        Animator animator;
        FabricEnemyCollision activateCollision;
        event Action<string, Transform> OnPlaySound;
        [SerializeField]bool startActivated;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            try 
            { 
                activateCollision= GetComponentInChildren<FabricEnemyCollision>();  
            }
            catch{ }
        }
        private void Start()
        {
            if (startActivated)
                ActivateBlock();
            if (eventName == "" || eventName == string.Empty || eventName == null)
                return;
            if (SaveManager.IsEventKeySaved(eventName))
                gameObject.SetActive(false);
        }
        private void OnEnable()
        {
            OnPlaySound += AudioManager.instance.PlaySFXSound;

            if (activateCollision!=null)
                activateCollision.OnPlayerEnter += PlayerCollisionEnter;
        }
        private void OnDisable()
        {
            OnPlaySound -= AudioManager.instance.PlaySFXSound;

            if (activateCollision != null)
                activateCollision.OnPlayerEnter -= PlayerCollisionEnter;
        }
        void PlayerCollisionEnter(Collider2D _player)
        { 
            ActivateBlock();
        }

        public void ActivateBlock()
        { 
            animator.SetTrigger("Activate");
        }

        public void DeactivateBlock()
        {
            animator.SetTrigger("Deactivate");
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();
            if (damageable != null)
            {
                OnPlaySound?.Invoke("LaserHit", transform);
                damageable.Damage(new DamageData(damageData.attackDamage, gameObject));
            }
            IKnockBackable knockBackable = collision.GetComponent<IKnockBackable>();
            if (knockBackable != null)
            {
                int direction = (transform.position.x - collision.transform.position.x > 0) ? -1 : 1;
                knockBackable.KnockBack(new KnockBackData(damageData.knockbackAngle, damageData.knockbackStrength, direction, gameObject));
            }
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();
            if (damageable != null)
            {
                OnPlaySound?.Invoke("LaserHit", transform);
                damageable.Damage(new DamageData(damageData.attackDamage, gameObject));
            }
            IKnockBackable knockBackable = collision.GetComponent<IKnockBackable>();
            if (knockBackable != null)
            {
                int direction = (transform.position.x - collision.transform.position.x > 0) ? -1 : 1;
                knockBackable.KnockBack(new KnockBackData(damageData.knockbackAngle, damageData.knockbackStrength, direction, gameObject));
            }
        }
    }
}
