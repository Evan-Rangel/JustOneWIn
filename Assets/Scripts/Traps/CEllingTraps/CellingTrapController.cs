using Avocado.Combat.Damage;
using Avocado.Combat.KnockBack;
using UnityEngine;

namespace Avocado
{
    public class CellingTrapController : MonoBehaviour
    {
        [SerializeField] D_MeleeAttack damageData;
        [SerializeField] float activationDelay;
        Animator anim;
        private void Awake()
        {
            anim =GetComponent<Animator>();
        }
      
        private void OnEnable()
        {
            InvokeRepeating("ActivateTrap", activationDelay, activationDelay);
        }
       
        private void OnDisable()
        {
            CancelInvoke("ActivateTrap");
        }
        void ActivateTrap()
        {
            anim.SetTrigger("Activate");
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.Damage(new DamageData(damageData.attackDamage, gameObject));
            }

            IKnockBackable knockBackable = collision.GetComponent<IKnockBackable>();
            if (knockBackable != null)
            {
                int direction = (transform.position.x - collision.transform.position.x > 0) ? -1 : 1;
                knockBackable.KnockBack(new KnockBackData(damageData.knockbackAngle, damageData.knockbackStrength, direction, gameObject));
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();
            if (damageable!=null)
            {
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
