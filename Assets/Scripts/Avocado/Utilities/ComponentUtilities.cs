using Avocado.Interaction;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script te permite verificar si ese componente implementa la interfaz.
Pero como IsInteractable está escrito como una extensión de Component, debes llamar el método 
desde un Component, no directamente desde GameObject.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Utilities
{
    public static class ComponentUtilities
    {
        // Método de extensión para cualquier Component que verifica si implementa IInteractable
        public static bool IsInteractable(this Component component, out IInteractable interactable)
        {
            // Usa TryGetComponent para revisar si el componente tiene la interfaz
            return component.TryGetComponent(out interactable);
        }
    }
}
