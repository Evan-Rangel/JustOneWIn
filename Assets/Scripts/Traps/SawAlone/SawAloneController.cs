using Avocado.Combat.Damage;
using Avocado.Combat.KnockBack;
using UnityEngine;

namespace Avocado
{
    public class SawAloneController : MonoBehaviour
    {
        [SerializeField] D_MeleeAttack damageData;
        [SerializeField] Transform damagePoint;
        private void Update()
        {
            Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(damagePoint.position, damageData.attackRadius, damageData.whatIsPlayer);

            foreach (Collider2D collider in detectedObjects)
            {
                IDamageable damageable = collider.GetComponent<IDamageable>();

                if (damageable != null)
                {
                    damageable.Damage(new DamageData(damageData.attackDamage, gameObject));
                }

                IKnockBackable knockBackable = collider.GetComponent<IKnockBackable>();
                if (knockBackable != null)
                {
                    int direction = (transform.position.x - collider.transform.position.x > 0) ? -1 : 1;
                    if (transform.position.y - collider.transform.position.y < 0)
                    knockBackable.KnockBack(new KnockBackData(damageData.knockbackAngle, damageData.knockbackStrength, direction, gameObject));
                    else knockBackable.KnockBack(new KnockBackData(new Vector2(damageData.knockbackAngle.x, 0), damageData.knockbackStrength, direction, gameObject));
                }
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(damagePoint.position, damageData.attackRadius);
        }
    }
}
