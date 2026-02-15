using Avocado.Combat.Damage;
using Avocado.Combat.KnockBack;
using UnityEngine;

namespace Avocado
{
    public class SpikesController : MonoBehaviour
    {
        [SerializeField] D_MeleeAttack damageData;

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
