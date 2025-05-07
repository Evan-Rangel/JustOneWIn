using Avocado.Combat.PoiseDamage;
using Avocado.Interfaces;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este componente aplica daño a la postura (poise) cuando un ataque golpea a un enemigo. Utiliza 
el sistema de colisiones ActionHitBox, que detecta qué objetos han sido impactados durante una 
animación de ataque.
Si un Collider2D impactado implementa la interfaz IPoiseDamageable, entonces recibe daño a su 
postura.
La cantidad de daño se define en currentAttackData.Amount.
Esto se puede usar para sistemas en los que los enemigos quedan aturdidos o desestabilizados 
al recibir suficiente daño a la postura, incluso si aún tienen vida.
Este componente aplica daño a la postura ("poise") de los enemigos cuando un ataque golpea.
Se activa cuando el ActionHitBox detecta colisiones.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    public class PoiseDamage : WeaponComponent<PoiseDamageData, AttackPoiseDamage>
    {
        private ActionHitBox hitBox;

        // Intenta aplicar daño a la postura de cada enemigo impactado.
        private void HandleDetectCollider2D(Collider2D[] colliders)
        {
            foreach (var item in colliders)
            {
                if (item.TryGetComponent(out IPoiseDamageable poiseDamageable))
                {
                    // Aplica daño a la postura usando los datos actuales del ataque
                    poiseDamageable.DamagePoise(new Combat.PoiseDamage.PoiseDamageData(currentAttackData.Amount, Core.Root));
                }
            }
        }

        // Al iniciar, se conecta al evento del hitbox para recibir notificaciones de colisiones.
        protected override void Start()
        {
            base.Start();

            hitBox = GetComponent<ActionHitBox>();
            hitBox.OnDetectedCollider2D += HandleDetectCollider2D;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            hitBox.OnDetectedCollider2D -= HandleDetectCollider2D;
        }
    }
}
