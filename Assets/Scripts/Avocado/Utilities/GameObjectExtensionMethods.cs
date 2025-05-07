using UnityEngine;

/*---------------------------------------------------------------------------------------------
Proporciona una versión “segura” de GetComponentInChildren<T>() usando TryGet — devolviendo 
true si encontró el componente, o false si no lo hizo. 
Esto evita tener que comprobar si es null cada vez manualmente.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Utilities
{
    public static class GameObjectExtensionMethods
    {
        // Intenta obtener un componente de tipo T en los hijos del GameObject.
        public static bool TryGetComponentInChildren<T>(this GameObject gameObject, out T component)
        {
            component = gameObject.GetComponentInChildren<T>();
            return component != null;
        }

        // Variante para usar directamente sobre cualquier Component (por ejemplo un MonoBehaviour)
        public static bool TryGetComponentInChildren<T>(this Component comp, out T component)
        {
            return TryGetComponentInChildren(comp.gameObject, out component);
        }
    }
}
