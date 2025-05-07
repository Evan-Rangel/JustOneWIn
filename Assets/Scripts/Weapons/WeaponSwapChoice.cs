using System;

/*---------------------------------------------------------------------------------------------
Este script define un sistema de intercambio de armas:
-WeaponSwapChoiceRequest encapsula una solicitud de cambio de arma, incluyendo:
-El arma nueva (NewWeaponData).
-Las opciones disponibles en el inventario para ser reemplazadas (Choices).
-Un callback (Callback) que se ejecuta cuando el jugador elige una opción.
WeaponSwapChoice representa cada opción individual de intercambio, con:
-El WeaponDataSO actual en un slot.
-El índice del slot correspondiente.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons
{
    public class WeaponSwapChoiceRequest
    {
        // Arreglo de opciones de intercambio 
        public WeaponSwapChoice[] Choices { get; }

        // Nueva arma que se quiere introducir en el inventario
        public WeaponDataSO NewWeaponData { get; }

        // Callback que se ejecuta cuando el jugador elige una de las opciones
        public Action<WeaponSwapChoice> Callback;

        public WeaponSwapChoiceRequest(Action<WeaponSwapChoice> callback, WeaponSwapChoice[] choices, WeaponDataSO newWeaponData)
        {
            Callback = callback;
            Choices = choices;
            NewWeaponData = newWeaponData;
        }
    }

    // Representa una de las posibles elecciones que el jugador puede hacer durante un intercambio de armas.
    public class WeaponSwapChoice
    {
        // Datos del arma existente en el inventario
        public WeaponDataSO WeaponData { get; }

        // Índice del slot donde se encuentra esta arma
        public int Index { get; }

        public WeaponSwapChoice(WeaponDataSO weaponData, int index)
        {
            WeaponData = weaponData;
            Index = index;
        }
    }
}
