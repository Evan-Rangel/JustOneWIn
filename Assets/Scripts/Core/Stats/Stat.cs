using System;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script representa una estadística de un personaje o entidad (como vida, poise, 
energía, etc.), controlando su valor máximo, valor actual y reaccionando cuando este llega 
a cero.
Funciones principales:
-Init(): Inicializa el valor actual (CurrentValue) al máximo (MaxValue).
-Increase(float amount): Aumenta el valor actual.
-Decrease(float amount): Disminuye el valor actual.
-OnCurrentValueZero: Evento que se dispara automáticamente si CurrentValue llega a 0, 
permitiendo reaccionar (ej: muerte del personaje, stun, etc.).
---------------------------------------------------------------------------------------------*/

namespace Avocado.CoreSystem.StatsSystem
{
    [Serializable] 
    public class Stat
    {
        // Evento que se dispara cuando el valor actual llega a cero
        public event Action OnCurrentValueZero;

        [field: SerializeField] public float MaxValue { get; private set; }

        public float CurrentValue
        {
            get => currentValue;
            set
            {
                // Clamp para que CurrentValue siempre esté entre 0 y MaxValue
                currentValue = Mathf.Clamp(value, 0f, MaxValue);

                // Si llega a 0, dispara el evento
                if (currentValue <= 0f)
                {
                    OnCurrentValueZero?.Invoke();
                }
            }
        }

        private float currentValue;

        // Inicializa el Stat seteando CurrentValue a su MaxValue
        public void Init() => CurrentValue = MaxValue;

        // Aumenta el valor actual en una cantidad dada
        public void Increase(float amount) => CurrentValue += amount;

        // Disminuye el valor actual en una cantidad dada
        public void Decrease(float amount) => CurrentValue -= amount;
    }
}
