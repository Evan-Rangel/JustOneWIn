using System;
using Avocado.Interaction;
using Avocado.Interaction.Interactables;
using Avocado.Weapons;

/*---------------------------------------------------------------------------------------------
Este script Gestionar el sistema de recolección e intercambio de armas cuando el personaje 
interactúa con un pickup de arma (WeaponPickup).
Funciones principales:
-Detectar cuando se intenta recoger un arma (HandleTryInteract).
-Si hay un espacio libre, agregar directamente el arma al inventario.
-Si no hay espacio, solicitar al jugador que elija qué arma quiere intercambiar (OnChoiceRequested).
-Reemplazar el arma elegida y disparar un evento de que un arma fue descartada (OnWeaponDiscarded).
-Manejar correctamente las suscripciones a eventos (OnEnable, OnDisable).
Notas importantes:
-Utiliza el WeaponInventory para manipular las armas disponibles.
-Utiliza el InteractableDetector para saber cuándo el jugador interactúa.
-OnChoiceRequested permite mostrar una UI o algún menú de selección de armas.
-WeaponSwapChoiceRequest y WeaponSwapChoice son clases/estructuras auxiliares que ayudan en 
el flujo de decisiones.
---------------------------------------------------------------------------------------------*/

namespace Avocado.CoreSystem
{
    public class WeaponSwap : CoreComponent
    {
        // Evento que solicita una elección de arma al jugador
        public event Action<WeaponSwapChoiceRequest> OnChoiceRequested;

        // Evento que notifica que un arma fue descartada
        public event Action<WeaponDataSO> OnWeaponDiscarded;

        private InteractableDetector interactableDetector;
        private WeaponInventory weaponInventory;

        private WeaponDataSO newWeaponData;

        private WeaponPickup weaponPickup;

        // Maneja el intento de interactuar con un objeto interactuable
        private void HandleTryInteract(IInteractable interactable)
        {
            // Si no es un pickup de arma, salir
            if (interactable is not WeaponPickup pickup)
                return;

            weaponPickup = pickup;
            newWeaponData = weaponPickup.GetContext(); // Obtener el arma del pickup

            // Si hay un espacio vacío en el inventario, ponerla directamente
            if (weaponInventory.TryGetEmptyIndex(out var index))
            {
                weaponInventory.TrySetWeapon(newWeaponData, index, out _);
                var pickupNetId = weaponPickup.netId;
                transform.root.GetComponent<PlayerObjectController>().CmdPickupWeapon(pickupNetId);
                //interactable.Interact();
                newWeaponData = null;
                return;
            }

            // Si no hay espacio, solicitar al jugador elegir qué arma intercambiar
            OnChoiceRequested?.Invoke(new WeaponSwapChoiceRequest(
                HandleWeaponSwapChoice,                 // Callback que maneja la decisión
                weaponInventory.GetWeaponSwapChoices(), // Opciones disponibles
                newWeaponData                           // Nueva arma que se quiere recoger
            ));
        }

        // Maneja la elección del jugador sobre qué arma intercambiar
        private void HandleWeaponSwapChoice(WeaponSwapChoice choice)
        {
            // Si no puede hacer swap, salir
            if (!weaponInventory.TrySetWeapon(newWeaponData, choice.Index, out var oldData))
                return;

            newWeaponData = null;

            // Disparar evento de que un arma fue descartada
            OnWeaponDiscarded?.Invoke(oldData);

            if (weaponPickup is null)
                return;

            // Interactuar con el pickup para recoger el arma
            var pickupNetId = weaponPickup.netId;
            transform.root.GetComponent<PlayerObjectController>().CmdPickupWeapon(pickupNetId);
           // weaponPickup.Interact();
        }

        // Inicializar referencias
        protected override void Awake()
        {
            base.Awake();

            interactableDetector = core.GetCoreComponent<InteractableDetector>();
            weaponInventory = core.GetCoreComponent<WeaponInventory>();
        }

        // Subscribirse al evento de intentar interactuar
        private void OnEnable()
        {
            interactableDetector.OnTryInteract += HandleTryInteract;
        }

        // Desubscribirse al evento
        private void OnDisable()
        {
            interactableDetector.OnTryInteract -= HandleTryInteract;
        }
    }
}
