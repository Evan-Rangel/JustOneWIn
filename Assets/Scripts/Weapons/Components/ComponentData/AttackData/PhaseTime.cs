using System;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
PhaseTime es una estructura que se usa para definir cuándo (en relación al tiempo actual) debe 
comenzar una fase específica de un ataque, como "startup", "active", "recovery", etc.
El campo Duration representa cuántos segundos deben pasar desde el momento actual (Time.time) 
para que se active.
Phase define a qué fase del ataque se refiere esta duración.
El método TryGetTriggerTime() calcula el tiempo futuro en el que debería ejecutarse esta fase 
solo si la fase pasada como parámetro coincide con la definida en el struct.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    [Serializable]
    public struct PhaseTime
    {
        // Cuánto tiempo (en segundos) dura esta fase o cuánto tiempo después se activa.
        [field: SerializeField] public float Duration { get; private set; }

        // A qué fase del ataque corresponde esta duración.
        [field: SerializeField] public AttackPhases Phase { get; private set; }

        /*
         * Intenta obtener el tiempo en el que se debe activar esta fase específica.
         * Devuelve true si la fase proporcionada coincide con la que contiene este struct.
         * El triggerTime es el tiempo actual + la duración.
         */
        public bool TryGetTriggerTime(AttackPhases phase, out float triggerTime)
        {
            // Calcula el momento de activación basado en el tiempo actual + duración
            triggerTime = Time.time + Duration;

            // Solo retorna true si la fase pasada como parámetro coincide con la fase de este struct
            return phase == Phase;
        }
    }
}
