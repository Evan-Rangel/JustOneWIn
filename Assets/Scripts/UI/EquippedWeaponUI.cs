using System;
using Avocado.CoreSystem;
using Avocado.Weapons;
using UnityEngine;
using UnityEngine.UI;

/*---------------------------------------------------------------------------------------------
Este script forma parte del sistema de interfaz de usuario para mostrar qué arma está equipada 
en un slot específico del jugador. Escucha cambios en el WeaponInventory y actualiza el ícono 
de la UI según el arma correspondiente a la entrada (input) asignada. Es útil para mostrar en 
pantalla qué armas están equipadas en distintos botones o inputs del jugador.
---------------------------------------------------------------------------------------------*/

namespace Avocado.UI
{
    public class EquippedWeaponUI : MonoBehaviour
    {
        // Referencia a la imagen del icono del arma en la UI
        [SerializeField] private Image weaponIcon;

        // El índice del input que representa el slot (por ejemplo: ataque principal, secundario, etc.)
        [SerializeField] private CombatInputs input;

        // Inventario de armas del jugador desde el cual se obtiene el arma equipada
        [SerializeField] private WeaponInventory weaponInventory;

        // Referencia al ScriptableObject que contiene los datos del arma actual
        private WeaponDataSO weaponData;

        // Método para actualizar el icono del arma en la UI
        private void SetWeaponIcon()
        {
            // Si hay un arma equipada, muestra su sprite; si no, oculta el ícono
            weaponIcon.sprite = weaponData ? weaponData.Icon : null;
            weaponIcon.color = weaponData ? Color.white : Color.clear;
        }

        // Callback que se ejecuta cuando se cambia un arma del inventario
        private void HandleWeaponDataChanged(int inputIndex, WeaponDataSO data)
        {
            // Solo actualiza si el cambio corresponde al slot de este UI
            if (inputIndex != (int)input)
                return;

            weaponData = data;
            SetWeaponIcon();
        }

        // Se ejecuta al iniciar la escena. Inicializa la UI con el arma equipada actualmente
        private void Start()
        {
            weaponInventory.TryGetWeapon((int)input, out weaponData);
            SetWeaponIcon();
        }

        // Suscribe el evento cuando este objeto se activa
        private void OnEnable()
        {
            weaponInventory.OnWeaponDataChanged += HandleWeaponDataChanged;
        }

        // Cancela la suscripción al evento cuando este objeto se desactiva
        private void OnDisable()
        {
            weaponInventory.OnWeaponDataChanged -= HandleWeaponDataChanged;
        }
    }
}
