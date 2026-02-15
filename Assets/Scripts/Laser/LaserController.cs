using Avocado.Combat.Damage;
using Avocado.Combat.KnockBack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class LaserController : MonoBehaviour
    {
        [SerializeField] D_MeleeAttack damageData;
        Animator anim;
        [SerializeField] float activationDelay=3;

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }
        private void Start()
        {
            InvokeRepeating("ActiveLaser", activationDelay, activationDelay);
        }
        void ActiveLaser()
        { 
            anim.SetTrigger("Active");
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
    }
}
