using System;
using Avocado.Weapons;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script gestionar el inventario de armas de un personaje (usar, cambiar, obtener armas).
Funciones principales:
-TrySetWeapon: Cambiar un arma en un slot específico y disparar un evento de cambio.
-TryGetWeapon: Obtener el arma en un slot dado si existe.
-TryGetEmptyIndex: Buscar el primer slot vacío disponible.
-GetWeaponSwapChoices: Obtener una lista de elecciones para intercambiar armas 
(útil para UI o sistemas de swap).
---------------------------------------------------------------------------------------------*/

namespace Avocado.CoreSystem
{
    public class WeaponInventory : CoreComponent
    {
        // Evento que se dispara cuando se cambia un arma en el inventario
        public event Action<int, WeaponDataSO> OnWeaponDataChanged;

        [field: SerializeField] public WeaponDataSO[] weaponData { get; private set; }

        // Intenta establecer un arma nueva en un índice específico del inventario
        public bool TrySetWeapon(WeaponDataSO newData, int index, out WeaponDataSO oldData)
        {
            // Si el índice está fuera de rango, falla
            if (index >= weaponData.Length)
            {
                oldData = null;
                return false;
            }

            // Guarda el arma anterior antes de reemplazar
            oldData = weaponData[index];
            weaponData[index] = newData;

            // Dispara evento notificando el cambio
            OnWeaponDataChanged?.Invoke(index, newData);

            return true;
        }

        // Intenta obtener un arma de un índice específico
        public bool TryGetWeapon(int index, out WeaponDataSO data)
        {
            if (index >= weaponData.Length)
            {
                data = null;
                return false;
            }

            data = weaponData[index];
            return true;
        }

        // Busca un índice libre (sin arma asignada) en el inventario
        public bool TryGetEmptyIndex(out int index)
        {
            for (var i = 0; i < weaponData.Length; i++)
            {
                if (weaponData[i] is not null)
                    continue;

                index = i;
                return true;
            }

            index = -1;
            return false;
        }

        // Genera una lista de opciones de intercambio de armas basada en el inventario actual
        public WeaponSwapChoice[] GetWeaponSwapChoices()
        {
            var choices = new WeaponSwapChoice[weaponData.Length];

            for (var i = 0; i < weaponData.Length; i++)
            {
                var data = weaponData[i];

                choices[i] = new WeaponSwapChoice(data, i);
            }

            return choices;
        }
    }
}
