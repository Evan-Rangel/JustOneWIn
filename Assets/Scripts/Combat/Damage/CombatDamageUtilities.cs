using System.Collections.Generic;
using Avocado.Combat.Damage;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script hace que la clase CombatDamageUtilities permite:
-Aplicar daño a un solo objeto (GameObject) si tiene un componente que implemente IDamageable.
-Aplicar daño a varios objetos (Collider2D[]), guardando los que fueron dañados exitosamente.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Utilities
{
    public static class CombatDamageUtilities
    {
        // Intenta aplicar daño a un solo GameObject.
        public static bool TryDamage(GameObject gameObject, DamageData damageData, out IDamageable damageable)
        {
            // Usa el método de extensión TryGetComponentInChildren para buscar un IDamageable
            if (gameObject.TryGetComponentInChildren(out damageable))
            {
                damageable.Damage(damageData);
                return true;
            }

            return false;
        }

        // Intenta aplicar daño a varios colliders.
        public static bool TryDamage(Collider2D[] colliders, DamageData damageData, out List<IDamageable> damageables)
        {
            var hasDamaged = false;
            damageables = new List<IDamageable>();

            foreach (var collider in colliders)
            {
                // Intenta dañar cada GameObject asociado a los colliders
                if (TryDamage(collider.gameObject, damageData, out IDamageable damageable))
                {
                    damageables.Add(damageable);
                    hasDamaged = true;
                }
            }

            return hasDamaged;
        }
    }
}
