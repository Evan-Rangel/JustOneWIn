using Avocado.Combat.Damage;
using UnityEngine;
using static Avocado.Utilities.CombatDamageUtilities; // Importación estática para llamar a funciones de utilidad directamente

/*---------------------------------------------------------------------------------------------
DamageOnHitBoxAction es un componente que detecta colisiones a través de un ActionHitBox, y 
cuando se detectan enemigos dentro del área de ataque, les aplica daño automáticamente. 
Utiliza una función de utilidad (TryDamage) para evitar escribir el mismo bucle cada vez.
Esto permite que cualquier enemigo u objeto que implemente la interfaz IDamageable reciba 
daño correctamente durante un ataque.
Este componente de arma se encarga de aplicar daño cuando el HitBox detecta una colisión.
Usa una utilidad estática (TryDamage) para aplicar daño a todos los objetos detectados que 
implementen la interfaz IDamageable.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    public class DamageOnHitBoxAction : WeaponComponent<DamageOnHitBoxActionData, AttackDamage>
    {
        private ActionHitBox hitBox; // Referencia al hitbox que detecta colisiones con enemigos

        // Esta función se ejecuta cada vez que el HitBox detecta un conjunto de colliders.
        // Aplica daño a cada collider que implemente la interfaz IDamageable.
        private void HandleDetectCollider2D(Collider2D[] colliders)
        {
            TryDamage(colliders, new DamageData(currentAttackData.Amount, Core.Root), out _);
        }

        // Se ejecuta al iniciar. Obtiene el HitBox y se suscribe al evento de detección de colisiones.
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
