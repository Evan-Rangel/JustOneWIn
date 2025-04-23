using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.Utilities
{
    public static class GameObjectExtensionMethods
    {
        public static bool TryGetComponentInChildren<T>(this GameObject gameObject, out T component)
        {
            component = gameObject.GetComponentInChildren<T>();
            return component != null;
        }

        public static bool TryGetComponentInChildren<T>(this Component comp, out T component)
        {
            return TryGetComponentInChildren(comp.gameObject, out component);
        }
    }
}
