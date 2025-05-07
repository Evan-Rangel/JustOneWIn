using System;
using Avocado.CoreSystem;
using Avocado.Weapons;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
WeaponSwapUI es el controlador que gestiona toda la interfaz gráfica para intercambiar armas. 
Cuando el sistema (WeaponSwap) lo solicita, este script muestra la nueva arma y permite al 
jugador comparar y elegir entre mantener la suya actual o cambiarla. Usa CanvasGroup para 
activar o desactivar la UI y se asegura de que el juego entre en modo UI mientras se toma la 
decisión, evitando que el jugador actúe hasta elegir.
---------------------------------------------------------------------------------------------*/

namespace Avocado.UI
{
    // Controlador de la UI que permite al jugador elegir si intercambia su arma actual por una nueva
    public class WeaponSwapUI : MonoBehaviour
    {
        // Referencia al sistema que gestiona los intercambios de armas
        [SerializeField] private WeaponSwap weaponSwap;

        // Referencia a la UI que muestra la información del arma nueva
        [SerializeField] private WeaponInfoUI newWeaponInfo;

        // Arreglo de opciones visuales (por ejemplo, izquierda y derecha) para intercambiar armas
        [SerializeField] private WeaponSwapChoiceUI[] weaponSwapChoiceUIs;

        // Referencia al GameManager, utilizado para cambiar el estado del juego
        [SerializeField] private GameManager gameManager;

        // Componente CanvasGroup para mostrar u ocultar la UI
        private CanvasGroup canvasGroup;

        // Delegado para almacenar el callback del intercambio
        private Action<WeaponSwapChoice> choiceSelectedCallback;

        // Método que se llama cuando el sistema de intercambio solicita una elección
        private void HandleChoiceRequested(WeaponSwapChoiceRequest choiceRequest)
        {
            // Cambia el estado del juego para pausar la acción y mostrar la UI
            gameManager.ChangeState(GameManager.GameState.UI);

            // Guarda el callback que se llamará cuando se haga una elección
            choiceSelectedCallback = choiceRequest.Callback;

            // Muestra la información de la nueva arma en la UI
            newWeaponInfo.PopulateUI(choiceRequest.NewWeaponData);

            // Informa a cada UI de opción para que muestre su elección correspondiente
            foreach (var weaponSwapChoiceUi in weaponSwapChoiceUIs)
            {
                weaponSwapChoiceUi.TakeRelevantChoice(choiceRequest.Choices);
            }

            // Activa la UI visualmente
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
        }

        // Método que se llama cuando el jugador selecciona una opción
        private void HandleChoiceSelected(WeaponSwapChoice choice)
        {
            // Regresa el juego a su estado normal
            gameManager.ChangeState(GameManager.GameState.Gameplay);

            // Ejecuta el callback con la elección seleccionada
            choiceSelectedCallback?.Invoke(choice);

            // Oculta la UI
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
        }

        // Se llama al iniciar el script
        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0f; // Inicia oculta
        }

        // Se llama cuando el objeto se activa
        private void OnEnable()
        {
            // Se suscribe al evento de solicitud de elección de arma
            weaponSwap.OnChoiceRequested += HandleChoiceRequested;

            // Se suscribe al evento de selección para cada opción visual
            foreach (var weaponSwapChoiceUI in weaponSwapChoiceUIs)
            {
                weaponSwapChoiceUI.OnChoiceSelected += HandleChoiceSelected;
            }
        }

        // Se llama cuando el objeto se desactiva
        private void OnDisable()
        {
            // Se desuscribe del evento de solicitud
            weaponSwap.OnChoiceRequested -= HandleChoiceRequested;

            // Se desuscribe de cada opción visual
            foreach (var weaponSwapChoiceUI in weaponSwapChoiceUIs)
            {
                weaponSwapChoiceUI.OnChoiceSelected -= HandleChoiceSelected;
            }
        }
    }
}
