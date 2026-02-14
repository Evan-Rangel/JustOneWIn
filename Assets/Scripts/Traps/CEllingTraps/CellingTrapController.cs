using Avocado.Combat.Damage;
using Avocado.Combat.KnockBack;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace Avocado
{
    public class CellingTrapController : MonoBehaviour
    {
        [SerializeField] D_MeleeAttack damageData;
        GameObject colliderBlock;
        [SerializeField] float activationDelay;
        //float
        Collider2D trapCollider;
        Animator anim;
        FabricEnemyCollision enemyCollision;
        private void Awake()
        {
            trapCollider = GetComponent<Collider2D>();
            anim =GetComponent<Animator>();
            enemyCollision = GetComponentInChildren<FabricEnemyCollision>();
            colliderBlock = transform.GetChild(0).gameObject;
        }
        void ActiveCollider()
        {
            colliderBlock.SetActive(true);
            trapCollider.enabled = true;
        }
        void DisableCollider()
        {
            colliderBlock.SetActive(false);
            trapCollider.enabled = false;
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
