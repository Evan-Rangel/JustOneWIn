using System;
using Avocado.Weapons;
using UnityEngine;
using UnityEngine.UI;

/*---------------------------------------------------------------------------------------------
WeaponSwapChoiceUI es un componente para mostrar una de las posibles elecciones de armas cuando 
el jugador está eligiendo con cuál quedarse (por ejemplo, después de recoger una nueva). Se le 
da un conjunto de opciones (WeaponSwapChoice[]) y este script selecciona la que corresponde al 
input asignado (como "Izquierda" o "Derecha"), muestra su información y permite al jugador 
seleccionarla a través de un botón. Al hacer clic, lanza un evento con la elección para que 
otro sistema (como el inventario) la procese.
---------------------------------------------------------------------------------------------*/

namespace Avocado.UI
{
    // Este componente representa una opción de intercambio de arma en la UI
    public class WeaponSwapChoiceUI : MonoBehaviour
    {
        // Evento que se dispara cuando el jugador selecciona esta opción
        public event Action<WeaponSwapChoice> OnChoiceSelected;

        // Referencia al componente UI que muestra la información del arma
        [SerializeField] private WeaponInfoUI weaponInfoUI;

        // Input asociado (por ejemplo, Izquierda o Derecha para dos slots de arma)
        [SerializeField] private CombatInputs input;

        // Botón UI que el jugador presiona para elegir esta opción
        [SerializeField] private Button button;

        // La elección de arma específica que se mostrará y seleccionará
        private WeaponSwapChoice weaponSwapChoice;

        // Método público que recibe un arreglo de posibles elecciones y toma la que corresponde a este input
        public void TakeRelevantChoice(WeaponSwapChoice[] choices)
        {
            var inputIndex = (int)input;

            // Si no hay una elección correspondiente, no hacer nada
            if (choices.Length <= inputIndex)
            {
                return;
            }

            // Asigna y muestra la elección relevante
            SetChoice(choices[inputIndex]);
        }

        // Asigna internamente la elección y actualiza la UI con la información del arma
        private void SetChoice(WeaponSwapChoice choice)
        {
            weaponSwapChoice = choice;

            // Rellena el panel UI con los datos del arma
            weaponInfoUI.PopulateUI(choice.WeaponData);
        }

        // Método llamado cuando el botón es presionado
        private void HandleClick()
        {
            // Dispara el evento para notificar que esta opción fue seleccionada
            OnChoiceSelected?.Invoke(weaponSwapChoice);
        }
      
        // Se suscribe al evento del botón cuando el objeto se habilita
        private void OnEnable()
        {
           // Debug.Log("Enable");
            button.onClick.AddListener(HandleClick);
        }

        // Se desuscribe del evento cuando el objeto se deshabilita
        private void OnDisable()
        {
            button.onClick.RemoveListener(HandleClick);
        }
    }
}
