using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script Definir contratos para objetos del juego que puedan ser interactuados de alguna 
forma. Detalles:
-IInteractable define las operaciones básicas que cualquier interactuable debe implementar: 
activar, desactivar, obtener su posición y realizar una acción de interacción.
-IInteractable<T> agrega funcionalidades genéricas para manejar un contexto adicional.
Por ejemplo: un WeaponPickup podría implementar IInteractable<WeaponDataSO> para almacenar 
información de un arma.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Interaction
{
    public interface IInteractable
    {
        // Activa la interacción (por ejemplo, mostrar un ícono, activar colisión, etc.)
        void EnableInteraction();

        // Desactiva la interacción
        void DisableInteraction();

        // Retorna la posición del objeto interactuable
        Vector3 GetPosition();

        // Lógica que se ejecuta al interactuar (por ejemplo, recoger un objeto, abrir una puerta, etc.)
        void Interact();
    }

    public interface IInteractable<T> : IInteractable
    {
        // Devuelve el contexto (por ejemplo, el arma contenida en un WeaponPickup)
        T GetContext();

        // Establece el contexto
        void SetContext(T context);
    }
}
