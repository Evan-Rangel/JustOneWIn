using System;
using Avocado.Weapons;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script permite probar manualmente en el editor de Unity el cambio de armas de un 
WeaponInventory mientras el juego está corriendo.
Funciones principales:
-En el Inspector, puedes dar clic derecho en el componente y ejecutar "Change Weapon" 
para asignar el arma (WeaponDataSO) al índice del inventario (CombatInputs).
-Solo funciona si el juego está en ejecución (Application.isPlaying).
---------------------------------------------------------------------------------------------*/

namespace Avocado.CoreSystem.Testers
{
    // Clase de prueba/debug para cambiar el arma equipada
    public class DEBUG_WeaponInventory : MonoBehaviour
    {
        [field: SerializeField] public WeaponDataSO WeaponData { get; private set; }

        [field: SerializeField] public CombatInputs CombatInput { get; private set; }

        private WeaponInventory weaponInventory;

        private void Awake()
        {
            // Obtiene el componente WeaponInventory en el mismo GameObject
            weaponInventory = GetComponent<WeaponInventory>();
        }

        // Método que se puede ejecutar desde el inspector (clic derecho -> "Change Weapon")
        [ContextMenu("Change Weapon")]
        private void ChangeWeaponData()
        {
            // Solo permite ejecutar si el juego está en Play Mode
            if (!Application.isPlaying)
                return;

            // Intenta establecer un arma nueva en el slot correspondiente
            weaponInventory.TrySetWeapon(WeaponData, (int)CombatInput, out _);
        }
    }
}
