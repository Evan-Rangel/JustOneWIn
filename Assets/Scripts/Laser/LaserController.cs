using Avocado.Combat.Damage;
using Avocado.Combat.KnockBack;
using System;
using UnityEngine;

namespace Avocado
{
    public class LaserController : MonoBehaviour
    {
        [SerializeField] D_MeleeAttack damageData;
        Animator anim;
        [SerializeField] float activationDelay=3;
        event Action<string, Transform> OnPlaySound;

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }
        private void OnEnable()
        {
            OnPlaySound += AudioManager.instance.PlaySFXSound;
        }
        private void OnDisable()
        {
            OnPlaySound -= AudioManager.instance.PlaySFXSound;
            CancelInvoke("ActiveLaser");
        }
        private void Start()
        {
            InvokeRepeating("ActiveLaser", activationDelay, activationDelay);
        }
        void ActiveLaser()
        { 
            anim.SetTrigger("Active");
        }
        void PlayLaserCastSound()
        {
            OnPlaySound?.Invoke("Lasercast", transform);
        }
        void PlayLaserPreCastSound()
        {
            OnPlaySound?.Invoke("LaserPrecast", transform);
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
