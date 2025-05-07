using System.Collections.Generic;
using Avocado.Combat.Parry;
using Avocado.Utilities;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script es una clase de utilidades que te permite aplicar retroceso (knockback) a objetos 
de forma genérica, con múltiples overloads.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Combat.KnockBack
{
    // Utilidad para aplicar efectos de KnockBack de forma genérica a objetos que implementen IKnockBackable.
    public static class CombatKnockBackUtilities
    {
        // Aplica KnockBack a un GameObject si contiene un componente que implemente IKnockBackable.
        public static bool TryKnockBack(GameObject gameObject, KnockBackData data, out IKnockBackable knockBackable)
        {
            if (gameObject.TryGetComponentInChildren(out knockBackable))
            {
                knockBackable.KnockBack(data);
                return true;
            }

            return false;
        }

        // Versión sobrecargada que acepta un Component.
        public static bool TryKnockBack(Component component, KnockBackData data, out IKnockBackable knockBackable)
        {
            return TryKnockBack(component.gameObject, data, out knockBackable);
        }

        // Aplica KnockBack a múltiples GameObjects.
        public static bool TryKnockBack(IEnumerable<GameObject> gameObjects, KnockBackData data,
            out List<IKnockBackable> knockBackables)
        {
            var hasKnockedBack = false;
            knockBackables = new List<IKnockBackable>();

            foreach (var gameObject in gameObjects)
            {
                if (TryKnockBack(gameObject, data, out var knockBackable))
                {
                    knockBackables.Add(knockBackable);
                    hasKnockedBack = true;
                }
            }

            return hasKnockedBack;
        }

        // Aplica KnockBack a múltiples componentes.
        public static bool TryKnockBack(IEnumerable<Component> components, KnockBackData data,
            out List<IKnockBackable> knockBackables)
        {
            var hasKnockedBack = false;
            knockBackables = new List<IKnockBackable>();

            foreach (var comp in components)
            {
                if (TryKnockBack(comp, data, out var knockBackable))
                {
                    knockBackables.Add(knockBackable);
                    hasKnockedBack = true;
                }
            }

            return hasKnockedBack;
        }
    }
}
